using AMS.CustomControls;
using AMS.Database;
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

            this.UserId.FocusItem();
        }
        //Show Main Page on Login Button Click
        
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            Login();
        }

        private void Login()
        {
            DataBase db = new DataBase();

            if (String.IsNullOrEmpty(UserId.Text))
            {
                MessageBox.Show("아이디를 입력해주세요", "알림", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else if (String.IsNullOrEmpty(UserPassword.passBox.Password))
            {
                MessageBox.Show("비밀번호를 입력해주세요", "알림", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var userInfo = db.RequestData("SELECT * FROM USER_INFO WHERE ID = '" + UserId.Text + "' AND PASSWORD = '" + UserPassword.passBox.Password + "';");

            if (userInfo.Rows.Count > 0)
            {
                mainWindow.InitPage(userInfo);
            }
            else
            {
                MessageBox.Show("사용자 정보가 일치하지 않습니다.", "알림", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ForgetPassword(object e)
        {
            mainWindow.popup.Content = new ForgetPasswordWindow();
            mainWindow.Overlay.Visibility = Visibility.Visible;
            mainWindow.popup.Visibility = Visibility.Visible;
        }

        private void Check_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
                Login();
        }
    }
}
