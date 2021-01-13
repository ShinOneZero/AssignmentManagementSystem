using AMS.CustomClass;
using AMS.Database;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
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
    /// DetailRequestInfoView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class DetailRequestInfoView : UserControl
    {
        private MainWindow mainWindow { get => Application.Current.MainWindow as MainWindow; }
        private DataBase db = new DataBase();
        private RequestInfo m_RequestInfo;

        public DetailRequestInfoView(RequestInfo requestInfo, VIEW_MODE mode)
        {
            InitializeComponent();

            if(requestInfo != null)
            {
                m_RequestInfo = requestInfo;
            }
            else
            {
                m_RequestInfo = new RequestInfo();
                m_RequestInfo.Request_No = db.RequestData("SELECT IIF(MAX(IIF(LEFT(REQUEST_NO, 2) = FORMAT(DATE(), \"YY\"), REQUEST_NO, NULL))<>NULL, " +
                                                                             "MAX(IIF(LEFT(REQUEST_NO, 2) = FORMAT(DATE(), \"YY\"), LEFT(REQUEST_NO, 2) & \"-\" & FORMAT(RIGHT(REQUEST_NO, 4)+1, \"0000\"), NULL)), " +
                                                                             "(FORMAT(DATE(), \"YY\")+1) & \"-0001\") AS REQUEST_NO FROM REQUEST_INFO").Rows[0]["REQUEST_NO"].ToString();
                m_RequestInfo.Creation_TimeStamp = DateTime.Now;
                m_RequestInfo.Last_Update_TimeStamp = DateTime.Now;
                m_RequestInfo.Request_Date = DateTime.Now;
                m_RequestInfo.Requester_Emp_No = int.Parse(mainWindow.GetUserInfo().Rows[0]["USER_NO"].ToString());
                m_RequestInfo.Requester_Emp_Name = mainWindow.GetUserInfo().Rows[0]["USER_NAME"].ToString();
                m_RequestInfo.Product = "0000000";
                m_RequestInfo.Request_State = 1;
            }

            InitControl();
        }

        private void InitControl()
        {
            if(m_RequestInfo != null)
            {
                RequestNo.Text = m_RequestInfo.Request_No;
                RequestDate.SelectedDate = m_RequestInfo.Request_Date;
                RequesterName.Text = m_RequestInfo.Requester_Emp_Name + "("
                    + db.RequestData("SELECT DEPORTMENT FROM USER_INFO WHERE USER_NO = " + m_RequestInfo.Requester_Emp_No).Rows[0]["DEPORTMENT"].ToString() + ")";

                if (m_RequestInfo.Title != null)
                    Title.Text = m_RequestInfo.Title;

                if(m_RequestInfo.Product != null)
                {
                    var arr = m_RequestInfo.Product.ToArray();
                    for (int i=0; i < arr.Length; i++)
                    {
                        if(arr[i].Equals('1'))
                        {
                            switch(i)
                            {
                                case 0:
                                    Product1.IsChecked = true;
                                    break;
                                case 1:
                                    Product2.IsChecked = true;
                                    break;
                                case 2:
                                    Product3.IsChecked = true;
                                    break;
                                case 3:
                                    Product4.IsChecked = true;
                                    break;
                                case 4:
                                    Product5.IsChecked = true;
                                    break;
                                case 5:
                                    Product6.IsChecked = true;
                                    break;
                                case 6:
                                    Product7.IsChecked = true;
                                    break;
                            }
                        }
                    }
                }

                if (m_RequestInfo.R_Conent != null) ;

                if (m_RequestInfo.Process_Start_Date != DateTime.MinValue)
                    ProcessStartDate.SelectedDate = m_RequestInfo.Process_Start_Date;

                if (m_RequestInfo.Process_End_Date != DateTime.MinValue)
                    ProcessEndDate.SelectedDate = m_RequestInfo.Process_End_Date;

                if (m_RequestInfo.Performer_Emp_Name != null)
                    ProcesserName.Text = m_RequestInfo.Performer_Emp_Name + "("
                        + db.RequestData("SELECT DEPORTMENT FROM USER_INFO WHERE USER_NO = " + m_RequestInfo.Performer_Emp_No).Rows[0]["DEPORTMENT"].ToString() + ")";

                if (m_RequestInfo.P_Content != null) ;
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.popup.Content = null;
            mainWindow.Overlay.Visibility = Visibility.Collapsed;
            mainWindow.popup.Visibility = Visibility.Collapsed;
        }

        private void RequestCancelButton_Click(object sender, RoutedEventArgs e)
        {
            popup.Content = new WaitWindow("Wait...");
            Overlay.Visibility = Visibility.Visible;
            popup.Visibility = Visibility.Visible;

            string no = null;
            var data = db.RequestData("SELECT * FROM REQUEST_INFO WHERE REQUEST_NO = " + m_RequestInfo.Request_No);

            if(data.Rows.Count == 1)
                no = data.Rows[0]["REQUEST_NO"].ToString();

            if(no != null)
            {
                db.RequestSQL("UPDATE REQUEST_INFO SET REQUEST_STATE=9 WHERE REQUEST_NO = " + m_RequestInfo.Request_No);
                popup.Content = null;
                Overlay.Visibility = Visibility.Collapsed;
                popup.Visibility = Visibility.Collapsed;

                MessageBox.Show("접수가 취소되었습니다.");
                mainWindow.popup.Content = null;
                mainWindow.Overlay.Visibility = Visibility.Collapsed;
                mainWindow.popup.Visibility = Visibility.Collapsed;
            }
            else
            {
                MessageBox.Show("접수되지 않은 업무입니다.");
                popup.Content = null;
                Overlay.Visibility = Visibility.Collapsed;
                popup.Visibility = Visibility.Collapsed;
            }
        }

        private void RequestEditButton_Click(object sender, RoutedEventArgs e)
        {
            popup.Content = new WaitWindow("Wait...");
            Overlay.Visibility = Visibility.Visible;
            popup.Visibility = Visibility.Visible;

            string no = null;
            var data = db.RequestData("SELECT * FROM REQUEST_INFO WHERE REQUEST_NO = " + m_RequestInfo.Request_No);

            if (data.Rows.Count == 1)
                no = data.Rows[0]["REQUEST_NO"].ToString();

            // 제품정보가져오기
            GetProductInfo();

            // 수정
            if (no != null)
            {
                db.RequestSQL("UPDATE REQUEST_INFO SET " +
                                    "LAST_UPDATE_TIMESTAMP = " + DateTime.Now +
                                    "REQUEST_END_DATE = " + DateTime.Now +
                                    "TITLE = " + m_RequestInfo.Title +
                                    "R_CONTENT = " + m_RequestInfo.R_Conent +
                                    "REQUEST_HOPE_END_DATE = " + m_RequestInfo.Request_Hope_End_Date +
                                    "WHERE REQUEST_NO = " + m_RequestInfo.Request_No); ;
                popup.Content = null;
                Overlay.Visibility = Visibility.Collapsed;
                popup.Visibility = Visibility.Collapsed;

                MessageBox.Show("접수내용이 수정되었습니다.");
            }
            // 접수
            else
            {
                /*
                OleDbCommand cmd = new OleDbCommand("INSERT INTO REQUEST_INFO (REQUEST_NO, REQUEST_DATE, CREATION_TIMESTAMP, LAST_UPDATE_TIMESTAMP, REQUEST_STATE, " +
                                                                            "REQUEST_END_DATE, PRODUCT, REQUESTER_EMP_NO, TITLE, R_CONTENTREQUEST_HOPE_END_DATE" +
                                                                            ") VALUES (@value)");
                cmd.Parameters.AddWithValue("@value", m_RequestInfo.Request_No);
                cmd.Parameters.AddWithValue("@value", m_RequestInfo.Request_Date);
                cmd.Parameters.AddWithValue("@value", m_RequestInfo.Creation_TimeStamp);
                cmd.Parameters.AddWithValue("@value", m_RequestInfo.Last_Update_TimeStamp);
                cmd.Parameters.AddWithValue("@value", m_RequestInfo.Request_State);
                cmd.Parameters.AddWithValue("@value", m_RequestInfo.Request_End_Date);
                cmd.Parameters.AddWithValue("@value", m_RequestInfo.Product);
                cmd.Parameters.AddWithValue("@value", m_RequestInfo.Requester_Emp_No);
                cmd.Parameters.AddWithValue("@value", m_RequestInfo.Title);
                cmd.Parameters.AddWithValue("@value", m_RequestInfo.R_Conent);
                cmd.Parameters.AddWithValue("@value", m_RequestInfo.Request_Hope_End_Date);
                */
                OleDbCommand cmd = new OleDbCommand("INSERT INTO REQUEST_INFO (REQUEST_NO, REQUEST_DATE" +
                                                            ") VALUES (@value)");
                cmd.Parameters.AddWithValue("@value", m_RequestInfo.Request_No);
                cmd.Parameters.AddWithValue("@value", m_RequestInfo.Creation_TimeStamp.ToString("yyyy-MM-dd HH:mm:ss"));
                db.RequestSQL(cmd);
                popup.Content = null;
                Overlay.Visibility = Visibility.Collapsed;
                popup.Visibility = Visibility.Collapsed;
                MessageBox.Show("신규 접수가 완료되었습니다.");
            }
        }

        private void GetProductInfo()
        {
            if ((bool)Product1.IsChecked) m_RequestInfo.Product = "1";
            else m_RequestInfo.Product = "0";

            if ((bool)Product2.IsChecked) m_RequestInfo.Product += "1";
            else m_RequestInfo.Product += "0";

            if ((bool)Product3.IsChecked) m_RequestInfo.Product += "1";
            else m_RequestInfo.Product += "0";

            if ((bool)Product4.IsChecked) m_RequestInfo.Product += "1";
            else m_RequestInfo.Product += "0";

            if ((bool)Product5.IsChecked) m_RequestInfo.Product += "1";
            else m_RequestInfo.Product += "0";

            if ((bool)Product6.IsChecked) m_RequestInfo.Product += "1";
            else m_RequestInfo.Product += "0";

            if ((bool)Product7.IsChecked) m_RequestInfo.Product += "1";
            else m_RequestInfo.Product += "0";
        }

        private void ProcessEditButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ProcessComplete_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
