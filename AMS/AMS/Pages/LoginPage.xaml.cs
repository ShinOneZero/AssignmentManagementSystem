using AMS.CustomControls;
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

namespace AMS.Pages
{
    /// <summary>
    /// Page1.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class LoginPage : Page
    {
        #region Members
        private DelegateCommand forgetPasswordCommand;
        private MainWindow mainWindow { get => Application.Current.MainWindow as MainWindow; }
        #endregion

        #region Interface
        public DelegateCommand ForgetPasswordCommand
        {
            get { return forgetPasswordCommand; }
        }
        #endregion
        public LoginPage()
        {
            InitializeComponent();
            DataContext = this;
            forgetPasswordCommand = new DelegateCommand( x=> ForgetPassword(x));
        }
        //Show Main Page on Login Button Click
        
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.mainFrame.Navigate(new Uri("/Pages/DashboardPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void ForgetPassword(object e)
        {
            mainWindow.Overlay.Visibility = Visibility.Visible;
            mainWindow.popup.Visibility = Visibility.Visible;
            mainWindow.popup.Content = new ForgetPasswordWindow();
        }
    }
}
