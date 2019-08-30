using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Microsoft.ApplicationBlocks.Data;
using System.Data;
using System.Data.SqlClient;

namespace TranferDataToSQLApp
{
    class GetTaskTable
    {
        //get connection string
        private string connectionstring = ConfigurationManager.ConnectionStrings["TranferDataToSQLApp.Properties.Settings.HCMHRSystemsConnectionString"].ConnectionString;
        public  DataTable JobsList;
        public  DataTable JobTasksList;
        public string msg;


        public string GetJobTasksTables(string userID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@userID", userID));
                //DataSet ds=SqlHelper.ExecuteDataset(connectionstring, "spTransferDataToSQLApp_ListofStepsbyUser", userID);
                CallSQLSP callsql = new CallSQLSP() ;
                callsql.connectionstring = connectionstring;
                callsql.spname = "spTransferDataToSQLApp_ListofStepsbyUser";
                callsql.parameters = parameters;
                callsql.ExecSPDataSetReturn();
                msg = callsql.msg;
                if (msg == "Succeed")
                {
                    DataSet ds = callsql.ds;
                    JobsList = ds.Tables[0];
                    JobTasksList = ds.Tables[1];
                    msg = "Succeed";
                }
                return msg;
            }
            catch (Exception e)
            {
                msg = e.Message;
                return msg;
            }

        }

    }

}
