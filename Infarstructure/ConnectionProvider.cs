using System.Configuration;
using System.Data.SqlClient;

namespace Infarstructure
{
    public class ConnectionProvider
    {
        public const string ConnectionString = "fssecure";

        public static SqlConnection GetGlobalConnection()
        {
            SqlConnection conn = new SqlConnection();

            conn.ConnectionString = ConfigurationManager.ConnectionStrings[ConnectionString].ConnectionString;

            conn.Open();
            return conn;
        }
    }
}