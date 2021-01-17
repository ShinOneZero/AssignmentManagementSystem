using AMS.Database;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace AMS
{
    /// <summary>
    /// App.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class App : Application
    {
        private void Application_Exit(object sender, ExitEventArgs e)
        {
            Tools.CheckFileAndRemove(System.Environment.CurrentDirectory + @"\QMS_DB.zip");
            Tools.CreateZIPFile(System.Environment.CurrentDirectory, System.Environment.CurrentDirectory + @"\QMS_DB.mdb");
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Tools.CheckFileAndRemove(System.Environment.CurrentDirectory + @"\QMS_DB.mdb");
            Tools.ExtractZIPFile(System.Environment.CurrentDirectory + @"\QMS_DB.zip", System.Environment.CurrentDirectory);
        }
    }
}
