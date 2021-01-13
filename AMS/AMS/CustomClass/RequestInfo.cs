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
    public class RequestInfo
    {
        public string Request_No { get; set; }
        public int Request_State { get; set; }

        public DateTime Request_Date { get; set; }
        public DateTime Creation_TimeStamp { get; set; }
        public DateTime Last_Update_TimeStamp { get; set; }
        public DateTime Request_End_Date { get; set; }
        public DateTime Request_Hope_End_Date { get; set; }
        public DateTime Process_Start_Date { get; set; }
        public DateTime Process_End_Date { get; set; }

        public int Performer_Emp_No { get; set; }
        public int Requester_Emp_No { get; set; }
        public string Requester_Emp_Name { get; set; }
        public string Performer_Emp_Name { get; set; }

        public string Product { get; set; }
        public string Title { get; set; }
        public string R_Conent { get; set; }
        public string P_Content { get; set; }
    }
}
