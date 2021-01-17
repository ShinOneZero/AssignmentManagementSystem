using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AMS.CustomTextEditor
{
    /// <summary>
    /// TextEditor.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class TextEditor : UserControl
    {
        public TextEditor()
        {
            InitializeComponent();

            FontsInitialisation();
            FontSizeInitialisation();
        }

        public void FontsInitialisation()
        {
            var cond = System.Windows.Markup.XmlLanguage.GetLanguage(System.Globalization.CultureInfo.CurrentUICulture.Name);

            List<string>listFont = new List<string>();
            foreach (FontFamily font in Fonts.SystemFontFamilies)
            {
                if (font.FamilyNames.ContainsKey(cond))
                    listFont.Add(font.FamilyNames[cond]);
                else
                    listFont.Add(font.ToString());
            }

            listFont.Sort();

            SettingFonts.ItemsSource = listFont;
        }
        public void FontSizeInitialisation()
        {
            List<string> items = new List<string>();

            for (int x = 1; x <= 7; x++)
            {
                items.Add(x.ToString());
            }
            SettingFontSize.ItemsSource = items;
        }

        public void SetHTML(string html)
        {
            Editor.SetHTML(html);
        }

        public string GetHTML()
        {
            return Editor.GetHTML();
        }

        private void SettingsBold_Click(object sender, RoutedEventArgs e)
        {
            Editor.bold();
        }

        private void SettingsItalic_Click(object sender, RoutedEventArgs e)
        {
            Editor.Italic();
        }

        private void SettingsUnderLine_Click(object sender, RoutedEventArgs e)
        {
            Editor.Underline();
        }

        private void SettingsLeftAlign_Click(object sender, RoutedEventArgs e)
        {
            Editor.JustifyLeft();
        }

        private void SettingsCenterAlign_Click(object sender, RoutedEventArgs e)
        {
            Editor.JustifyCenter();
        }

        private void SettingsRightAlign_Click(object sender, RoutedEventArgs e)
        {
            Editor.JustifyRight();
        }

        private void SettingsJustifyAlign_Click(object sender, RoutedEventArgs e)
        {
            Editor.JustifyFull();
        }

        private void SettingFonts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Editor.Fonts(SettingFonts);
        }

        private void SettingFontSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Editor.FontHeight(SettingFontSize);
        }

        private void SettingsFontColor_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
