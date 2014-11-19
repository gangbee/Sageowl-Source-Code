using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Data.SqlTypes;

namespace DataAccess
{
    public class Persistance
    {
        public delegate SqlConnection ConnectionProviderDelegate();

        [ThreadStatic]
        public static ConnectionProviderDelegate ConnectionProvider;

        /// <summary>
        /// Gets the provided connection.
        /// </summary>
        /// <returns></returns>
        public static SqlConnection GetProvidedConnection()
        {
            if (ConnectionProvider == null) throw new Exception("Cannot provide connection.");

            return ConnectionProvider();
        }

        public static SqlParameter BuildParameter(string name, object value, Type t)
        {
            // Some may ask why this crap has to be done. The answer is that theres
            // several parts of the framework where Nullable<T> support is not fully
            // integrated. Hopefully this will be fixed before this code is in
            // production, instead of it being looked at a while down the track
            // whereupon I will be looked at as an idiot for writing something like this.
            // * Conversions are straight out the mssql/clr data type equivalency table.
            // * Nulls will not have their correct type going into the sqlclient without this mess.
            // * Dont bother with something like "is value type", nullable's being a ref type is hidden by the clr.
            //		you could try for the type name starting with "Nullable`1" but thats a touch dodgier
            SqlParameter p = new SqlParameter(name, value ?? DBNull.Value);

            p.IsNullable = (t.Equals(typeof(int?)) || t.Equals(typeof(decimal?))
                || t.Equals(typeof(double?)) || t.Equals(typeof(float?))
                || t.Equals(typeof(DateTime?)) || t.Equals(typeof(Guid?))
                || t.Equals(typeof(bool?)));

            if (t.Equals(typeof(byte[])))
                p.SqlDbType = System.Data.SqlDbType.Image;

            else if (t.Equals(typeof(DateTime)) || t.Equals(typeof(DateTime?)))
                p.SqlDbType = System.Data.SqlDbType.DateTime;

            else if (t.Equals(typeof(SqlDateTime)) || t.Equals(typeof(SqlDateTime?)))
                p.SqlDbType = System.Data.SqlDbType.DateTime;

            else if (t.Equals(typeof(int)) || t.Equals(typeof(int?)))
                p.SqlDbType = System.Data.SqlDbType.Int;

            else if (t.Equals(typeof(decimal)) || t.Equals(typeof(decimal?)))
                p.SqlDbType = System.Data.SqlDbType.Decimal;

            else if (t.Equals(typeof(byte)) || t.Equals(typeof(byte?)))
                p.SqlDbType = System.Data.SqlDbType.TinyInt;

            else if (t.Equals(typeof(double))
                || t.Equals(typeof(float))
                || t.Equals(typeof(double?))
                || t.Equals(typeof(float?)))
                p.SqlDbType = System.Data.SqlDbType.Float;

            else if (t.Equals(typeof(string)))
                p.SqlDbType = System.Data.SqlDbType.VarChar;

            else if (t.Equals(typeof(Guid)) || t.Equals(typeof(Guid?)))
                p.SqlDbType = System.Data.SqlDbType.UniqueIdentifier;

            else if (t.Equals(typeof(bool)) || t.Equals(typeof(bool?)))
                p.SqlDbType = System.Data.SqlDbType.Bit;

            else
                System.Diagnostics.Debug.WriteLine(string.Format("WARNING: Did not set parameter '{0}' value '{1}' type '{2}'", name, value, t.FullName));

            return p;
        }

        /// <summary>
        /// Adds the params.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <param name="cmd">The CMD.</param>
        private static void AddParams(SqlParameter[] parameters, SqlCommand cmd)
        {
            foreach (SqlParameter par in parameters)
            {
                cmd.Parameters.Add(
                    par
                );
            }
        }

        //private static string ConnString = ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString;

        private static t PopulateObject<t>(t obj, SqlDataReader dataRow)
        {

            foreach (FieldInfo item in typeof(t).GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly))
            {
                DataColumn dataColumn = item.GetCustomAttributes(typeof(DataColumn), false).FirstOrDefault() as DataColumn;

                if (dataColumn != null)
                {
                    if (dataRow[dataColumn.ColumnName] == DBNull.Value)
                    {
                        item.SetValue(obj, null);
                    }else
                        item.SetValue(obj, dataRow[dataColumn.ColumnName]);
                }

                //item.SetValue(obj, dataRow[item.Name], null);
            }

            return obj;
        }

        private static SqlParameterCollection GenerateParameters<t>(t obj, SqlParameterCollection parameterCollection, out SqlParameter primaryParameter)
        {
            primaryParameter=null;

            foreach (FieldInfo item in typeof(t).GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly))
            {
                DataColumn dataColumn = item.GetCustomAttributes(typeof(DataColumn), false).FirstOrDefault() as DataColumn;
                if (dataColumn != null)
                {
                    if (item.GetCustomAttributes(typeof(PrimaryKey), false).FirstOrDefault() == null)
                    {
                        parameterCollection.Add(
                            new SqlParameter()
                            {
                                ParameterName = dataColumn.ColumnName,
                                Value = item.GetValue(obj)??DBNull.Value
                            }
                            );
                    }
                    else
                    {
                        primaryParameter = new SqlParameter()
                        {
                            ParameterName = dataColumn.ColumnName,
                            Value = item.GetValue(obj),
                            Direction = ParameterDirection.InputOutput
                        };
                        parameterCollection.Add(primaryParameter);
                    }
                }

            }

            return parameterCollection;
        }

        private static SqlParameter GetPrimaryKey<t>(t obj = null) where t : class
        {
            FieldInfo[] propertyInfo = typeof(t).GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);

            foreach (FieldInfo item in propertyInfo)
            {
                if (item.GetCustomAttributes(typeof(PrimaryKey), false).FirstOrDefault() != null)
                {
                    DataColumn dataColumn = item.GetCustomAttributes(typeof(DataColumn), false).FirstOrDefault() as DataColumn;
                    if (dataColumn != null)
                        return new SqlParameter(dataColumn.ColumnName, obj == null ? null : item.GetValue(obj));
                }

            }

            return null;
        }


        private static string GetCreateProcedure<t>()
        {
            StoredProcedure storedProcedure = typeof(t).GetCustomAttributes(typeof(StoredProcedure), false).FirstOrDefault() as StoredProcedure;

            if (storedProcedure != null && storedProcedure.CreateProcedure != null)
                return storedProcedure.CreateProcedure;
            else
                throw new Exception(" Insert operation not implemented");
        }

        private static string GetUpdateProcedure<t>()
        {
            StoredProcedure storedProcedure = typeof(t).GetCustomAttributes(typeof(StoredProcedure), false).FirstOrDefault() as StoredProcedure;

            if (storedProcedure != null && storedProcedure.UpdateProcedure != null)
                return storedProcedure.UpdateProcedure;
            else
                throw new Exception("Update operation not implemented");
        }

        private static string GetReadProcedure<t>()
        {
            StoredProcedure storedProcedure = typeof(t).GetCustomAttributes(typeof(StoredProcedure), false).FirstOrDefault() as StoredProcedure;

            if (storedProcedure != null && storedProcedure.ReadProcedure != null)
                return storedProcedure.ReadProcedure;
            else
                throw new Exception("Read operation not implemented");
        }

        private static string GetDeleteProcedure<t>()
        {
            StoredProcedure storedProcedure = typeof(t).GetCustomAttributes(typeof(StoredProcedure), false).FirstOrDefault() as StoredProcedure;

            if (storedProcedure != null && storedProcedure.DeleteProcedure != null)
                return storedProcedure.DeleteProcedure;
            else
                throw new Exception("Delete operation not implemented");
        }

        public static t Create<t>(t target) where t : class
        {
            SqlConnection conn = GetProvidedConnection();
            SqlCommand sqlCommand = conn.CreateCommand();
            SqlParameter primaryParameter;
            sqlCommand.CommandText = GetCreateProcedure<t>();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            GenerateParameters<t>(target, sqlCommand.Parameters, out primaryParameter);

            sqlCommand.ExecuteNonQuery();
            //Read the object back
            if (primaryParameter != null)
                target = Persistance.ResolveKey<t>(primaryParameter.Value);

            return target;
        }

        public static t Update<t>(t target) where t : class
        {
            SqlConnection conn = GetProvidedConnection();
            SqlCommand sqlCommand = conn.CreateCommand();
            sqlCommand.CommandText = GetUpdateProcedure<t>();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            SqlParameter primaryParameter;
            GenerateParameters<t>(target, sqlCommand.Parameters, out primaryParameter);
            sqlCommand.ExecuteNonQuery();

            //Read the object back
            if (primaryParameter != null)
                target = Persistance.ResolveKey<t>(primaryParameter.Value);
            return target;
        }

        public static void ExecuteNonQuery(string StoredProcedure, params SqlParameter[] parameter) 
        {
            SqlConnection conn = GetProvidedConnection();
            SqlCommand sqlCommand = conn.CreateCommand();
            sqlCommand.CommandText = StoredProcedure;
            sqlCommand.Parameters.AddRange(parameter);
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;           
            sqlCommand.ExecuteNonQuery();
            return;
        }

        public static void Delete<t>(t target) where t : class
        {
            SqlConnection conn = GetProvidedConnection();
            SqlCommand sqlCommand = conn.CreateCommand();
            sqlCommand.CommandText = GetDeleteProcedure<t>();
            sqlCommand.Parameters.Add(GetPrimaryKey<t>(target));
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCommand.Parameters[0].Value = GetPrimaryKey<t>(target).Value;
            sqlCommand.ExecuteNonQuery();
            return;
        }


        public static t ResolveKey<t>(object value) where t : class
        {
            SqlCommand cmd = GetProvidedConnection().CreateCommand();
            cmd.Parameters.Add(GetPrimaryKey<t>(null));
            cmd.Parameters[0].Value = value;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = GetReadProcedure<t>();

            t target = Activator.CreateInstance<t>();

            SqlDataReader reader = cmd.ExecuteReader();

            try
            {
                if (!reader.Read())
                    return null;

                PopulateObject<t>(target, reader);
            }
            finally
            {
                if (reader != null && !reader.IsClosed) reader.Close();
            }

            return target;
        }

        public static List<t> ReadList<t>(string StoredProcedure, params SqlParameter[] parameter)
        {
            SqlCommand cmd = GetProvidedConnection().CreateCommand();
            AddParams(parameter, cmd);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = StoredProcedure;
            List<t> list = new List<t>();

            //AddParams(parameters, cmd);

            Dictionary<string, object> output = new Dictionary<string, object>();


            SqlDataReader reader = cmd.ExecuteReader();
            try
            {


                while (reader.Read())
                {
                    t target = Activator.CreateInstance<t>();
                    list.Add(PopulateObject<t>(target, reader));
                }
            }
            finally
            {
                if (reader != null && !reader.IsClosed) reader.Close();
            }
            return list;
        }


    }
}
