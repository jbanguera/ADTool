using ADTool.Encryption;
using System;

namespace ADTool.Data
{
    /// <summary>
    /// Clase con las propiedades de una conexión
    /// </summary>
    public class DataConnection : IDataConnection
    {
        #region - P R O P E R T I E S
        /// <summary>
        /// Connection name
        /// </summary>
        public string ConnectionName { get; set; }

        /// <summary>
        /// Server name
        /// </summary>
        public string ServerName { get; set; }

        /// <summary>
        /// Catalog
        /// </summary>
        public string Catalog { get; set; }

        /// <summary>
        /// Catalog
        /// </summary>
        public string Database { get; set; }
        
        /// <summary>
        /// Port
        /// </summary>
        public string Port { get; set; }

        /// <summary>
        /// Mode Ssl
        /// </summary>
        public string SslMode { get; set; }

        /// <summary>
        /// Integrated Security
        /// </summary>
        public string IntegratedSecurity { get; set; }

        /// <summary>
        /// Connection Time Out
        /// </summary>
        public string ConnectionTimeOut { get; set; }

        /// <summary>
        /// Name user
        /// </summary>
        public string User { get; set; }

        /// <summary>
        /// Encrypt password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Value Persist Security
        /// </summary>
        public string PersistSecurity { get; set; }
        #endregion

        #region - M E T H O D S
        /// <summary>
        /// Get connection by tyipe provider
        /// </summary>
        /// <param name="providerType"></param>
        /// <returns></returns>
        public string GetConnectionString(DataConnectionTypeProviderEnum providerType)
        {
            return providerType == DataConnectionTypeProviderEnum.MSSQL ? GetConnectionStringForSql() :
                    providerType == DataConnectionTypeProviderEnum.MYSQL ? GetConnectionStringForMySql() : 
                    providerType == DataConnectionTypeProviderEnum.SQLITE ? GetConnectionStringForSqlite() : null;
        }

        /// <summary>
        /// Get the connection For SQLite
        /// </summary>
        /// <returns></returns>
        public string GetConnectionStringForSqlite()
        {
            return $"Data Source={this.ServerName};Version=3;";
        }

        /// <summary>
        /// Get the connection For SQL Server
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string GetConnectionStringForSql()
        {
            try
            {
                CryptLib aDTool = new CryptLib();
                return Convert.ToBoolean(this.IntegratedSecurity ?? "False") ?
                    $"Data Source={this.ServerName};Initial Catalog={this.Catalog};Integrated Security=True;Pooling=False;MultipleActiveResultSets=True;" :
                    $"Data Source={this.ServerName};Initial Catalog={this.Catalog};User Id={this.User};Password={aDTool.Decrypt(this.Password)};Connection Timeout={this.ConnectionTimeOut};Pooling=False;MultipleActiveResultSets=True;";
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// Get the connection for MySQL Server
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string GetConnectionStringForMySql()
        {
            try
            {
                CryptLib aDTool = new CryptLib();
                return String.IsNullOrEmpty(Port) 
                    ? $"database={this.Database}; server={this.ServerName}; SslMode={this.SslMode}; user id={this.User}; pwd={aDTool.Decrypt(this.Password)}" 
                    : $"database={this.Database}; server={this.ServerName}; Port={this.Port}; SslMode={this.SslMode}; user id={this.User}; pwd={aDTool.Decrypt(this.Password)}";
            }
            catch (Exception ex) { throw ex; }
        }

        #endregion
    }

}