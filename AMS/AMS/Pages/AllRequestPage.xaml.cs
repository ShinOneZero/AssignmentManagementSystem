using AMS.CustomClass;
using AMS.CustomControls;
using AMS.Database;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace AMS.Pages
{
    /// <summary>
    /// AllRequestPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class AllRequestPage : Page
    {
        MainWindow mainWindow { get => Application.Current.MainWindow as MainWindow; }
        DataTable m_AllRequests;
        ObservableCollection<RequestInfo> AllRequest_List;
        public AllRequestPage()
        {
            InitializeComponent();
            RefreshData();
        }

        public void RefreshData()
        {
            AllRequest_List = new ObservableCollection<RequestInfo>();
            DataBase db = new DataBase();
            m_AllRequests = db.RequestData("SELECT A.REQUEST_NO, A.REQUEST_DATE, A.CREATION_TIMESTAMP, A.LAST_UPDATE_TIMESTAMP, A.REQUEST_STATE, A.REQUEST_END_DATE, A.PRODUCT, A.REQUESTER_EMP_NO, " +
                                                        "A.PERFORMER_EMP_NO, A.TITLE, A.R_CONTENT, A.P_CONTENT, A.REQUEST_HOPE_END_DATE, A.PROCESS_END_DATE, A.PROCESS_START_DATE, " +
                                                        "(SELECT USER_NAME FROM USER_INFO S WHERE S.USER_NO = A.REQUESTER_EMP_NO)  AS REQUESTER_EMP_NAME, " +
                                                        "(SELECT USER_NAME FROM USER_INFO S WHERE S.USER_NO = A.PERFORMER_EMP_NO) AS PERFORMAER_EMP_NAME " +
                                                        "FROM REQUEST_INFO A " +
                                                        "WHERE A.REQUEST_STATE <> 9");

            if (m_AllRequests != null)
            {
                foreach (DataRow dr in m_AllRequests.Rows)
                {
                    AllRequest_List.Add(new RequestInfo
                    {
                        Request_No = (string)dr["REQUEST_NO"],
                        Request_Date = (DateTime)dr["REQUEST_DATE"],
                        Creation_TimeStamp = (DateTime)dr["CREATION_TIMESTAMP"],
                        Last_Update_TimeStamp = (DateTime)dr["LAST_UPDATE_TIMESTAMP"],
                        Request_State_Name = (int)dr["REQUEST_STATE"] == 1 ? "접수완료" :
                                                                    (int)dr["REQUEST_STATE"] == 2 ? "처리중" :
                                                                            (int)dr["REQUEST_STATE"] == 3 ? "완료" : "취소",
                        Request_State = (int)dr["REQUEST_STATE"],
                        Request_End_Date = (DateTime)dr["REQUEST_END_DATE"],
                        Product = (string)dr["PRODUCT"],
                        Requester_Emp_No = (int)dr["REQUESTER_EMP_NO"],
                        Performer_Emp_No = (int)dr["REQUESTER_EMP_NO"],
                        Title = (string)dr["TITLE"],
                        R_Content = (string)dr["R_CONTENT"],
                        P_Content = (string)dr["P_CONTENT"],
                        Request_Hope_End_Date = (DateTime)dr["REQUEST_HOPE_END_DATE"],
                        Process_Start_Date = dr["PROCESS_START_DATE"] == DBNull.Value ? null : (Nullable<DateTime>)dr["PROCESS_START_DATE"],
                        Process_End_Date = dr["PROCESS_END_DATE"] == DBNull.Value ? null : (Nullable<DateTime>)dr["PROCESS_END_DATE"],
                        Requester_Emp_Name = dr["REQUESTER_EMP_NAME"] == DBNull.Value ? null : (string)dr["REQUESTER_EMP_NAME"],
                        Performer_Emp_Name = dr["PERFORMAER_EMP_NAME"] == DBNull.Value ? "" : (string)dr["PERFORMAER_EMP_NAME"]
                    });

                    AllRequestList.ItemsSource = AllRequest_List;
                }
            }
        }

        private void SearchKeywordButtton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SearchDetailButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ExportButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SearchKeywordButtton_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void tbx_SearchText_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void AllRequestList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            mainWindow.popup.Content = new DetailRequestInfoView(this, (RequestInfo)AllRequestList.SelectedItem, VIEW_MODE.READ_MODE);
            mainWindow.Overlay.Visibility = Visibility.Visible;
            mainWindow.popup.Visibility = Visibility.Visible;
        }
    }
}
