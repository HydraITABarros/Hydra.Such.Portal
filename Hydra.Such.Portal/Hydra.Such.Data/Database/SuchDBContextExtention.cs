using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Linq;


namespace Hydra.Such.Data.Database
{
    public partial class SuchDBContext
    {
        public static string ConnectionString { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(ConnectionString);
            }
        }
    }

    public class SuchDBContextExtention : SuchDBContext
    {
        public virtual IEnumerable<dynamic> execStoredProcedure(String cmdText, SqlParameter[] parameters)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(cmdText, connection))
                {
                    foreach (SqlParameter item in parameters)
                    {
                        command.Parameters.Add(item.ParameterName, System.Data.SqlDbType.NVarChar);
                        command.Parameters[item.ParameterName].Value = item.Value == null ? "" : item.Value;
                    }

                    using (var dataReader = command.ExecuteReader())
                    {
                        var fields = new List<String>();

                        for (var i = 0; i < dataReader.FieldCount; i++)
                        {
                            fields.Add(dataReader.GetName(i));
                        }

                        while (dataReader.Read())
                        {
                            var item = new ExpandoObject() as IDictionary<String, Object>;

                            for (var i = 0; i < fields.Count; i++)
                            {
                                item.Add(fields[i], dataReader[fields[i]]);
                            }

                            yield return item;
                        }
                    }
                }
            }
        }

        public virtual int? execStoredProcedureNQ(String cmdText, SqlParameter[] parameters)
        {
            int result = 0;
            try
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    using (var command = new SqlCommand(cmdText, connection))
                    {
                        foreach (SqlParameter item in parameters)
                        {
                            command.Parameters.Add(item.ParameterName, System.Data.SqlDbType.NVarChar);
                            command.Parameters[item.ParameterName].Value = item.Value == null ? "" : item.Value;
                        }

                        result = command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return result;
        }

        public virtual int execStoredProcedureFH(String cmdText, SqlParameter[] parameters)
        {
            int result = 0;
            try
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    using (var command = new SqlCommand(cmdText, connection))
                    {
                        foreach (SqlParameter item in parameters)
                        {
                            command.Parameters.Add(item.ParameterName, System.Data.SqlDbType.NVarChar);
                            command.Parameters[item.ParameterName].Value = item.Value == null ? "" : item.Value;
                        }

                        var dataReader = command.ExecuteReader();

                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                result = dataReader.GetInt32(0);
                            }
                        }
                        else
                        {
                            result = 99;
                        }
                        dataReader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                return 99;
            }
            return result;
        }

        public virtual int execStoredProcedurePedidoPagamento(String cmdText, SqlParameter[] parameters)
        {
            int result = 0;
            try
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    using (var command = new SqlCommand(cmdText, connection))
                    {
                        foreach (SqlParameter item in parameters)
                        {
                            command.Parameters.Add(item.ParameterName, System.Data.SqlDbType.NVarChar);
                            command.Parameters[item.ParameterName].Value = item.Value == null ? "" : item.Value;
                        }

                        var dataReader = command.ExecuteReader();

                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                result = dataReader.GetInt32(0);
                            }
                        }
                        else
                        {
                            result = 99;
                        }
                        dataReader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                return 99;
            }
            return result;
        }

        /// <summary>
        /// 
        /// *************************************************************************************************
        /// zpgm.
        /// Downloaded on 05-12-2018 from https://code.msdn.microsoft.com/Stored-Procedure-with-6c194514
        /// Adapted to allow the usage of table-valued parameters as decribed in 
        ///     https://docs.microsoft.com/en-us/sql/relational-databases/tables/use-table-valued-parameters-database-engine?view=sql-server-2017
        ///     
        /// Changed method return type to bool and added the try/catch block, to become possible to catch any error executing the stored procedure
        /// 
        /// *************************************************************************************************
        /// 
        /// Execute stored procedure with single table value parameter.
        /// </summary>
        /// <typeparam name="T">Type of object to store.</typeparam>
        /// <param name="context">DbContext instance.</param>
        /// <param name="data">Data to store</param>
        /// <param name="procedureName">Procedure name</param>
        /// <param name="paramName">Parameter name</param>
        /// <param name="typeName">User table type name</param>
        public virtual bool ExecuteTableValueProcedure<T>(IEnumerable<T> data, string procedureName, string paramName, string typeName)
        {
            SuchDBContext context = new SuchDBContext();

            //// convert source data to DataTable
            DataTable table = data.ToDataTable();

            //// create parameter
            SqlParameter parameter = new SqlParameter(paramName, table)
            {
                SqlDbType = SqlDbType.Structured,
                TypeName = typeName
            };


            //// execute sp sql
            string sql = String.Format("EXEC {0} {1};", procedureName, paramName);


            //// execute sql
            try
            {
                // /!\/!\ zpgm.18072019 ****** SQL injection vulnerability! *******
                context.Database.ExecuteSqlCommand(sql, parameter);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

    }

    #region Extension classes to use in Tabele-valued stored procedure
    [Flags]
    public enum PropertyReflectionOptions : int
    {
        /// <summary>
        /// Take all.
        /// </summary>
        All = 0,

        /// <summary>
        /// Ignores indexer properties.
        /// </summary>
        IgnoreIndexer = 1,

        /// <summary>
        /// Ignores all other IEnumerable properties
        /// except strings.
        /// </summary>
        IgnoreEnumerable = 2
    }
    public static class ReflectionExtensions
    {
        /// <summary>
        /// 
        /// *************************************************************************************************
        /// zpgm.Downloaded on 05-12-2018 from https://code.msdn.microsoft.com/Stored-Procedure-with-6c194514
        /// Allows the usage of table-valued parameters as decribed in 
        ///     https://docs.microsoft.com/en-us/sql/relational-databases/tables/use-table-valued-parameters-database-engine?view=sql-server-2017
        /// *************************************************************************************************
        ///
        /// Gets properties of T
        /// </summary>
        public static IEnumerable<PropertyInfo> GetProperties<T>(BindingFlags binding, PropertyReflectionOptions options = PropertyReflectionOptions.All)
        {
            var properties = typeof(T).GetProperties(binding);

            bool all = (options & PropertyReflectionOptions.All) != 0;
            bool ignoreIndexer = (options & PropertyReflectionOptions.IgnoreIndexer) != 0;
            bool ignoreEnumerable = (options & PropertyReflectionOptions.IgnoreEnumerable) != 0;

            foreach (var property in properties)
            {
                if (!all)
                {
                    if (ignoreIndexer && IsIndexer(property))
                    {
                        continue;
                    }

                    if (ignoreIndexer && !property.PropertyType.Equals(typeof(string)) && IsEnumerable(property))
                    {
                        continue;
                    }
                }

                yield return property;
            }
        }

        /// <summary>
        /// Check if property is indexer
        /// </summary>
        private static bool IsIndexer(PropertyInfo property)
        {
            var parameters = property.GetIndexParameters();

            if (parameters != null && parameters.Length > 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Check if property implements IEnumerable
        /// </summary>
        private static bool IsEnumerable(PropertyInfo property)
        {
            return property.PropertyType.GetInterfaces().Any(x => x.Equals(typeof(System.Collections.IEnumerable)));
        }
    }

    public static class DataTableExtensions
    {
        /// <summary>
        /// 
        /// *************************************************************************************************
        /// zpgm.Downloaded on 05-12-2018 from https://code.msdn.microsoft.com/Stored-Procedure-with-6c194514
        /// Allows the usage of table-valued parameters as decribed in 
        ///     https://docs.microsoft.com/en-us/sql/relational-databases/tables/use-table-valued-parameters-database-engine?view=sql-server-2017
        /// *************************************************************************************************
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(this IEnumerable<T> source)
        {
            DataTable table = new DataTable();

            //// get properties of T
            var binding = BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty;
            var options = PropertyReflectionOptions.IgnoreEnumerable | PropertyReflectionOptions.IgnoreIndexer;

            var properties = ReflectionExtensions.GetProperties<T>(binding, options).ToList();

            //// create table schema based on properties
            foreach (var property in properties)
            {
                table.Columns.Add(property.Name, Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType);
            }

            //// create table data from T instances
            object[] values = new object[properties.Count];

            foreach (T item in source)
            {
                for (int i = 0; i < properties.Count; i++)
                {
                    values[i] = properties[i].GetValue(item, null);
                }

                table.Rows.Add(values);
            }

            return table;
        }
    }
    #endregion
}
