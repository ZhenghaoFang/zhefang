using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using UserVerify;

namespace TranferDataToSQLApp
{
    public class JobList
    {
        public static List<string> retJobList(UserVerify.VerifyUser user)
        {
           
            List<string> jobs = new List<string>();

            string permissionLevel = user.PermissionLevel;
            string userID = user.UserID;

            if (permissionLevel == "1")
            {
                // Populate Job Postings List
                string connectionString = @"Data Source=D2E1CLDB15\SQL16DEVH;" + "Initial Catalog=HCMHRSystems;" + "Integrated Security=True";
                SqlConnection conn = new SqlConnection(connectionString);

                conn.Open();

                SqlCommand command = new SqlCommand("dbo.spTransferDataToSQLApp_ListofStepsbyUser", conn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@userID", "linj3842"); //later on change to userID

                SqlDataReader reader = command.ExecuteReader();


                try
                {
                    while (reader.Read())
                    {
                        jobs.Add(reader["JobDescription"].ToString());

                    }
                }

                catch (Exception e)
                {
                    MessageBox.Show("No available job postings");
                }

                conn.Close();
            }

            else if (permissionLevel == "2")
            {

            }

            return jobs;

        }

      
     
    }
}




























