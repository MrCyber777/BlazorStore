using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace BlazorStore.Data.Models
{
    public class PayPalResponse
    {
              
        public double GrossTotal { get; set; }
        public int InvoiceNumber { get; set; }
        public string PaymentStatus { get; set; }
        public string PayerFirstName { get; set; }
        public double PaymentFee { get; set; }
        public string BusinessEmail { get; set; }
        public string PayerEmail { get; set; }
        public string TxToken { get; set; }
        public string PayerLastName { get; set; }
        public string ReceiverEmail { get; set; }
        public string ItemName { get; set; }
        public string Currency { get; set; }
        public string TransactionId { get; set; }
        public string SubscriberId { get; set; }
        public string Custom { get; set; }

        static string authToken, txToken, query, strResponse;

        public static PayPalResponse Success(string tx)
        {
            var payPalConfig = PayPal.GetPayPalConfig();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            authToken = payPalConfig.AuthToken;
            txToken = tx;// Присваеваем данные сформированные с формы
            query = string.Format($"cmd=_notify-synch&tx={txToken}&at={authToken}");//формируем запрос 
            string url = payPalConfig.PostUrl;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);// Создаем запрос к серверу Paypal
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";// Тип контента запроса отправляется из приложения по протоколу Http из формы в закодированном виде.
            request.ContentLength = query.Length;

            //Получаем запрос, прочитываем и помещаем в оперативную память.
            StreamWriter strOut = new(request.GetRequestStream(), System.Text.Encoding.ASCII);
            strOut.Write(query);
            strOut.Close();

            //Прочитываем из оперативной памяти 
            StreamReader strIn = new(request.GetResponse().GetResponseStream());
            strResponse = strIn.ReadToEnd();
            strIn.Close();

            if (strResponse.StartsWith("SUCCESS"))
                return Parse(strResponse);
            return null;
        }
        static PayPalResponse Parse(string postData)
        {
            string sKey, sValue;
            PayPalResponse payPalResponse = new();
            try
            {
                string[] stringArr = postData.Split('\n');// Разделяем строки и заносим в массив
                for(int i = 1; i < stringArr.Length - 1; i++)
                {
                    string[] arr1 = stringArr[i].Split('=');//Разделяем строку на ключ и значение
                    sKey = arr1[0];//Получаем ключ 
                    sValue = HttpUtility.UrlDecode(arr1[1]);// Получаем значение свойства
                    switch (sKey)
                    {
                        case "mc_gross":
                            payPalResponse.GrossTotal = double.Parse(sValue, CultureInfo.InvariantCulture);
                            break;
                        case "invoce":
                            payPalResponse.InvoiceNumber = Convert.ToInt32(sValue);
                            break;
                        case "payment_status":
                            payPalResponse.PaymentStatus = sValue;
                            break;
                        case "first_name":
                            payPalResponse.PayerFirstName = sValue;
                            break;
                        case "mc_fee":
                            payPalResponse.PaymentFee = double.Parse(sValue, CultureInfo.InvariantCulture);
                            break;
                        case "business":
                            payPalResponse.BusinessEmail = sValue;
                            break;
                        case "payer_email":
                            payPalResponse.PayerEmail = sValue;
                            break;
                        case "Tx_Token":
                            payPalResponse.TxToken = sValue;
                            break;
                        case "last_name":
                            payPalResponse.PayerLastName = sValue;
                            break;
                        case "receiver_email":
                            payPalResponse.ReceiverEmail = sValue;
                            break;
                        case "item_name":
                            payPalResponse.ItemName = sValue;
                            break;
                        case "mc_currency":
                            payPalResponse.Currency = sValue;
                            break;
                        case "txn_id":
                            payPalResponse.TransactionId = sValue;
                            break;
                        case "custom":
                            payPalResponse.Custom = sValue;
                            break;
                        case "subscr_id":
                            payPalResponse.SubscriberId = sValue;
                            break;
                    }
                }
                return payPalResponse;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
    }
}
