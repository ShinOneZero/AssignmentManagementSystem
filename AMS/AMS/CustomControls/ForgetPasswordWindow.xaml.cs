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

namespace AMS.CustomControls
{
    /// <summary>
    /// ForgetPasswordWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ForgetPasswordWindow : UserControl
    {
        #region Members
        private MainWindow mainWindow { get => Application.Current.MainWindow as MainWindow; }
        #endregion

        public ForgetPasswordWindow()
        {
            InitializeComponent();
            frame.Navigate(new Uri("/Pages/ForgetPasswordPage1.xaml", UriKind.RelativeOrAbsolute));
        }

        private void btn_Exit_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.Overlay.Visibility = Visibility.Collapsed;
            mainWindow.popup.Visibility = Visibility.Collapsed;
            mainWindow.popup.Content = null;
        }
    }
}
