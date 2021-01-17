using mshtml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AMS.CustomTextEditor
{
    public class Gui
    {
        public static WPFWebBrowser webBrowser;

        public static List<string> ComboboxFontSizeInitialisation()
        {
            List<string> items = new List<string>();

            items.Add("8.5");
            items.Add("10.5");
            items.Add("12");
            items.Add("14");
            items.Add("18");
            items.Add("24");
            items.Add("36");
            return items;
        }

        public static void SettingsFontColor()
        {

        }

        public static void RibbonComboboxFonts(ComboBox RibbonComboboxFonts)
        {
            var doc = webBrowser.webBrowser.Document as HTMLDocument;
            if (doc != null)
            {
                doc.execCommand("FontName", false, RibbonComboboxFonts.SelectedItem.ToString());
            }
        }

        public static void newdocument()
        {
            //webBrowser.newWb("");
        }
    }
}
