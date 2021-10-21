namespace BlazorStore.Merchant
{
    public class PaypalAccessToken
    {
        public string scope { get; set; }// ограничение запроса
        public string access_token { get; set; }
        public string token_type { get; set; }
        public string app_id { get; set; }
        public int expires_in { get; set; }
        public string nonce { get; set; }//назначение запроса
    }
}