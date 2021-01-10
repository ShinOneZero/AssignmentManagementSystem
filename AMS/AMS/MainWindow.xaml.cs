using AMS.Database;
using AMS.Pages;
using System;
using System.Collections.Generic;
using System.Data;
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

namespace AMS
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Page> pages;

        DataTable m_UserInfo;
        public MainWindow()
        {
            InitializeComponent();
            pages = new List<Page>();
            mainFrame.Navigate(new Uri("/Pages/LoginPage.xaml", UriKind.RelativeOrAbsolute));
        }

        public void InitPage(DataTable Info)
        {
            m_UserInfo = Info;

            TopPanel.Visibility = Visibility.Visible;
            GridMenu.Visibility = Visibility.Visible;

            UserName.Text = m_UserInfo.Rows[0]["USER_NAME"].ToString();

            pages.Add(new MyRequestPage());
            pages.Add(new AllRequestPage());

            mainFrame.Content = pages[0];
        }

        public DataTable GetUserInfo()
        {
            return m_UserInfo;
        }

        private void ButtonOpenMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonOpenMenu.Visibility = Visibility.Collapsed;
            ButtonCloseMenu.Visibility = Visibility.Visible;
        }

        private void ButtonCloseMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonOpenMenu.Visibility = Visibility.Visible;
            ButtonCloseMenu.Visibility = Visibility.Collapsed;
        }

        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SettingButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ListViewMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(mainFrame != null)
            {
                var item = (ListBox)sender;
                mainFrame.Content = pages[item.SelectedIndex];
            }
        }
    }
}
