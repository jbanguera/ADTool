using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ADTool.Data;
using MySql.Data.MySqlClient;

namespace ADTool.Data.DapperClient
{
    /// <summary>
    /// Class <see cref="DapperModel"></see> is used for access to Database Engine and manage it. 
    /// You must use this class with the <seealso cref="DapperManager"></seealso> instance.
    /// <para>This class use the Dapper ORM for access to database provaiders. </para>
    /// </summary>
    public class DapperModel : IDisposable
    {
        #region - P R O P E R T I E S
        private bool disposed = false;
        private IDbConnection connection;
        private string connectionString = String.Empty;
        #endregion

        #region - C O N S T R U C T O R S
        /// <summary>
        /// Constructor <see langword="class " cref="DapperModel"/>
        /// </summary>
        public DapperModel()
        {
        }

        /// <summary>
        /// Constructor <see langword="class " cref="DapperModel"/>
        /// </summary>
        /// <param name="connection"></param>
        public DapperModel(IDbConnection connection)
        {
            this.connection = connection;
            connectionString = connection.ConnectionString;
        }

        /// <summary>
        /// Constructor <see langword="class " cref="DapperModel"/>
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="typeProviderEnum"></param>
        public DapperModel(string connectionString, DataConnectionTypeProviderEnum typeProviderEnum)
        {
            switch (typeProviderEnum)
            {
                case DataConnectionTypeProviderEnum.MSSQL:
                    connection = new SqlConnection {  ConnectionString = this.connectionString = connectionString };
                    break;
                case DataConnectionTypeProviderEnum.MYSQL:
                    connection = new MySqlConnection { ConnectionString = this.connectionString = connectionString };
                    break;
            }
        }

        #endregion

        #region - M E T H O D S

        #region MANAGE OPEN AND CLOSE CONNECTION BD METHODS
        /// <summary>
        /// Is DatabaseModel Open
        /// </summary>
        private bool IsOpen
        {
            get { return (connection.State & ConnectionState.Open) == ConnectionState.Open; }
        }

        /// <summary>
        /// Open the DDBB.
        /// </summary>
        private void Open()
        {
            if (!IsOpen)
            {
                connection.Open();
            }
        }

        /// <summary>
        /// Close the DDBB.
        /// </summary>
        /// <exception cref="Exception"></exception>
        private void Close()
        {
            if (IsOpen)
            {
                connection.Close();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool ThereIsConectivity()
        {
            using (connection)
            {
                if (connection.State == ConnectionState.Closed)
                {
                    CheckAndSetConnectionStringForConnection();
                    Open();
                    if (IsOpen) { connection.Close(); return true; }
                    else throw new Exception("Has been occurred an error opening the connection because.");
                }
            }
            return false;
        }
        #endregion

        #region STATEMENT SELECT DB METHODS
        /// <summary>
        /// Execute query using a new connection with param
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connectionString"></param>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        internal static IEnumerable<T> Query<T>(string connectionString, string sql, object param=null, bool buffered=true)
        {
            IEnumerable<T> result = null;
            using (IDbConnection Connection = new SqlConnection(connectionString))
            {
                if (Connection.State == ConnectionState.Closed)
                {
                    try
                    {
                        Connection.Open();
                        result = Connection.Query<T>(sql, param, buffered: buffered);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Has been occurred an error check the internal exception for more details.", ex);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Execute query using connection passed by parameter with parameters
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Connection"></param>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        internal static IEnumerable<T> Query<T>(SqlConnection Connection, string sql, object param=null, bool buffered = true)
        {
            IEnumerable<T> result = null;
            using (Connection)
            {
                if (Connection.State == ConnectionState.Closed)
                {
                    try
                    {
                        Connection.Open();
                        result = Connection.Query<T>(sql, param, buffered: buffered);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Has been occurred an error check the internal exception for more details.", ex);
                    }
                }
            }
            return result;
        }
        /// <summary>
        /// Check the property this.connectionString with property this.connection.ConnectionString are equal, if they are different then 
        ///  this.connection.ConnectionString = this.connectionString
        /// </summary>
        public void CheckAndSetConnectionStringForConnection()
        {
            if (connection.ConnectionString != connectionString || String.IsNullOrEmpty(connection.ConnectionString))
                connection.ConnectionString = connectionString;
        }

        /// <summary>
        /// Execute query with parameters
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        internal IEnumerable<T> Query<T>(string sql, object param=null, bool buffered = true)
        {
            IEnumerable<T> result = null;
            using (connection)
            {
                if (connection.State == ConnectionState.Closed)
                {
                    CheckAndSetConnectionStringForConnection();
                    try
                    {
                        Open();
                        result = connection.Query<T>(sql, param, buffered:buffered);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Has been occurred an error check the internal exception for more details.", ex);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Execute statement select on DB and return anonymous result
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        internal IEnumerable<dynamic> Query(string sql, object param=null, bool buffered = true)
        {
            IEnumerable<dynamic> result = null;
            using (connection)
            {
                if (connection.State == ConnectionState.Closed)
                {
                    try
                    {
                        Open();
                        result = connection.Query(sql, param, buffered: buffered);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Has been occurred an error check the internal exception for more details.", ex);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Execute statement select on DB and return first or default anonymous result
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        internal T QueryFirstOrDefault<T>(string sql, object param = null)
        {
            T result = default(T);
            using (connection)
            {
                if (connection.State == ConnectionState.Closed)
                {
                    CheckAndSetConnectionStringForConnection();
                    try
                    {
                        Open();
                        result = connection.QueryFirstOrDefault<T>(sql, param);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Has been occurred an error check the internal exception for more details.", ex);
                    }
                }
            }
            return result;
        }
        #endregion

        #region STATEMENT INSERT, DELETE, UPDATE AND PROCEDURE DB METHODS
        /// <summary>
        /// Execute someone insert, delete or update statement DB
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        internal int Execute(string sql, object param=null, CommandType? commandType = null)
        {
            int result = 0;
            using (connection)
            {
                if (connection.State == ConnectionState.Closed)
                {
                    CheckAndSetConnectionStringForConnection();
                    try
                    {
                        Open();
                        result = connection.Execute(sql, param: param, commandType: commandType);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Has been occurred an error check the internal exception for more details.", ex);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Execute someone insert, delete or update statement DB
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        internal static int Execute(string connectionString, string sql, object param=null, CommandType? commandType = null)
        {
            int result = 0;
            using (IDbConnection Connection = new SqlConnection(connectionString))
            {
                if (Connection.State == ConnectionState.Closed)
                {
                    try
                    {
                        Connection.Open();
                        result = Connection.Execute(sql, param: param, commandType: commandType);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Has been occurred an error check the internal exception for more details.", ex);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Execute someone insert, delete or update statement DB
        /// </summary>
        /// <param name="Connection"></param>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        internal static int Execute(SqlConnection Connection, string sql, object param=null, CommandType? commandType = null)
        {
            int result = 0;
            using (Connection)
            {
                if (Connection.State == ConnectionState.Closed)
                {
                    try
                    {
                        Connection.Open();
                        result = Connection.Execute(sql, param: param, commandType: commandType);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Has been occurred an error check the internal exception for more details.", ex);
                    }
                }
            }
            return result;
        }
        #endregion

        #region DISPOSE METHODS

        /// <summary>
        /// DatabaseModel Destructor
        /// </summary>
        ~DapperModel()
        {
            Dispose(false);
        }

        /// <summary>
        /// Dispose the DDBB.
        /// </summary>
        /// <exception cref="Exception"></exception>
        public void Dispose()
        {
            try
            {
                Dispose(true);
            }
            catch (Exception ex)
            {
                throw new Exception("Has been occurred an error check the internal exception for more details.", ex);
            }

            /* Take yourself off the Finalization queue to prevent finalization code
             * for this object from executing a second time.
             */
            GC.SuppressFinalize(this);
        }

        /* Dispose(bool disposing) executes in two distinct scenarios.
         * If disposing equals true, the method has been called directly or indirectly
         * by a user's code. Managed and unmanaged resources can be disposed.
         * If disposing equals false, the method has been called by the runtime from
         * inside the finalizer and you should not reference other objects. Only
         * unmanaged resources can be disposed.
         */
        /// <summary>
        /// Virtual dispose the DDBB.
        /// </summary>
        /// <exception cref="Exception"></exception>
        protected internal virtual void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!this.disposed)
            {
                // If disposing equals true, dispose all managed
                // and unmanaged resources.
                if (disposing)
                {
                    // Dispose managed resources.
                    if (connection != null)
                    {
                        try
                        {
                            this.Close();
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Has been occurred an error check the internal exception for more details.", ex);
                        }
                        connection.Dispose();
                        connection = null;
                        ConnectionString = String.Empty;
                    }
                }
                /* Release unmanaged resources. If disposing is false, only the following code
                 * is executed. Note that this is not thread safe. Another thread could start
                 * disposing the object after the managed resources are disposed, but before
                 * the disposed flag is set to true. If thread safety is necessary, it must be
                 * implemented by the client.
                 */
            }
            disposed = true;
        }
        #endregion

        #region QUERY MULTIPLE
        /// <summary>
        /// Eecute many sql statement on DB.
        /// </summary>
        /// <param name="queries"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        internal SqlMapper.GridReader QueryMultiple(IEnumerable<string> queries, object parameters=null)
        {
            SqlMapper.GridReader result = null;
            using (connection)
            {
                if (connection.State == ConnectionState.Closed)
                {
                    CheckAndSetConnectionStringForConnection();
                    try
                    {
                        //connection.Open();
                        Open();
                        result = connection.QueryMultiple(String.Join(";", queries.ToString()), parameters);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Has been occurred an error check the internal exception for more details.", ex);
                    }
                }
            }
            return result;
        }
        #endregion

        #region TRANSACTION
        /// <summary>
        /// Execute a transaction on DB. 
        /// <para>The parameter is a <see langword="internal struct " cref="DapperSimpleQuery"/>.</para>
        /// </summary>
        /// <param name="sqls"></param>
        /// <returns></returns>
        /// <exception cref="SqlException"></exception>
        internal int Transaction(IEnumerable<DapperSimpleQuery> sqls)
        {
            int result = 0;
            using (connection)
            {
                if (connection.State == ConnectionState.Closed)
                {
                    CheckAndSetConnectionStringForConnection();
                    //connection.Open();
                    Open();

                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            foreach (DapperSimpleQuery query in sqls)
                            {
                                result += connection.Execute(query.Sql, query.Param, transaction);
                            }
                            transaction.Commit();
                        }
                        catch(SqlException ex)
                        {
                            transaction.Rollback();
                            throw new Exception("Exception by a execute statement sql on DB, check the inner exception for more detail.",ex);
                        }
                    }
                }
            }
            return result;
        }

        #endregion

        #region MULTI MAPPING
        /// <summary>
        /// Query method can execute a query and map the result to a strongly typed list with a one to many relations or none to one relations.
        /// </summary>
        /// <typeparam name="TFirst"></typeparam>
        /// <typeparam name="TSecond"></typeparam>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="sql"></param>
        /// <param name="map"></param>
        /// <param name="param"></param>
        /// <param name="buffered"></param>
        /// <param name="splitOn"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        /// <example>
        /// <code>
        /// (One to One)
        /// var invoices = connection.Query<Invoice, InvoiceDetail, Invoice>(
        ///     sql,
        ///        (invoice, invoiceDetail) =>
        ///        {
        ///            invoice.InvoiceDetail = invoiceDetail;
        ///            return invoice;
        ///        },
        ///        splitOn: "InvoiceID")
        ///     .Distinct()
        ///     .ToList();
        ///     
        /// (One to Many)   
        /// var orderDictionary = new Dictionary<int, Order>();
        ///
        ///       var list = connection.Query<Order, OrderDetail, Order>(
        ///          sql,
        ///         (order, orderDetail) =>
        ///         {
        ///             Order orderEntry;
        ///
        ///             if (!orderDictionary.TryGetValue(order.OrderID, out orderEntry))
        ///             {
        ///                 orderEntry = order;
        ///                 orderEntry.OrderDetails = new List<OrderDetail>();
        ///                 orderDictionary.Add(orderEntry.OrderID, orderEntry);
        ///             }
        ///
        ///             orderEntry.OrderDetails.Add(orderDetail);
        ///             return orderEntry;
        ///         },
        ///         splitOn: "OrderDetailID")
        ///     .Distinct()
        ///     .ToList();
        /// </code>
        /// </example>
        internal IEnumerable<TReturn> Query<TFirst,TSecond, TReturn>(string sql, Func<TFirst, TSecond, TReturn> map, object param = null, string splitOn = null) {
            
            IEnumerable<TReturn> result = null;
            using (connection)
            {
                if (connection.State == ConnectionState.Closed)
                {
                    CheckAndSetConnectionStringForConnection();
                    try
                    {
                        Open();
                        if(splitOn == null)
                            result = connection.Query<TFirst, TSecond, TReturn>(sql, map, param);
                        else
                            result = connection.Query<TFirst, TSecond, TReturn>(sql, map, param, splitOn: splitOn);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Has been occurred an error check the internal exception for more details.", ex);
                    }
                }
            }
            return result;
        }
        #endregion

        #region INFORMATION SQLCONNECTION
        /// <summary>
        /// Get state of DB connection.
        /// </summary>
        /// <returns></returns>
        public ConnectionState State()
        {
            return connection == null ? ConnectionState.Broken : connection.State;
        }

        public int? ConnectionTimeout()
        {
            return connection == null ? 0 : connection.ConnectionTimeout;
        }

        /// <summary>
        /// Get database's name.
        /// </summary>
        /// <returns></returns>
        public string DatabaseName()
        {
            return connection == null ? String.Empty : connection.Database;
        }
        #endregion

        #endregion

        #region - S E T T E R S A N D S E T T E R S
        /// <summary>
        /// 
        /// </summary>
        public string ConnectionString
        {
            get
            {
                return connectionString;
            }

            set
            {
                connectionString = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public IDbConnection Connection
        {
            get
            {
                return connection;
            }

            set
            {
                connection = value;
            }
        }

        #endregion
    }

}
