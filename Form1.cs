using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.ApplicationBlocks.Data;
using System.IO;
using openApp = System.Diagnostics;
using System.Diagnostics;
using UserVerify;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using NLog;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;

namespace TranferDataToSQLApp
{
    public partial class UserInterface : Form
    {
        public static bool Error = true; //true means there is error, false means no error
       
        GetTaskTable userTables = new GetTaskTable();
        DataTable TheJobTaskTable = new DataTable();
        public static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        Hashtable myHashtable;

        // Declear a delegate variable
        //public delegate void UpdateProgressBarDelegate(int progress);


        public UserInterface()
        {

            UserVerify.VerifyUser verifyUser = new UserVerify.VerifyUser();
            verifyUser.UserID = Environment.UserName;
            bool userVerify=false;
            try
            {
                 userVerify = verifyUser.getUserVerify("dbo.TransferDataToSQLApp_User");
            }
            catch(Exception e)
            {
                logger = LogManager.GetCurrentClassLogger();
                logger.Error(e,"UserVerify String Error");
                MessageBox.Show("Error loading UserVerify String", "ERROR");
            }
            string permissionLevel = verifyUser.PermissionLevel;

            try
            {
                string success = userTables.GetJobTasksTables(verifyUser.UserID);
            }
            catch(Exception se)
            {
                logger = LogManager.GetCurrentClassLogger();
                logger.Error(se,"Error loading database");
                MessageBox.Show("Error loading database", "ERROR");

            }
            DataTable JobsList = userTables.JobsList;
            DataTable JobTasksList = userTables.JobTasksList;

            
            if (userVerify)
            {
               
                InitializeComponent();
                //InitializeBackgroundWorker();

                //populate job postings drop-down menu
                for (int r = 0; r < JobsList.Rows.Count; r++)
                {
                    this.JobPos.Items.Add(JobsList.Rows[r][1].ToString());
                }
                if (JobPos.Items.Count == 1)
                {
                    JobPos.Text=JobsList.Rows[0][1].ToString();
                }
            }

            else
            {
                MessageBox.Show("No Permission", "WARNING");
                Load += (s, e) => Close();
                return;
            }

        }
       

        private void Browse_Click(object sender, EventArgs e)
        {
            logger = LogManager.GetCurrentClassLogger();
            logger.Debug("Clicked browse");
            this.Submit.Enabled = false;
            OpenFileDialog b1 = new OpenFileDialog
            {
                InitialDirectory = @"C:\",
                Title = "Browse Files",
                CheckFileExists = true,
                CheckPathExists = true,
                DefaultExt = "xls|xlsx|csv",
                Filter = "xls, xlsx, csv files (*.xls;*.xlsx;*.csv)|*.xls;*.xlsx;*.csv",
                FilterIndex = 2,
                RestoreDirectory = true,
                ReadOnlyChecked = true,
                ShowReadOnly = true
            };


            if (b1.ShowDialog() == DialogResult.OK)
            {
                FilePath.Text = b1.FileName;
                this.Submit.Enabled = true;
            }
            if(FilePath.Text != "")
            {
                this.Submit.Enabled = true;
            }
        }

        private void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Label1_Click(object sender, EventArgs e)
        {

        }

        private void Label2_Click(object sender, EventArgs e)
        {

        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            
            if (FilePath.Text != "")
            {
                this.Submit.Enabled = true;
            }

            if (FilePath.Text == "")
            {
                this.Submit.Enabled = false;
            }
        }

        private void JobPos_SelectedIndexChanged(object sender, EventArgs e)
        {

            logger = LogManager.GetCurrentClassLogger();
            logger.Debug("Text Changed");

            if (JobPos.Text != "")
            {
                Browse.Enabled = true; //job pos is accepting applications
                FilePath.Enabled = true;
            }

            TheJobTaskTable = GetTheJobTaskTable(userTables.JobTasksList, getJobID());

            //populate task list

            this.listBox1.Items.Clear();
            for (int r = 0; r < TheJobTaskTable.Rows.Count; r++)
            {
                this.listBox1.Items.Add(TheJobTaskTable.Rows[r][1].ToString() + ".   " + TheJobTaskTable.Rows[r][5].ToString());
            }

            this.listBox1.Show();
            this.TaskListLabel.Show();

        }        

        private void Submit_ClickAsync(object sender, EventArgs e)
        {
            logger = LogManager.GetCurrentClassLogger();
            logger.Debug("Click Submit");
          

            this.CurrentTask.Show();
            this.TaskStep.Show();
            this.progressBar1.Show();
          
            for (int i = 0; i < TheJobTaskTable.Rows.Count; i++)
            {

                TaskStep.Text = TheJobTaskTable.Rows[i][1].ToString() + ".   " + TheJobTaskTable.Rows[i][5].ToString();


                //---------------------------------- External Task ------------------------------------------                
                if (TheJobTaskTable.Rows[i][3].ToString().Equals("False"))
                {
                    if (TheJobTaskTable.Rows[i][2].Equals("External_Command"))
                    {
                        String fileString = TheJobTaskTable.Rows[i][4].ToString();
                        JSON fileJSON = JsonConvert.DeserializeObject<JSON>(fileString);
                       
                        //----------------------Default = YES-------------------


                        if (fileJSON.Default.Equals("Yes"))
                        {
                            logger = LogManager.GetCurrentClassLogger();
                            logger.Debug("User Verify File");
                            if (MessageBox.Show("Do you want to verify if your file has the correct data?", "Verify Data", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                TaskStep.Text = "Waiting for Resume...";
                                logger = LogManager.GetCurrentClassLogger();
                                logger.Debug("User Chose Verify File");
                                // user clicked yes
                                if (!Open.OpenAppVerify(FilePath.Text))
                                {
                                    
                                    MessageBox.Show("Please reselect your file", "MESSAGE");
                                    return;
                                }


                            }
                            else
                            {
                                logger = LogManager.GetCurrentClassLogger();
                                logger.Debug("User Chose Not To Verify");
                            }
                        }

                        //----------------------Default = YES END-------------------

                        //----------------------Default = NO--------------------
                        else
                        {

                          
                                if (fileJSON.Default.Equals("No"))
                                {
                                    logger = LogManager.GetCurrentClassLogger();
                                    logger.Debug("Oppening External App");
                                    if (!Open.OpenJson(fileJSON.App))
                                    {
                                        MessageBox.Show("unable to call external command", "ERROR");
                                        return;
                                    }
                                    Task.Delay(10000).Wait();
                                }
                          
                            ThreadStart progressBarThreadStart = () =>
                            {
                                progressBar1.Invoke(new Action(() =>
                                {
                                    progressBar1.Value = 0;
                                }));
                                while (progressBar1.Value < 100)
                                {

                                    progressBar1.Invoke((MethodInvoker)delegate ()
                                    {

                                        if (!Open.complete)
                                        {
                                            progressBar1.Value += 1;

                                        }
                                        else
                                        {
                                            progressBar1.Value = progressBar1.Maximum;
                                        }
                                    });

                                    Thread.Sleep(80);

                                }

                                Thread.Sleep(1000);
                                progressBar1.Invoke(new Action(() =>
                                {
                                    progressBar1.Value = 0;
                                }));
                            };

                            Thread progressBarThread = new Thread(progressBarThreadStart);
                            progressBarThread.Start();
                            //await Task.Run(() =>
                            //{
                            //    progressBar1.Invoke(new Action(() =>
                            //    {
                            //        progressBar1.Value = 0;
                            //    }));
                            //    while (progressBar1.Value < 100)
                            //    {

                            //        progressBar1.Invoke((MethodInvoker)delegate ()
                            //        {

                            //            if (!Open.complete)
                            //            {
                            //                progressBar1.Value += 1;

                            //            }
                            //            else
                            //            {
                            //                progressBar1.Value = progressBar1.Maximum;
                            //            }
                            //        });

                            //        Thread.Sleep(80);

                            //    }

                            //    Thread.Sleep(1000);
                            //    progressBar1.Invoke(new Action(() =>
                            //    {
                            //        progressBar1.Value = 0;
                            //    }));


                            //});
                        }

                        //----------------------Default = NO END--------------------

                        //-----------------30 SEC AUTOMATED MESSAGE--------------------

                                 var result = AutoClosingMessageBox.Show(
                                   text: "Do you want to continue?",
                                   caption: "Continue",
                                   timeout: 30000,
                                   buttons: MessageBoxButtons.YesNo,
                                   defaultResult: DialogResult.Yes);
                                if (result == DialogResult.Yes)
                                {
                                }
                                else
                                {
                                    return;
                                }                       

                        //-----------------30 SEC AUTOMATED MESSAGE END--------------------

                    }
                //----------------EXTRERNAL COMMAND END----------------------------- 
                }



                //----------------Internal Task---------------------------
                else if (TheJobTaskTable.Rows[i][3].ToString().Equals("True"))
                { 
                //---------------- Excel Format Validation --------------------                

                    if (TheJobTaskTable.Rows[i][2].Equals("Excel_Validation"))
                    {
                        string filePath = FilePath.Text;
                        string templatePath = TheJobTaskTable.Rows[i][4].ToString();

                      
                        Boolean ErrorFound = false; //true when found an error

                        ThreadStart progressBarThreadStart = () =>
                        {
                            progressBar1.Invoke(new Action(() =>
                            {
                                progressBar1.Value = 0;
                            }));
                            while (progressBar1.Value < 100)
                            {
                               

                                progressBar1.Invoke((MethodInvoker)delegate ()
                                {

                                    if (!Compare.complete & ErrorFound == false)
                                    {
                                        progressBar1.Value += 1;

                                    }
                                    else
                                    {
                                        progressBar1.Value = progressBar1.Maximum;
                                    }
                                });

                                Thread.Sleep(80);

                            }
                            
                            Thread.Sleep(1500);

                            progressBar1.Invoke(new Action(() =>
                            {
                                progressBar1.Value = 0;
                            }));
                        };


                        Thread progressBarThread = new Thread(progressBarThreadStart);
                        progressBarThread.Start();

                       
                        try
                        {
                            logger = LogManager.GetCurrentClassLogger();
                            logger.Debug("Validating user's file with template");
                           
                            CheckExcellProcesses();
                            Compare.compare(GetExcelRange(filePath), GetExcelRange(templatePath), templatePath);
                            
                            KillExcel();

                        }

                        catch (Exception ce)
                        {   KillExcel();
                            ErrorFound = true;
                            MessageBox.Show("File can not be found", "ERROR");
                            logger = LogManager.GetCurrentClassLogger();
                            logger.Error(ce,"File can not be found");
                            
                        }

                       

 
                        if (Error == true)
                        {
                            logger = LogManager.GetCurrentClassLogger();
                            logger.Debug("program exit with a validation error.");
                            return;
                        }

                    }

                    //-----------------EXCEL VALIDATION END--------------------------------



                    //---------------- SQL_SP_ImportData ------------------------------    

                    if (TheJobTaskTable.Rows[i][2].Equals("SQL_SP_ImportData") & Error == false)
                    {
                        logger = LogManager.GetCurrentClassLogger();
                        logger.Debug("Calling SQL_SP_ImportData");
                        ThreadStart progressBarThreadStart = () =>
                        {
                            progressBar1.Invoke(new Action(() =>
                            {
                                progressBar1.Value = 0;
                            }));
                            while (progressBar1.Value < 100)
                            {
                                                                
                                progressBar1.Invoke((MethodInvoker)delegate ()
                                {

                                    if (!Compare.complete)
                                    {
                                        progressBar1.Value += 1;

                                    }
                                    else
                                    {
                                        progressBar1.Value = progressBar1.Maximum;
                                    }
                                });

                                Thread.Sleep(80);
                                Application.DoEvents();
                            }
                        
                            Thread.Sleep(1500);
                            progressBar1.Invoke(new Action(() =>
                            {
                                progressBar1.Value = 0;
                            }));
                        };

                        Thread progressBarThread = new Thread(progressBarThreadStart);
                        progressBarThread.Start();

                        String SQL_SP_ImportDataString = TheJobTaskTable.Rows[i][4].ToString();
                        JSON SQL_SP_ImportDataJSON = JsonConvert.DeserializeObject<JSON>(SQL_SP_ImportDataString);
                        try {
                            CheckExcellProcesses();
                            Compare.SQL_SP_ImportData(GetExcelRange(FilePath.Text), SQL_SP_ImportDataJSON);
                            KillExcel();
                            if (Error == true)
                            {                              
                                KillExcel();
                                return;
                            }
                        }
                        catch (Exception ce)
                        {
                            KillExcel();
                            logger = LogManager.GetCurrentClassLogger();
                            logger.Error(ce,"File Not Found in SQL_SP_ImportData");
                            MessageBox.Show("File cannot be found", "ERROR");
                            return;
                        }
                    }

                    //------------------SQL_SP_ImportData END------------------------------


                    //-------------------- SQL_SP ------------------------------------

                    if (TheJobTaskTable.Rows[i][2].Equals("SQL_SP") & Error == false)
                    {
                        logger = LogManager.GetCurrentClassLogger();
                        logger.Debug("Calling SQL_SP");
                        ThreadStart progressBarThreadStart = () =>
                        {
                            progressBar1.Invoke(new Action(() =>
                            {
                                progressBar1.Value = 0;
                            }));
                            while (progressBar1.Value < 100)
                            {
                                                                 
                                progressBar1.Invoke((MethodInvoker)delegate ()
                                {

                                    if (!Compare.complete)
                                    {
                                        progressBar1.Value += 1;

                                    }
                                    else
                                    {
                                        progressBar1.Value = progressBar1.Maximum;
                                    }
                                });

                                Thread.Sleep(80);

                            }
                           
                            Thread.Sleep(1000);
                            progressBar1.Invoke(new Action(() =>
                            {
                                progressBar1.Value = 0;
                            }));
                        };

                        Thread progressBarThread = new Thread(progressBarThreadStart);
                        progressBarThread.Start();

                        String SQL_SPString = TheJobTaskTable.Rows[i][4].ToString();
                        JSON SQL_SPJSON = JsonConvert.DeserializeObject<JSON>(SQL_SPString);

                        try
                        {
                            Compare.SQL_SP(SQL_SPJSON);
                            if (Error == true)
                            {
                                return;
                            }
                        }
                        catch(Exception se)
                        {
                            logger = LogManager.GetCurrentClassLogger();
                            logger.Error(se,"Calling SQL_SP failed");
                            MessageBox.Show("Failed to call SQL_SP.", "ERROR");
                            return;
                        }
                    //-------------------- SQL_SP END ------------------------------------
                    }

                //-------------------------INTERNAL TASK END----------------------------

                }
              //--------------------------END-----------------------------------------      
            }
            Thread.Sleep(1500);

            MessageBox.Show("Your file has been successfully submitted!", "Success");
            TaskStep.Text = "All Tasks Finished";
            //this.Close();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            logger = LogManager.GetCurrentClassLogger();
            logger.Debug("User Clicked Cancel Button");
            this.Close();
        }




        // -------------------------------- HELPER METHODS ------------------------------------------------------


        public Excel.Range GetExcelRange(string excelFileName)
        {
            Excel.Application excelFileObject = new Excel.Application();
            Excel.Workbook workBookObject = excelFileObject.Workbooks.Open(excelFileName/*, 0, true, 5, "", "", false, Excel.XlPlatform.xlWindows,"",true,false,0,true,false,false*/) ;
            Excel.Sheets sheets = workBookObject.Worksheets;

            // get the first and only worksheet from the collection of worksheets
            Excel.Worksheet worksheet = (Excel.Worksheet)sheets.get_Item(1);

            Excel.Range xlRange = worksheet.UsedRange;

            return xlRange;

        }

        public int getJobID()
        {
            int JobID = 0;

            string connectionString = @"Data Source=D2E1CLDB15\SQL16DEVH;" + "Initial Catalog=HCMHRSystems;" + "Integrated Security=True";
            SqlConnection conn = new SqlConnection(connectionString);

            // Get JobID

            conn.Open();

            SqlCommand getJobID = conn.CreateCommand();
            getJobID.CommandText = String.Format(@"select {0} from dbo.TransferDataToSQLApp_JobList where JobDescription = @prmJobDescription", "JobID");
            getJobID.Parameters.AddWithValue("@prmJobDescription", JobPos.Text);
            SqlDataReader jobIDReader = getJobID.ExecuteReader();


            while (jobIDReader.Read())
            {
                JobID = Convert.ToInt32(jobIDReader[0]);
            }

            conn.Close();

            return JobID;
        }

        public DataTable GetTheJobTaskTable(DataTable table, int JobID)
        {
            DataTable retTable = new DataTable();
            retTable = table.Select("JobID = " + JobID.ToString()).CopyToDataTable();
            return retTable;
        }

     


        private void CheckExcellProcesses()
        {
            Process[] AllProcesses = Process.GetProcessesByName("excel");
            myHashtable = new Hashtable();
            int iCount = 0;

            foreach (Process ExcelProcess in AllProcesses)
            {
                myHashtable.Add(ExcelProcess.Id, iCount);
                iCount = iCount + 1;
            }
        }

        private void KillExcel()
        {
            Process[] AllProcesses = Process.GetProcessesByName("excel");

            // check to kill the right process
            foreach (Process ExcelProcess in AllProcesses)
            {
                if (myHashtable.ContainsKey(ExcelProcess.Id) == false)
                    ExcelProcess.Kill();
            }

            AllProcesses = null;
        }

        private void UserInterface_Activated(object sender, EventArgs e)
        {
            TaskStep.Text = "No Current Task";
        }

   
    }
}


