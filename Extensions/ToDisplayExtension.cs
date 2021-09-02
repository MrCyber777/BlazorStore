using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;


namespace BlazorStore.Extensions
{
    public static class ToDisplayExtension
    {      
        public static string ImageToDisplayConverter(this byte[] image)
        {
            if (image is null)
            {
                return "https://st4.depositphotos.com/1028437/21297/v/600/depositphotos_212975764-stock-illustration-image-available-sign.jpg";
            }
               
            var base64 = Convert.ToBase64String(image);
            var finalString = string.Format("data:image/jpg;base64,{0}", base64);

            return finalString;
        }     
    }
}
