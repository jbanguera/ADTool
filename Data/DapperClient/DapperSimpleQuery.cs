using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADTool.Data.DapperClient
{
    internal struct DapperSimpleQuery
    {
        #region - P R O P E R T I E S
        string sql;
        object param;
        #endregion

        #region - C O N S T R U C T
        /// <summary>
        /// Constructor <see langword="struct " cref="DapperSimpleQuery"/>.
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        public DapperSimpleQuery(string sql, object param)
        {
            this.sql = sql;
            this.param = param;
        }
        #endregion

        #region - S E T T E R S A N D G E T T E R S
        /// <summary>
        /// Get and Set sql property.
        /// </summary>
        public string Sql
        {
            get
            {
                return sql;
            }

            set
            {
                sql = value;
            }
        }
        /// <summary>
        /// Get and Set param property.
        /// </summary>
        public object Param
        {
            get
            {
                return param;
            }

            set
            {
                param = value;
            }
        }
        #endregion

    }
}
