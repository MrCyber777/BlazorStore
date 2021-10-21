using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace BlazorStore.Extensions
{
    public static class SessionExtension
    {
        public delegate void NotifyDelegate(int count);
        public static event NotifyDelegate NotifyChanges;

        public static async Task SetForSession<T>(this ProtectedLocalStorage session, string key, T value)
        {
            await session.SetAsync(key, JsonSerializer.Serialize(value));// 1) Получаем текущую сессию пользователя. 2) Установи в эту сессию асинхронно некоторые значения.
            //1) Параметр ключ для доступа к значениям. 2) Передаем сериализованное в Json формат значения  

            var newList = await session.GetFromSession<List<int>>(key);
            NotifyChanges?.Invoke(newList.Count);
            /*
             void ShowChanges(int count)
            {
                productCount = count;
                InvokeAsync(StateHasChanged);
            }
             */
        }
        public static async Task<T> GetFromSession<T>(this ProtectedLocalStorage session, string key)
        {
            var value = await session.GetAsync<string>(key);
            return (value.Value == null) ? default : JsonSerializer.Deserialize<T>(value.Value);
        }
    }
}
