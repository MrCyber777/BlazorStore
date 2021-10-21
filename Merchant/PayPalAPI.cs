using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BlazorStore.Merchant
{
    public class PayPalAPI
    {
        public IConfiguration configuration { get; }

        public PayPalAPI(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        public async Task<string> getRedirectUrlToPayPalAsync(double total, string currency, int appointmentId)
        {
            try
            {
                return Task.Run(async () =>
               {
                   HttpClient http = GetPayPalHttpClient();
                   PaypalAccessToken accessToken = await GetPayPalAccessTokenAsync(http);
                   PayPalRequest createdPayment = await CreatePaymentAsync(http, accessToken, total, currency, appointmentId);
                   return createdPayment.links.First(x => x.rel == "approval_url").href; // Обращение к созданному заказу, массив Link, получаем link, если содержит approval url
                }).Result;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex, "Failed to login to PayPal");
                return null;
            }
        }

        public async Task<PayPalResponse> executedPayment(string paymentId, string payerId)
        {
            try
            {
                HttpClient http = GetPayPalHttpClient();
                PaypalAccessToken accessToken = await GetPayPalAccessTokenAsync(http);
                return await ExecutePayPalPaymentAsync(http, accessToken, paymentId, payerId);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex, "Failed to login to PayPal");
                return null;
            }
        }

        private HttpClient GetPayPalHttpClient()
        {
            //Получаем конфигурацию API url PayPal из appsettings.json
            var payPalConfig = configuration["PayPal:urlAPI"];
            var http = new HttpClient
            {
                BaseAddress = new Uri(payPalConfig),
                Timeout = TimeSpan.FromSeconds(30)
            };
            return http;
        }

        private async Task<PaypalAccessToken> GetPayPalAccessTokenAsync(HttpClient http)
        {
            byte[] bytes = Encoding.GetEncoding("iso-8859-1").GetBytes($"{configuration["PayPal:clientId"]}:{configuration["PayPal:secret"]}");// закодированный массив байтов
            HttpRequestMessage request = new(HttpMethod.Post, "/v1/oauth2/token");
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(bytes));//добавление авторизационных заголовков
            var form = new Dictionary<string, string>
            {
                ["grant_type"] = "client_credentials"
            };
            request.Content = new FormUrlEncodedContent(form);// заставляем PayPal вернуть информацию о заказе
            HttpResponseMessage response = await http.SendAsync(request);

            var content = await response.Content.ReadAsStringAsync();
            PaypalAccessToken accessToken = JsonConvert.DeserializeObject<PaypalAccessToken>(content);

            return accessToken;
        }

        private async Task<PayPalRequest> CreatePaymentAsync(HttpClient http, PaypalAccessToken accessToken, double total, string currency, int appointmentId)
        {
            HttpRequestMessage request = new(HttpMethod.Post, "/v1/payments/payment");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.access_token);
            JObject payment = JObject.FromObject(new
            {
                intent = "sale",
                redirect_urls = new
                {
                    return_url = configuration["PayPal:returnUrl"],
                    cancel_url = configuration["PayPal:cancelUrl"]
                },
                payer = new { payment_method = "paypal" },
                transactions = JArray.FromObject(new[]
                {
                   new
                   {
                       amount=new
                       {
                           total=total,
                           currency=currency
                       },
                       description = appointmentId.ToString()
                   }
                })
            });
            request.Content = new StringContent(JsonConvert.SerializeObject(payment), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await http.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            PayPalRequest createdPayPalPayment = JsonConvert.DeserializeObject<PayPalRequest>(content);
            return createdPayPalPayment;
        }

        private async Task<PayPalResponse> ExecutePayPalPaymentAsync(HttpClient http, PaypalAccessToken accessToken, string paymentId, string payerId)
        {
            HttpRequestMessage request = new(HttpMethod.Post, $"v1/payments/payment/{paymentId}/execute");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.access_token);
            JObject payment = JObject.FromObject(new
            {
                payer_id = payerId
            });
            request.Content = new StringContent(JsonConvert.SerializeObject(payment), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await http.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            PayPalResponse executedPayment = JsonConvert.DeserializeObject<PayPalResponse>(content);

            return executedPayment;
        }
    }
}