using BlazorInputFile;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorStore.Utility
{
    public static class UploadImageHelper
    {
        public static async Task<byte[]> HandleUpoad(IFileListEntry[] files)
        {           
            var file = files.FirstOrDefault();
            if (file is not null)
            {
                MemoryStream memoryStream = new();
                await file.Data.CopyToAsync(memoryStream);// асинхронно скопировать картинку в ОП
                var ImageUploaded = memoryStream.ToArray();//Преобразование картинки в массив байтов
                return ImageUploaded;
            }
            return null;
        }
    }
}
