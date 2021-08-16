using System;
using System.Collections.Generic;
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
                return "";
            var base64 = Convert.ToBase64String(image);
            var finalString = string.Format("data:image/jpg;base64,{0}", base64);

            return finalString;
        }     
    }
}
