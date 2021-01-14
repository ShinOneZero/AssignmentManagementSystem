using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace AMS.CustomEditor
{
    public static class Initialisation
    {
        public static WebEditor webeditor;

        public static void RibbonComboboxFontsInitialisation()
        {
            var cond = System.Windows.Markup.XmlLanguage.GetLanguage(System.Globalization.CultureInfo.CurrentUICulture.Name);

            var listFont = new List<string>();
            foreach (FontFamily font in Fonts.SystemFontFamilies)
            {
                if (font.FamilyNames.ContainsKey(cond))
                    listFont.Add(font.FamilyNames[cond]);
                else
                    listFont.Add(font.ToString());
            }

            webeditor.SettingFonts.ItemsSource = listFont;
            webeditor.SettingFonts.SelectedIndex = 22;
        }

        public static void RibbonComboboxFontSizeInitialisation()
        {
            webeditor.SettingFontSize.ItemsSource = Gui.RibbonComboboxFontSizeInitialisation();
            webeditor.SettingFontSize.SelectedIndex = 3;
        }
    }
}
