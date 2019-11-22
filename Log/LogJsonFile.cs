using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADTool.Log
{
    /// <summary>
    /// Class exemple for test the manage class log <see cref="LogManager{T}"/>
    /// </summary>
    public class LogJsonFile : ILog
    {
        #region - P R O P E R T I E S
        /// <summary>
        /// Field to identified a log
        /// </summary>
        public int IdLog { get; set; }
        /// <summary>
        /// Field to know the client
        /// </summary>
        public string Client { get; set; }
        /// <summary>
        /// Field to know the task name
        /// </summary>
        public string TaskName { get; set; }
        /// <summary>
        /// Field to know the text message
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// Field to know the log type
        /// </summary>
        public LogType LogType { get; set; }
        /// <summary>
        /// Field to know the exception information
        /// </summary>
        public LogException? Exception { get; set; }
        /// <summary>
        /// Field to know the date create log
        /// </summary>
        public string CreatedAt { get; set; }
        /// <summary>
        /// Custom specific property
        /// </summary>
        public int CustomProperty { get; set; }
        #endregion

        #region - C O N S T R U C T O R S
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client"></param>
        /// <param name="taskName"></param>
        /// <param name="text"></param>
        /// <param name="logType"></param>
        /// <param name="exception"></param>
        /// <param name="createdAt"></param>
        public LogJsonFile(string client, string taskName, string text, LogType logType, LogException? exception, string createdAt)
        {
            Client = client;
            TaskName = taskName;
            Text = text;
            LogType = logType;
            Exception = exception;
            CreatedAt = createdAt;
        }
        /// <summary>
        /// Empty Constructor
        /// </summary>
        public LogJsonFile()
        {
        }
        #endregion
    }
}
