using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccess;
using System.Data.SqlClient;

namespace FS.Common
{
     [StoredProcedure(ReadProcedure = "GetValueDataById")]
    public class ValuseData
    {
        [DataColumn("ValuseDataId")]
        [PrimaryKey]
        private int _ValuseDataId;
        public int ValuseDataId
        {
            get { return _ValuseDataId; }
            set { _ValuseDataId = value; }
        }
        [DataColumn("DataKey")]
        private string _DataKey;
        public string DataKey
        {
            get { return _DataKey; }
            set { _DataKey = value; }
        }
        [DataColumn("DataValue")]
        private string _DataValue;
        public string DataValue
        {
            get { return _DataValue; }
            set { _DataValue = value; }
        }
        [DataColumn("DataText")]
        private string _DataText;
        public string DataText
        {
            get { return _DataText; }
            set { _DataText = value; }
        }

        public static List<ValuseData> GetValueData(string key)
        {
            return Persistance.ReadList<ValuseData>("GetValueData", new SqlParameter("@DataKey", key));
        }
    }
}
