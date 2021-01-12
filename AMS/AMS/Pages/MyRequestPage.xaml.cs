using AMS.CustomClass;
using AMS.CustomControls;
using AMS.Database;
using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Shapes;

namespace AMS.Pages
{
    /// <summary>
    /// MyRequestPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MyRequestPage : Page
    {
        MainWindow mainWindow { get => Application.Current.MainWindow as MainWindow; }
        DataTable m_UserInfo, m_MyRequests;
        public MyRequestPage()
        {
            InitializeComponent();

            m_UserInfo = mainWindow.GetUserInfo();
            DataBase db = new DataBase();
            m_MyRequests = db.RequestData("SELECT A.REQUEST_NO, A.REQUEST_DATE, A.CREATION_TIMESTAMP, A.LAST_UPDATE_TIMESTAMP, A.REQUEST_STATE, A.REQUEST_END_DATE, A.PRODUCT, " +
                                                        "A.REQUESTER_EMP_NO, A.PERFORMER_EMP_NO, A.TITLE, A.R_CONTENT, A.P_CONTENT, " +
                                                        "B.USER_NAME AS REQUESTER_EMP_NAME, C.USER_NAME AS PERFORMAER_EMP_NAME " +
                                                        "FROM REQUEST_INFO A, USER_INFO B, USER_INFO C " +
                                                        "WHERE (A.REQUESTER_EMP_NO = " + m_UserInfo.Rows[0]["USER_NO"].ToString() + " OR A.PERFORMER_EMP_NO = " + m_UserInfo.Rows[0]["USER_NO"].ToString() + ") " +
                                                        "AND A.REQUESTER_EMP_NO = B.USER_NO AND A.PERFORMER_EMP_NO = C.USER_NO;");

            if (m_MyRequests != null)
            {
                DataTable list = new DataTable();
                foreach(DataGridColumn col in MyRequestList.Columns)
                {
                    list.Columns.Add(col.Header.ToString());
                }

                foreach (DataRow dr in m_MyRequests.Rows)
                {
                    
                }
            }
        }

        private void SearchKeywordButtton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void tbx_SearchText_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.popup.Content = new WaitWindow();
            mainWindow.Overlay.Visibility = Visibility.Visible;
            mainWindow.popup.Visibility = Visibility.Visible;

            Thread thread = new Thread(new ThreadStart(delegate ()
            {
                DataTable table = new DataTable();
                Tools.ExportToExcel(table);

                this.Dispatcher.Invoke(new Action(delegate ()
                {
                    mainWindow.Overlay.Visibility = Visibility.Collapsed;
                    mainWindow.popup.Visibility = Visibility.Collapsed;
                    mainWindow.popup.Content = null;
                }));
            }));
            thread.Start();
        }

        private void MyRequestList_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void AddRequestButton_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.popup.Content = new DetailRequestInfoView(null);
            mainWindow.Overlay.Visibility = Visibility.Visible;
            mainWindow.popup.Visibility = Visibility.Visible;
        }
    }
}
