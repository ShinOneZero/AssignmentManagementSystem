using AMS.CustomClass;
using AMS.Database;
using AMS.Pages;
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
        private VIEW_MODE m_Mode;
        private object m_Parent;

        public DetailRequestInfoView(object parent, RequestInfo requestInfo, VIEW_MODE mode)
        {
            InitializeComponent();
            m_RequestInfo = requestInfo;
            m_Mode = mode;
            m_Parent = parent;

            if (m_RequestInfo == null && mode == VIEW_MODE.WRITE_MODE)
            {
                m_RequestInfo = new RequestInfo();
                m_RequestInfo.Request_No = db.RequestData("SELECT IIF(MAX(IIF(LEFT(REQUEST_NO, 2) = FORMAT(DATE(), \"YY\"), REQUEST_NO, NULL))<>NULL, " +
                                                                             "MAX(IIF(LEFT(REQUEST_NO, 2) = FORMAT(DATE(), \"YY\"), LEFT(REQUEST_NO, 2) & \"-\" & FORMAT(RIGHT(REQUEST_NO, 4)+1, \"0000\"), NULL)), " +
                                                                             "IIF(COUNT(REQUEST_NO) = 0, (FORMAT(DATE(), \"YY\")) & \"-0001\", " +
                                                                             "(FORMAT(DATE(), \"YY\")+1) & \"-0001\")) AS REQUEST_NO FROM REQUEST_INFO").Rows[0]["REQUEST_NO"].ToString();
                m_RequestInfo.Creation_TimeStamp = DateTime.Now;
                m_RequestInfo.Last_Update_TimeStamp = DateTime.Now;
                m_RequestInfo.Request_Hope_End_Date = DateTime.Now.AddDays(-7).Date;
                m_RequestInfo.Request_Date = DateTime.Now;
                m_RequestInfo.Requester_Emp_No = int.Parse(mainWindow.GetUserInfo().Rows[0]["USER_NO"].ToString());
                m_RequestInfo.Requester_Emp_Name = mainWindow.GetUserInfo().Rows[0]["USER_NAME"].ToString();
                m_RequestInfo.Product = "0000000";
                m_RequestInfo.Request_State = 1;
            }

            InitControl();
        }

        private void ProcesserInitialized()
        {
            List<string> list = new List<string>();
            list.Add("최동수");
            list.Add("윤숙현");
            list.Add("오만석");
            list.Add("신원영");

            ProcesserName.ItemsSource = list;

            if (!String.IsNullOrEmpty(m_RequestInfo.Performer_Emp_Name))
            {
                ProcesserName.SelectedIndex = ProcesserName.Items.IndexOf(m_RequestInfo.Performer_Emp_Name);
            }
        }
        private void InitControl()
        {
            if(m_RequestInfo != null)
            {
                RequestNo.Text = m_RequestInfo.Request_No;
                RequestDate.SelectedDate = m_RequestInfo.Request_Date;
                RequesterName.Text = m_RequestInfo.Requester_Emp_Name + "("
                    + db.RequestData("SELECT DEPORTMENT FROM USER_INFO WHERE USER_NO = " + m_RequestInfo.Requester_Emp_No).Rows[0]["DEPORTMENT"].ToString() + ")";

                RequestHopeEndDate.SelectedDate = m_RequestInfo.Request_Hope_End_Date;

                if (!String.IsNullOrEmpty(m_RequestInfo.Title))
                    Title.Text = m_RequestInfo.Title;

                if(!String.IsNullOrEmpty(m_RequestInfo.Product))
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

                if (!String.IsNullOrEmpty(m_RequestInfo.R_Content)) ;
                    RequestContent.SetHTML(m_RequestInfo.R_Content);

                ProcesserInitialized();

                if (m_RequestInfo.Request_State >= 2)
                {
                    if (m_RequestInfo.Process_Start_Date != DateTime.MinValue || m_RequestInfo.Process_Start_Date != null)
                        ProcessStartDate.SelectedDate = m_RequestInfo.Process_Start_Date;

                    if (m_RequestInfo.Process_End_Date != DateTime.MinValue || m_RequestInfo.Process_End_Date != null)
                        ProcessEndDate.SelectedDate = m_RequestInfo.Process_End_Date;

                    /*
                    if (!String.IsNullOrEmpty(m_RequestInfo.Performer_Emp_Name))
                    {
                        ProcesserName.Text = m_RequestInfo.Performer_Emp_Name + "("
                            + db.RequestData("SELECT DEPORTMENT FROM USER_INFO WHERE USER_NO = " + m_RequestInfo.Performer_Emp_No).Rows[0]["DEPORTMENT"].ToString() + ")";
                    } 
                    */

                    if (!String.IsNullOrEmpty(m_RequestInfo.P_Content))
                        ProcessContent.SetHTML(m_RequestInfo.P_Content);
                }
            }

            if (!mainWindow.GetUserInfo().Rows[0]["USER_TP"].ToString().Equals("A"))
            {
                ProcesserName.IsEnabled = false;
                ProcessContent.IsEnabled = false;
                ProcessContent.Editor.doc.designMode = "Off";
                ProcessEditButton.IsEnabled = false;
                ProcessComplete.IsEnabled = false;
            }

            if(m_Mode == VIEW_MODE.READ_MODE)
            {
                RequestHopeEndDate.IsEnabled = false;
                Title.IsEnabled = false;
                Product1.IsEnabled = false;
                Product2.IsEnabled = false;
                Product3.IsEnabled = false;
                Product4.IsEnabled = false;
                Product5.IsEnabled = false;
                Product6.IsEnabled = false;
                Product7.IsEnabled = false;
                RequestContent.IsEnabled = false;
                RequestContent.Editor.doc.designMode = "Off";
                RequestCancelButton.IsEnabled = false;
                RequestEditButton.IsEnabled = false;
                ProcesserName.IsEnabled = false;
                ProcessContent.IsEnabled = false;
                ProcessContent.Editor.doc.designMode = "Off";
                ProcessEditButton.IsEnabled = false;
                ProcessComplete.IsEnabled = false;
            }

            if (m_Mode == VIEW_MODE.WRITE_MODE && mainWindow.GetUserInfo().Rows[0]["USER_TP"].ToString() != "A" && m_RequestInfo.Request_State == 3)
            {
                RequestHopeEndDate.IsEnabled = false;
                Title.IsEnabled = false;
                Product1.IsEnabled = false;
                Product2.IsEnabled = false;
                Product3.IsEnabled = false;
                Product4.IsEnabled = false;
                Product5.IsEnabled = false;
                Product6.IsEnabled = false;
                Product7.IsEnabled = false;
                RequestContent.IsEnabled = false;
                RequestContent.Editor.doc.designMode = "Off";
                RequestCancelButton.IsEnabled = false;
                RequestEditButton.IsEnabled = false;
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
            string no = null;
            var data = db.RequestData("SELECT * FROM REQUEST_INFO WHERE REQUEST_NO = '" + m_RequestInfo.Request_No + "'");

            if(data.Rows.Count == 1)
                no = data.Rows[0]["REQUEST_NO"].ToString();

            if(no != null)
            {
                db.RequestSQL("UPDATE REQUEST_INFO SET REQUEST_STATE=9 WHERE REQUEST_NO = '" + m_RequestInfo.Request_No + "'");
                RefreshData();
                MessageBox.Show("접수가 취소되었습니다.");
            }
            else
            {
                MessageBox.Show("접수되지 않은 업무입니다.");
            }

            mainWindow.popup.Content = null;
            mainWindow.Overlay.Visibility = Visibility.Collapsed;
            mainWindow.popup.Visibility = Visibility.Collapsed;
        }

        private void RefreshData()
        {
            if (m_Parent.GetType() == typeof(MyRequestPage))
            {
                ((MyRequestPage)m_Parent).RefreshData();
            }
            else
            {
                ((AllRequestPage)m_Parent).RefreshData();
            }
        }

        private void RequestEditButton_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckControls())
                return;

            string no = null;
            var data = db.RequestData("SELECT * FROM REQUEST_INFO WHERE REQUEST_NO = '" + m_RequestInfo.Request_No + "'");

            if (data.Rows.Count == 1)
                no = data.Rows[0]["REQUEST_NO"].ToString();

            GetRequestInfo();

            // 수정
            if (no != null)
            {
                OleDbCommand cmd = new OleDbCommand("UPDATE REQUEST_INFO SET " +
                                                                            "LAST_UPDATE_TIMESTAMP = @value1, " +
                                                                            "REQUEST_END_DATE = @value2, " +
                                                                            "TITLE = @value3, " +
                                                                            "R_CONTENT = @value4, " +
                                                                            "REQUEST_HOPE_END_DATE = @value5, " +
                                                                            "PRODUCT = @value6 " +
                                                                            "WHERE REQUEST_NO = '" + m_RequestInfo.Request_No + "'");

                cmd.Parameters.AddWithValue("@value1", DateTime.Now.ToString());
                cmd.Parameters.AddWithValue("@value2", DateTime.Now.ToString());
                cmd.Parameters.AddWithValue("@value3", m_RequestInfo.Title);
                cmd.Parameters.AddWithValue("@value4", m_RequestInfo.R_Content);
                cmd.Parameters.AddWithValue("@value5", m_RequestInfo.Request_Hope_End_Date.ToString());
                cmd.Parameters.AddWithValue("@value6", m_RequestInfo.Product);

                db.RequestSQL(cmd);
                RefreshData();
                MessageBox.Show("접수내용이 수정되었습니다.");
            }
            // 접수
            else
            {
                OleDbCommand cmd = new OleDbCommand("INSERT INTO REQUEST_INFO (REQUEST_NO, REQUEST_DATE, CREATION_TIMESTAMP, LAST_UPDATE_TIMESTAMP, REQUEST_STATE, " +
                                                                            "REQUEST_END_DATE, PRODUCT, REQUESTER_EMP_NO, TITLE, R_CONTENT, REQUEST_HOPE_END_DATE, P_CONTENT, PERFORMER_EMP_NO" +
                                                                            ") VALUES (@value1,@value2,@value3,@value4,@value5,@value6,@value7,@value8,@value9,@value10,@value11, @value12, @value13)");
                cmd.Parameters.AddWithValue("@value1", m_RequestInfo.Request_No);
                cmd.Parameters.AddWithValue("@value2", m_RequestInfo.Request_Date.ToString());
                cmd.Parameters.AddWithValue("@value3", m_RequestInfo.Creation_TimeStamp.ToString());
                cmd.Parameters.AddWithValue("@value4", m_RequestInfo.Last_Update_TimeStamp.ToString());
                cmd.Parameters.AddWithValue("@value5", m_RequestInfo.Request_State);
                cmd.Parameters.AddWithValue("@value6", m_RequestInfo.Request_End_Date.ToString());
                cmd.Parameters.AddWithValue("@value7", m_RequestInfo.Product);
                cmd.Parameters.AddWithValue("@value8", m_RequestInfo.Requester_Emp_No);
                cmd.Parameters.AddWithValue("@value9", m_RequestInfo.Title);
                cmd.Parameters.AddWithValue("@value10", m_RequestInfo.R_Content);
                cmd.Parameters.AddWithValue("@value11", m_RequestInfo.Request_Hope_End_Date);
                cmd.Parameters.AddWithValue("@value12", m_RequestInfo.P_Content);
                cmd.Parameters.AddWithValue("@value13", m_RequestInfo.Performer_Emp_No);

                db.RequestSQL(cmd);
                RefreshData();
                MessageBox.Show("신규 접수가 완료되었습니다.");
            }
            mainWindow.popup.Content = null;
            mainWindow.Overlay.Visibility = Visibility.Collapsed;
            mainWindow.popup.Visibility = Visibility.Collapsed;
        }

        private void GetRequestInfo()
        {
            m_RequestInfo.Last_Update_TimeStamp = DateTime.Now;
            m_RequestInfo.Request_End_Date = DateTime.Now;
            m_RequestInfo.Request_Hope_End_Date = (DateTime)RequestHopeEndDate.SelectedDate;
            m_RequestInfo.Title = Title.Text;
            m_RequestInfo.R_Content = RequestContent.GetHTML();
            m_RequestInfo.P_Content = ProcessContent.GetHTML();

            #region GetProductInfo
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
            #endregion
        }

        private void GetPerforInfo()
        {
            m_RequestInfo.Last_Update_TimeStamp = DateTime.Now;

            if(m_RequestInfo.Request_State == 1)
            {
                m_RequestInfo.Process_Start_Date = ProcessStartDate.SelectedDate;
            }

            m_RequestInfo.Performer_Emp_Name = ProcesserName.SelectedItem.ToString();
            var data = db.RequestData("SELECT USER_NO FROM USER_INFO WHERE USER_NAME = '" + m_RequestInfo.Performer_Emp_Name + "'");

            if (data.Rows.Count == 1)
                m_RequestInfo.Performer_Emp_No = int.Parse(data.Rows[0]["USER_NO"].ToString());
            else
            {
                MessageBox.Show("유효하지 않은 처리자입니다.");
            }
            m_RequestInfo.P_Content = ProcessContent.GetHTML();
        }

        private void ProcessEditButton_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckControls())
                return;

            if(ProcesserName.SelectedItem == null)
            {
                MessageBox.Show("처리자를 입력해주세요.");
                return;
            }
            GetPerforInfo();

            // 처음 처리 등록
            if (m_RequestInfo.Request_State == 1)
            {
                m_RequestInfo.Request_State = 2;

                OleDbCommand cmd = new OleDbCommand("UPDATE REQUEST_INFO SET " +
                                                                            "LAST_UPDATE_TIMESTAMP = @value1, " +
                                                                            "PROCESS_START_DATE = @value2, " +
                                                                            "REQUEST_STATE = @value3, " +
                                                                            "P_CONTENT = @value4, " +
                                                                            "PERFORMER_EMP_NO = @value5 " +
                                                                            "WHERE REQUEST_NO = '" + m_RequestInfo.Request_No + "'");

                cmd.Parameters.AddWithValue("@value1", DateTime.Now.ToString());
                cmd.Parameters.AddWithValue("@value2", DateTime.Now.ToString());
                cmd.Parameters.AddWithValue("@value3", m_RequestInfo.Request_State);
                cmd.Parameters.AddWithValue("@value4", m_RequestInfo.P_Content);
                cmd.Parameters.AddWithValue("@value5", m_RequestInfo.Performer_Emp_No);

                db.RequestSQL(cmd);
                RefreshData();
                MessageBox.Show("처리내용이 저장되었습니다.");
            }
            else
            {
                OleDbCommand cmd = new OleDbCommand("UPDATE REQUEST_INFO SET " +
                                                            "LAST_UPDATE_TIMESTAMP = @value1, " +
                                                            "P_CONTENT = @value2, " +
                                                            "WHERE REQUEST_NO = '" + m_RequestInfo.Request_No + "'");

                cmd.Parameters.AddWithValue("@value1", DateTime.Now.ToString());
                cmd.Parameters.AddWithValue("@value2", m_RequestInfo.P_Content);

                db.RequestSQL(cmd);
                RefreshData();
                MessageBox.Show("처리내용이 수정되었습니다.");
            }
            mainWindow.popup.Content = null;
            mainWindow.Overlay.Visibility = Visibility.Collapsed;
            mainWindow.popup.Visibility = Visibility.Collapsed;
        }

        private void ProcessComplete_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckControls())
                return;
            GetPerforInfo();

            if(m_RequestInfo.Request_State == 1)
            {
                MessageBox.Show("처리(수정)후 완료처리를 진행해주세요.");
                return;
            }

            m_RequestInfo.Request_State = 3;

            OleDbCommand cmd = new OleDbCommand("UPDATE REQUEST_INFO SET " +
                                                                        "LAST_UPDATE_TIMESTAMP = @value1, " +
                                                                        "PROCESS_END_DATE = @value2, " +
                                                                        "REQUEST_STATE = @value3, " +
                                                                        "P_CONTENT = @value4 " +
                                                                        "WHERE REQUEST_NO = '" + m_RequestInfo.Request_No + "'");

            cmd.Parameters.AddWithValue("@value1", DateTime.Now.ToString());
            cmd.Parameters.AddWithValue("@value2", DateTime.Now.ToString());
            cmd.Parameters.AddWithValue("@value3", m_RequestInfo.Request_State);
            cmd.Parameters.AddWithValue("@value4", m_RequestInfo.P_Content);

            db.RequestSQL(cmd);
            RefreshData();
            MessageBox.Show("요청업무가 완료 처리되었습니다.");

            mainWindow.popup.Content = null;
            mainWindow.Overlay.Visibility = Visibility.Collapsed;
            mainWindow.popup.Visibility = Visibility.Collapsed;
        }

        private void ProcesserName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private bool CheckControls()
        {
            if(m_RequestInfo != null)
            {
                #region GetProductInfo
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
                #endregion

                if (String.IsNullOrEmpty(Title.Text))
                {
                    MessageBox.Show("제목을 입력해주세요");
                    return false;
                }

                if (RequestHopeEndDate.SelectedDate == DateTime.MinValue || RequestHopeEndDate.SelectedDate == null)
                {
                    MessageBox.Show("완료 희망일자를 선택해주세요");
                    return false;
                }

                return true;
            }
            return false;
        }
    }
}
