using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;

namespace TranferDataToSQLApp
{
    class CallSQLSP
    {
        public string connectionstring;
        public DataSet ds;
        public List<SqlParameter> parameters;
        public string spname;
        //public string parameter;
        //public string returnvalue;
        public string msg;


        public string ExecSPNoReturn() 
        {
            try
            {
                SqlHelper.ExecuteNonQuery(connectionstring, CommandType.StoredProcedure, spname,parameters.ToArray());
                return msg = "Succeed";
            }
            catch (Exception e)
            {
                return msg = e.Message;
            }
        }
        public string ExecSPDataSetReturn()
        {
            try
            {
                ds = SqlHelper.ExecuteDataset(connectionstring, CommandType.StoredProcedure,spname,parameters.ToArray());
                return msg = "Succeed";
            }
            catch (Exception e)
            {
                return msg = e.Message;
            }
        }
    }
}
