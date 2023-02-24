using Microsoft.Extensions.Hosting;
using System.IO;

namespace MyPet.Areas.SomeLogics
{
    public class SecialFileSaver
    {
        public static async Task SaveImageFromForm(string path, IFormFile image)
        {
            if (image != null && image.Length > 0)
            {
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }
            }
        }
    }
}

