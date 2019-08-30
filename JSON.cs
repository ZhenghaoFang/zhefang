using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data.SqlClient;

namespace TranferDataToSQLApp
{
    public class JSON
    {
        public string ConnectionString { get; set; }
        public string SP_Name { get; set; }
        public List<JObject> SP_Parameters { get; set; }

        public string App { get; set; }

        public string Default { get; set; }
    }
}
