using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TranferDataToSQLApp
{
    public class Open
    {
        public static bool complete = false;
        public static Boolean OpenApp(string templatePath)
        {
            
            Boolean fileExist = true; 
            try
            {
                
                Process.Start(@templatePath);

            }
            catch (Exception w)
            {
                fileExist = false;
                UserInterface.logger = LogManager.GetCurrentClassLogger();
                UserInterface.logger.Error(w,"File can not be opened");
                MessageBox.Show("File can not be opened", "ERROR");

            }
            return fileExist;
        }

        public static Boolean OpenAppVerify(string templatePath)
        {
            Boolean fileExist = true;
            try
            {
                //Process.Start(string.Format(@"{0}" + templatePath, (char)34));
                var p = Process.Start(@templatePath);
                p.WaitForExit();

            }
            catch (Exception s)
            {
                fileExist = false;
                UserInterface.logger = LogManager.GetCurrentClassLogger();
                UserInterface.logger.Error(s,"File can not be opened");
                MessageBox.Show("File can not be opened", "ERROR");
            }
            complete = true;
            return fileExist;
        }

        public static Boolean OpenJson(string cmdline)
        {
            bool noError = true;
            string sProcess="";
            //string sProcess = @"C:\windows\system32\cmd.exe";
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                 sProcess = appSettings["cmd_string"];
                
                
            }
            catch (Exception e)
            {
                complete = true;
                UserInterface.logger = LogManager.GetCurrentClassLogger();
                UserInterface.logger.Error(e,"Error reading cmd address");
                MessageBox.Show("Error reading cmd address", "ERROR");
                return noError=false;
            }
        
            

            Process p = new Process();
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            p.StartInfo.FileName = sProcess;
            // p.StartInfo.Arguments = cmd;
            p.StartInfo.CreateNoWindow = true;

            try
            {
                p.Start();
            }
            catch (Exception s)
            {
                complete = true;
                UserInterface.logger = LogManager.GetCurrentClassLogger();
                UserInterface.logger.Error(s,"Unable to run cmd");
                MessageBox.Show("Unable to run cmd", "ERROR");
                return noError=false;
            }
            System.IO.StreamReader sOut = p.StandardOutput;
            StreamWriter myStreamWriter = p.StandardInput;

            myStreamWriter.WriteLine(cmdline);  

            p.Close();
            complete = true;
            return noError;
        }
    }
}
