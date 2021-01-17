using mshtml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// CustomWebBrowser.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class WPFWebBrowser : UserControl
    {
        public IHTMLDocument2 doc;
        //public WebBrowser webBrowser;

        public WPFWebBrowser()
        {
            InitializeComponent();

            webBrowser.NavigateToString(Properties.Resources.New);
            doc = webBrowser.Document as IHTMLDocument2;
            doc.designMode = "On";
        }

        public string GetHTML()
        {
            return doc.body.innerHTML;
        }

        public void SetHTML(string html)
        {
            if (!String.IsNullOrEmpty(html))
            {
                doc.write(html);
            }
        }

        #region Format
        public void FontHeight(ComboBox fontsize)
        {
            if (doc != null)
            {
                doc.execCommand("FontSize", false, fontsize.SelectedItem);
            }
        }

        public void Fonts(ComboBox fonts)
        {
            if (doc != null)
            {
                doc.execCommand("FontName", false, fonts.SelectedItem.ToString());
            }
        }

        public void bold()
        {
            if (doc != null)
            {
                doc.execCommand("Bold", false, null);
            }
        }

        public void Italic()
        {
            if (doc != null)
            {
                doc.execCommand("Italic", false, null);
            }
        }

        public void Underline()
        {
            if (doc != null)
            {
                doc.execCommand("Underline", false, null);
            }
        }

        public void JustifyLeft()
        {
            if (doc != null)
            {
                doc.execCommand("JustifyLeft", false, null);
            }
        }

        public void JustifyCenter()
        {
            if (doc != null)
            {
                doc.execCommand("JustifyCenter", false, null);
            }
        }

        public void JustifyRight()
        {
            if (doc != null)
            {
                doc.execCommand("JustifyRight", false, null);
            }
        }

        public void JustifyFull()
        {
            if (doc != null)
            {
                doc.execCommand("JustifyFull", false, null);
            }
        }


        public void InsertOrderedList()
        {
            if (doc != null)
            {
                doc.execCommand("InsertOrderedList", false, null);
            }
        }

        public void InsertUnorderedList()
        {
            if (doc != null)
            {
                doc.execCommand("InsertUnorderedList", false, null);
            }
        }

        public void Outdent()
        {
            if (doc != null)
            {
                doc.execCommand("Outdent", false, null);
            }
        }

        public void Indent()
        {
            if (doc != null)
            {
                doc.execCommand("Indent", false, null);
            }
        }

        #endregion

        private void webBrowser_LoadCompleted(object sender, NavigationEventArgs e)
        {
            doc = webBrowser.Document as IHTMLDocument2;
            doc.designMode = "On";
        }
    }
}
