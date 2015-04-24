using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Oracle.DataAccess.Client;
using TED.Dashboard.Repository.Connections;

namespace TED.Dashboard.Repository
{
    /// <summary>
    /// Base repository for selecting items out of the Oracle database leveraging the current
    /// HV5 connection
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class BaseDataStore<TEntity> where TEntity: class
    {
        /// <summary>
        /// Determine connection type in inherited class
        /// </summary>
        private readonly IConnection connection;


        protected BaseDataStore(IConnection connection)
        {
            this.connection = connection;
        } 

        /// <summary>
        /// Get items of type from a select statement
        /// </summary>
        /// <param name="sql">sql to retrieve item</param>
        /// <param name="objectMapping">how to map sql to entity</param>
        /// <param name="args">arguments for sql statement</param>
        /// <returns>collection of entities</returns>
        protected IEnumerable<TEntity> RunSelect(string sql, Func<IDataReader,TEntity> objectMapping, IDictionary<string, string> args = null)
        {
            OracleParameter[] parameters = null;
            IEnumerable<TEntity> items;

            // add arguments
            if (args != null)
            {
                parameters = args.Select(a => new OracleParameter(a.Key, a.Value)).ToArray();
            }

            using (var conn = connection.OpenConnection())
            {
                using (var command = conn.CreateCommand())
                {
                    command.CommandText = sql;
                    items = ExecuteCommand(command, objectMapping, parameters);
                }
            }

            return items;
        }

        /// <summary>
        ///  Get items of type from a stored procedure
        /// </summary>
        /// <param name="sql">stored procedure to execute</param>
        /// <param name="objectMapping">how to map sql to entity</param>
        /// <param name="parameters">arguments for sql stored procedure</param>
        /// <returns>collection of entities</returns>
        protected IEnumerable<TEntity> RunStoredProcedure(string sql, Func<IDataReader, TEntity> objectMapping, IEnumerable<OracleParameter> parameters = null)
        {
            IEnumerable<TEntity> items;

            using (var conn = connection.OpenConnection())
            {
                using (var command = conn.CreateCommand())
                {
                    command.CommandText = sql;
                    command.CommandType = CommandType.StoredProcedure;
                    items = ExecuteCommand(command, objectMapping, parameters);
                }
            }

            return items;
        }

        private static IEnumerable<TEntity> ExecuteCommand(OracleCommand command, Func<IDataReader, TEntity> objectMapping, IEnumerable<OracleParameter> parameters = null)
        {
            List<TEntity> items;
            // assists with retrieving clob messages
            // http://stackoverflow.com/questions/19714909/setting-fetchsize-and-initiallobfetchsize-with-enterprise-library
            command.InitialLOBFetchSize = -1;

            // add arguments
            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    command.Parameters.Add(parameter);
                }
            }

            // create objects
            using (var reader = command.ExecuteReader())
            {
                // columns for the search
                //var columns = Enumerable.Range(0, reader.FieldCount).Select(reader.GetName).ToList();
                items = reader.Select(objectMapping).ToList();
            }

            return items;
        }

        /// <summary>
        /// executes a sql action that is not a query.  The value returned is the number of rows affected
        /// by the sql statement 
        /// </summary>
        /// <param name="sql">sql to run</param>
        /// <returns></returns>
        protected int RunSqlCommand(string sql)
        {
            var results = 0;
            using (var conn = connection.OpenConnection())
            {
                using (var command = conn.CreateCommand())
                {
                    command.CommandText = sql;
                    results = command.ExecuteNonQuery();
                }
            }

            return results;
        }

        /// <summary>
        /// Gets a count of the items associated with this type
        /// </summary>
        /// <param name="sql">sql to get count</param>
        /// <param name="args">arguments for sql statement</param>
        /// <returns>integer count</returns>
        protected int GetItemCount(string sql, IDictionary<string, string> args = null)
        {
            int? count;
            using (var conn = connection.OpenConnection())
            {
                using (var command = conn.CreateCommand())
                {
                    command.CommandText = sql;

                    // add arguments
                    if (args != null)
                    {
                        foreach (var arg in args)
                        {
                            command.Parameters.Add(new OracleParameter(arg.Key, arg.Value));
                        }
                    }

                    count = Convert.ToInt32(command.ExecuteScalar());
                }
            }

            if (!count.HasValue)
            {
                throw new Exception(string.Format("Sql for selecting count was invalid.  sql:{0}", sql));
            }

            return count.Value;
        }
    }
}
