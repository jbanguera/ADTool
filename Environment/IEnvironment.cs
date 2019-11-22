using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADTool.Environment
{
    public interface IEnvironment
    {
        #region - P R O P E R T I E S
        /// <summary>
        /// Fied to know Environment name
        /// </summary>
        string EnvironmentName { get; set; }
        /// <summary>
        /// Field to know if Environment is active
        /// </summary>
        bool ActiveEnvironment { get; set; }
        /// <summary>
        /// Field to know the type of Environment, the types are <see cref="DataEnvironmentTypeEnum"></see>
        /// </summary>
        DataEnvironmentTypeEnum DataEnvironmentType { get; set; }
        #endregion
    }
}
