
namespace ADTool.Data
{
    public interface IDataConnection
    {
        #region - P R O P E R T I E S
        /// <summary>
        /// Field to know the connection name
        /// </summary>
        string ConnectionName { get; set; }

        /// <summary>
        /// Field to know the server name
        /// </summary>
        string ServerName { get; set; }

        /// <summary>
        /// Field to know the catalog
        /// </summary>
        string Catalog { get; set; }

        /// <summary>
        /// Field to know the catalog
        /// </summary>
        string Database { get; set; }

        /// <summary>
        /// 
        /// </summary>
        string Port { get; set; }

        /// <summary>
        /// Ssl Mode
        /// </summary>
        string SslMode { get; set; }

        /// <summary>
        /// Connection Time out
        /// </summary>
        string ConnectionTimeOut { get; set; }

        /// <summary>
        /// Field to know the IntegratedSecurity
        /// </summary>
        string IntegratedSecurity { get; set; }

        /// <summary>
        /// Field to know User
        /// </summary>
        string User { get; set; }

        /// <summary>
        /// Field to know password user
        /// </summary>
        string Password { get; set; }

        /// <summary>
        /// Field to know persist security
        /// </summary>
        string PersistSecurity { get; set; }

        /// <summary>
        /// Field to know the connection string for SQL Server
        /// </summary>
        string ConnectionStringForSql { get; }

        /// <summary>
        /// Field to know the connection string for MySQL
        /// </summary>
        string ConnectionStringForMySql { get; }
        #endregion

        #region - M E T H O D S
        /// <summary>
        /// Get the connection for SQL Server
        /// </summary>
        /// <returns></returns>
        string GetConnectionStringForSql();

        /// <summary>
        /// Get the connection for MYSQL Server
        /// </summary>
        /// <returns></returns>
        string GetConnectionStringForMySql();
        #endregion
    }
}