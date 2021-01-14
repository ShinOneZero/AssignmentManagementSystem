using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.CustomClass
{
    public enum VIEW_MODE
    {
        READ_MODE=0,
        WRITE_MODE
    }
    public enum STATE_NAME
    {
        REQUESTING= 1,
        PROCESSING,
        COMPLETE
    }
    public class RequestInfo
    {
        public RequestInfo()
        {
            Request_No = Requester_Emp_Name = Performer_Emp_Name = Product = Title = R_Content = P_Content  = Request_State_Name = "";
            Request_State = Performer_Emp_No = Requester_Emp_No = -1;
            Request_Date = Creation_TimeStamp = Last_Update_TimeStamp = Request_End_Date = null;
        }
        public string Request_No { get; set; }
        public string Request_State_Name { get; set; }
        public int Request_State { get; set; }

        public Nullable<DateTime> Request_Date { get; set; }
        public Nullable<DateTime> Creation_TimeStamp { get; set; }
        public Nullable<DateTime> Last_Update_TimeStamp { get; set; }
        public Nullable<DateTime> Request_End_Date { get; set; }
        public Nullable<DateTime> Request_Hope_End_Date { get; set; }
        public Nullable<DateTime> Process_Start_Date { get; set; }
        public Nullable<DateTime> Process_End_Date { get; set; }

        public int Performer_Emp_No { get; set; }
        public int Requester_Emp_No { get; set; }
        public string Requester_Emp_Name { get; set; }
        public string Performer_Emp_Name { get; set; }

        public string Product { get; set; }
        public string Title { get; set; }
        public string R_Content { get; set; }
        public string P_Content { get; set; }
    }
}
