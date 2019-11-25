using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADTool.Data.DapperClient
{
    /// <summary>
    /// Class to manage de data base resources
    /// </summary>
    public class DapperManager
    {
        #region - P R O P E R T I E S
        private bool disposed = false;
        #endregion

        #region - C O N S T R U C T O R
        /// <summary>
        /// Constructor <see langword="class " cref="DapperManager"/>
        /// </summary>
        /// <param name="connectionString"></param>
        public DapperManager()
        {
        }

        #endregion

        #region - C H E C K C O N N E C T I O N
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dapperModel"></param>
        /// <returns></returns>
        public bool ThereIsConectivity(DapperModel dapperModel)
        {
            return dapperModel.ThereIsConectivity();
        }
        #endregion

        #region - M E T H O D S

        #region STATEMENT SELECT DB METHODS
        /// <summary>
        /// Execute Select statement and return <see langword="T"/> object mapping the column table with <see langword="T"/> object properties.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connectionString"></param>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static IEnumerable<T> Query<T>(string connectionString, string sql, object param=null)
        {
            IEnumerable<T> result = null;
            try
            {
                result = DapperModel.Query<T>(connectionString, sql, param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        /// <summary>
        /// Execute Select statement and return <see langword="T"/> object mapping the column table with <see langword="T"/> object properties.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Connection"></param>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public IEnumerable<T> Query<T>(SqlConnection Connection, string sql, object param=null)
        {
            IEnumerable<T> result = null;
            try
            {
                result = DapperModel.Query<T>(Connection, sql, param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        /// <summary>
        /// Execute Select statement and return <see langword="T"/> object mapping the column table with <see langword="T"/> object properties.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public IEnumerable<T> Query<T>(DapperModel dapperModel, string sql, object param=null)
        {
            IEnumerable<T> result = null;
            try
            {
                result = dapperModel.Query<T>(sql, param);
            }
            catch (Exception ex)
            {
                throw ex;
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
        public IEnumerable<dynamic> Query(DapperModel dapperModel, string sql, object param=null)
        {
            IEnumerable<dynamic> result = null;
            try
            {
                result = dapperModel.Query(sql, param);
            }
            catch (Exception ex)
            {
                throw ex;
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
        public T QueryFirstOrDefault<T>(DapperModel dapperModel, string sql, object param = null)
        {
            T result = default(T);
            try
            {
                result = dapperModel.QueryFirstOrDefault<T>(sql, param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        #endregion

        #region STATEMENT INSERT DB METHODS
        /// <summary>
        /// Insert DB row 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public int Insert(DapperModel dapperModel, string sql, object param=null)
        {
            int result = 0;
            try
            {
                result = dapperModel.Execute(sql, param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        #endregion

        #region STATEMENT UPDATE DB METHODS
        /// <summary>
        /// Update DB row
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public int Update(DapperModel dapperModel, string sql, object param=null)
        {
            int result = 0;
            try
            {
                result = dapperModel.Execute(sql, param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        #endregion

        #region EXECUTE
        /// <summary>
        /// Execute any statement
        /// </summary>
        /// <param name="dapperModel"></param>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public int Execute(DapperModel dapperModel, string sql, object param = null)
        {
            int result = 0;
            try
            {
                result = dapperModel.Execute(sql, param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        #endregion

        #region STATEMENT DELETE DB METHODS
        /// <summary>
        /// Delete DB row 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public int Delete(DapperModel dapperModel, string sql, object param=null)
        {
            int result = 0;
            try
            {
                result = dapperModel.Execute(sql, param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        #endregion

        #region EXECUTE PROCEDURE DB METHODS
        /// <summary>
        /// Delete DB row 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public int StorageProcedure(DapperModel dapperModel, string sql, object param=null)
        {
            int result = 0;
            try
            {
                result = dapperModel.Execute(sql, param, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        #endregion

        #region QUERY MULTIPLE METHODS
        /// <summary>
        /// Execute many diference sql statement on DB. 
        /// </summary>
        /// <param name="queries"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public SqlMapper.GridReader QueryMultiple(DapperModel dapperModel, List<string> queries, object parameters=null)
        {
            SqlMapper.GridReader result = null;
            try
            {
                result = dapperModel.QueryMultiple(queries, parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        #endregion

        #region TRANSACTION
        /// <summary>
        /// Execute the queries in a transaction. The queries and parameters must have the same Dictionary Key.
        /// </summary>
        /// <param name="sqls"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        /// <exception cref="SqlException"></exception>
        public int Transaction(DapperModel dapperModel, Dictionary<int,string> sqls, Dictionary<int, object> param=null)
        {
            int result = 0;
            List<DapperSimpleQuery> queries = new List<DapperSimpleQuery>();

            if (param == null)
            {
                foreach (KeyValuePair<int, string> sql in sqls)
                {
                    queries.Add(new DapperSimpleQuery(sql.Value, null));
                }
            }
            else
            {
                foreach (KeyValuePair<int, string> sql in sqls)
                {
                    queries.Add(new DapperSimpleQuery(sql.Value, param[sql.Key]));
                }
            }
            

            try{
                result = dapperModel.Transaction(queries);
            }
            catch(SqlException ex)
            {
                throw ex;
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
        /// <param name="dapperModel"></param>
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
        public IEnumerable<TReturn> Query<TFirst, TSecond, TReturn>(DapperModel dapperModel, string sql, Func<TFirst, TSecond, TReturn> map, object param = null, string splitOn = null)
        {
            IEnumerable<TReturn> result = null;
            try
            {
                result = dapperModel.Query<TFirst, TSecond, TReturn>(sql, map, param, splitOn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        #endregion

        #region MISCELLANEOUS
        /// <summary>
        /// Build the DDBB connection string BD.
        /// </summary>
        /// <returns>The DDBB connection string.</returns>
        public static string BuildConnectionString(string DataSource, string dBUser, string password)
        {
            // Compose the connection string, if not previously composed.
            try
            { DataSource = $"{DataSource}User ID={dBUser};Password={password};"; }
            catch
            { DataSource = null; }
            return DataSource;
        }
        #endregion
       
        #endregion
    }


}
