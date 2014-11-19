using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess
{
    [AttributeUsage(AttributeTargets.Class)]
    public class StoredProcedure:Attribute
    {
        public string CreateProcedure;
        public string ReadProcedure;
        public string UpdateProcedure;
        public string DeleteProcedure;
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class PrimaryKey : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class DataColumn : Attribute
    {
        public DataColumn(string columnName)
        {
            ColumnName = columnName;
        }
        public string ColumnName;
    }

  
}
