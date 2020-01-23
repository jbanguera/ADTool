using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADTool.Data
{
    public enum DataConnectionTypeProviderEnum
    {
        /// <summary>
        /// Identify to SQL Server provider
        /// </summary>
        MSSQL = 1,
        /// <summary>
        /// Identify to MySQL Server provider
        /// </summary>
        MYSQL = 2,
        /// <summary>
        /// Identify to SQLite provider
        /// </summary>
        SQLITE = 3
    }
}
