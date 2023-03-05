using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataParser.SupportingLogics
{

    //fake
    public class MachineTranslation
    {
        public static async Task<string?> TranslateSingleStroke(string textToTranslate)
        {
            string? translatedText = null;
            await Task.Run(() =>
            {
                CultureInfo russianCulture = CultureInfo.GetCultureInfo("ru-RU");
                CultureInfo englishCulture = CultureInfo.GetCultureInfo("en-US");

                string translatedText = englishCulture.TextInfo.ToTitleCase(russianCulture.TextInfo.ToLower(textToTranslate));
            });
            return translatedText;

        }
    }
}
