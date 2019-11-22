using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADTool.Log
{
    public interface ILog
    {
        #region - P R O P E R T I E S
        /// <summary>
        /// Field to identified a log
        /// </summary>
        int IdLog { get; set; }
        /// <summary>
        /// Name user
        /// </summary>
        string Client { get; set; }
        /// <summary>
        /// Name task
        /// </summary>
        string TaskName { get; set; }
        /// <summary>
        /// Text log
        /// </summary>
        string Text { get; set; }
        /// <summary>
        /// Type log
        /// </summary>
        LogType LogType { get; set; }
        /// <summary>
        /// Log exeption information
        /// </summary>
        LogException? Exception { get; set; }
        /// <summary>
        /// Date created log
        /// </summary>
        string CreatedAt { get; set; }
        #endregion
    }
}
