using System;

namespace BlazorStore.Merchant
{
    public class PayPalRequest
    {
        public string id { get; set; }
        public string intent { get; set; }
        public string state { get; set; }
        public Payer payer { get; set; }
        public Transaction[] transactions { get; set; }
        public DateTime create_time { get; set; }
        public Link[] links { get; set; }

        public class Payer
        {
            public string payment_method { get; set; }
        }

        public class Transaction
        {
            public Amount amount { get; set; }
            public object[] related_resources { get; set; }
            public string description { get; set; }
        }

        public class Amount
        {
            public string total { get; set; }
            public string currency { get; set; }
        }

        public class Link
        {
            public string href { get; set; }//адрес запроса
            public string rel { get; set; }//return url
            public string method { get; set; }
        }
    }
}