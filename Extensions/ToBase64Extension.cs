using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace BlazorStore.Extensions
{
    public static class ToBase64Extension
    {
        
        public static string ToBase64(this string text)
        {
            byte[] byteText = System.Text.Encoding.UTF8.GetBytes(text);
            string base64String = System.Convert.ToBase64String(byteText);
            return base64String;
        }
        public static string FromBase64(this string base64Data)
        {
            byte[] byteText = System.Convert.FromBase64String(base64Data);
            string text = System.Text.Encoding.UTF8.GetString(byteText);
            return text;
        }
    }
}
