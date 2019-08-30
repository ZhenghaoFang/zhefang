using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using Newtonsoft.Json.Linq;
using Microsoft.ApplicationBlocks.Data;
using System.Data.SqlClient;
using System.Configuration;
using NLog;
namespace TranferDataToSQLApp
{
    public class Compare
    {
        public static Boolean complete = false;
        //compare the user's file and template
        public static void compare(Excel.Range fileRange, Excel.Range tempRange, string templatePath)
        {
            int fileRow = fileRange.Rows.Count;
            int fileCol = fileRange.Columns.Count;


            int tempCol = tempRange.Columns.Count;
            try { 


            if (fileRow < 2)
            {
                complete = true;
                UserInterface.Error = true;
                UserInterface.logger = LogManager.GetCurrentClassLogger();
                UserInterface.logger.Debug("File has no content error");
                if (MessageBox.Show("The file has no content. \nDo you want to select an application to open the job posting's template?", "Error", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    // user clicked yes

                    Open.OpenApp(templatePath);
                }
                return;
            }
            for (int i = 1; i <= fileRow; i++)
            {
                //make sure each file row has the same num cols as temp row (extra content)
                if (fileCol != tempCol)
                {
                    complete = true;
                    UserInterface.Error = true;
                    UserInterface.logger = LogManager.GetCurrentClassLogger();
                    UserInterface.logger.Debug("File has extra content error");
                    if (MessageBox.Show("The file has extra content. Error found in Row " + i.ToString() + ".\nDo you want to select an application to open the job posting's template?", "Error", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        // user clicked yes
                        Open.OpenApp(templatePath);
                    }
                    return;
                }
                if (i == 1)
                {//header check

                    for (int j = 1; j <= tempCol; j++)
                    {
                        //make sure the file header row doesn't have blank in middle
                        if (fileRange.Cells[1, j].Value2 == null)
                        {
                            complete = true;
                            UserInterface.Error = true;
                            UserInterface.logger = LogManager.GetCurrentClassLogger();
                            UserInterface.logger.Debug("File has header middle missing error");
                            if (MessageBox.Show("The file has header middle missing error. Error found in Row 1 Column " + j.ToString() + ".\nDo you want to select an application to open the job posting's template?", "Error", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                // user clicked yes
                                Open.OpenApp(templatePath);
                            }
                            return;
                        }
                        //make sure the file header matches temp header
                        if (fileRange.Cells[1, j].Value2.ToString() != tempRange.Cells[1, j].Value2.ToString())
                        {
                            UserInterface.Error = true;
                            complete = true;
                            UserInterface.logger = LogManager.GetCurrentClassLogger();
                            UserInterface.logger.Debug("File header error");
                            if (MessageBox.Show("The file's header does not match the template's header. \nDo you want to select an application to open the job posting's template?", "Error", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                // user clicked yes
                                Open.OpenApp(templatePath);
                            }

                            return;
                        }
                    }
                }
                //content check
                for (int j = 1; j <= tempCol; j++)
                {

                    //make sure all contents are filled
                    if (tempRange.Cells[2, j].Value2 == null)// & tempRange.Cells[2, j].Value2.ToString()=="*")
                    { }
                    else if (tempRange.Cells[2, j].Value2.ToString() == "" || tempRange.Cells[2, j].Value2.ToString() == " ") { }
                    else
                    {
                        if (fileRange.Cells[i, j].Value2 == null)
                        {
                            UserInterface.Error = true;
                            complete = true;
                            UserInterface.logger = LogManager.GetCurrentClassLogger();
                            UserInterface.logger.Debug("File has content error");
                            if (MessageBox.Show("The file has content error. Error found in Row " + i.ToString() + " Column " + j.ToString() + ".\nDo you want to select an application to open the job posting's template?", "Error", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                // user clicked yes
                                Open.OpenApp(templatePath);
                            }

                            return;
                        }
                    }
                }

            }
        }
            catch(Exception e)
            {
                UserInterface.logger = LogManager.GetCurrentClassLogger();
                UserInterface.logger.Error(e,"The File casued error during the Validation");
                MessageBox.Show("The file has caused an internal error, please check your file!", "Validation");
            }

            complete = true;
            UserInterface.logger = LogManager.GetCurrentClassLogger();
            UserInterface.logger.Debug("The File Passed Validation");
            MessageBox.Show("The file is in the correct format", "Validation");

            UserInterface.Error = false;

        }


        public static void SQL_SP_ImportData(Excel.Range fileRange, JSON JSONString)
        {
            complete = false;
            CallSQLSP callsql = new CallSQLSP();
            string connectionstring = JSONString.ConnectionString; 
            int rows = fileRange.Rows.Count;
            int cols = fileRange.Columns.Count;
            string msg = callsql.msg;
            try
            {
                for (int i = 2; i <= rows; i++)
                {

                    var val = "";
                    List<SqlParameter> parameters = new List<SqlParameter>();
                    for (int j = 1; j <= cols; j++)
                    {
                        if (fileRange.Cells[i, j].Value2 == null)
                        {
                            val = null;
                        }
                        else
                        {
                            val = fileRange.Cells[i, j].Value2.ToString();


                            //Replace the "?" in the JSON string with the corresponding value in user's excel file
                            if (JSONString.SP_Parameters.ElementAt(j - 1).GetValue("value").ToString().Equals("?"))
                            {
                                if (JSONString.SP_Parameters.ElementAt(j - 1).GetValue("type").ToString().Equals("date"))
                                {
                                    double d = double.Parse(val);
                                    DateTime conv = DateTime.FromOADate(d);
                                    val = conv.ToShortDateString();
                                }
                            }
                        }
                        parameters.Add(new SqlParameter("@" + JSONString.SP_Parameters.ElementAt(j - 1).GetValue("innername").ToString(), val));
                    }


                    // Stored Procedure 
                    callsql.connectionstring = connectionstring;
                    callsql.spname = JSONString.SP_Name;
                    callsql.parameters = parameters;
                    callsql.ExecSPNoReturn();
                    msg = callsql.msg;

                }
            }
            catch(Exception e)
            {
                complete = true;
                UserInterface.Error = true;
                UserInterface.logger = LogManager.GetCurrentClassLogger();
                UserInterface.logger.Error(e,"SQL_SP_ImportData Error");
                MessageBox.Show("Fail to import data to database", "ERROR");
                return;
            }
            complete = true;
            UserInterface.logger = LogManager.GetCurrentClassLogger();
            UserInterface.logger.Debug("SQL_SP_ImportData" + msg);
            MessageBox.Show("SQL_SP_ImportData " + msg, "SQL_SP_ImportData Message");           
        }

        public static void SQL_SP(JSON JSONString)
        {
            complete = false;
            CallSQLSP callsql = new CallSQLSP();

            List<SqlParameter> parameters = new List<SqlParameter>();
            try
            {
                for (int x = 0; x < JSONString.SP_Parameters.Count; x++)
                {
                    int val = Convert.ToInt32(JSONString.SP_Parameters.ElementAt(x).GetValue("value"));
                    parameters.Add(new SqlParameter(JSONString.SP_Parameters.ElementAt(x).GetValue("innername").ToString(), val));
                }
          
            callsql.connectionstring = JSONString.ConnectionString;
            callsql.spname = JSONString.SP_Name;
            callsql.parameters = parameters;
            callsql.ExecSPNoReturn();
            }
            catch (Exception e)
            {
                complete = true;
                UserInterface.Error = true;
                UserInterface.logger = LogManager.GetCurrentClassLogger();
                UserInterface.logger.Error(e,"SQL_SP failed" );
                MessageBox.Show("SQL_SP failed", "ERROR");
                return;
            }
            string msg = callsql.msg;
            complete = true;
            UserInterface.logger = LogManager.GetCurrentClassLogger();
            UserInterface.logger.Debug("SQL_SP" + msg);
            MessageBox.Show("SQL_SP " + msg, "SQL_SP Message");

        }


    }

}
