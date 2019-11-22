using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADTool.Log
{
    /// <summary>
    /// Struct for storage the exception information
    /// </summary>
    public struct LogException
    {
        #region - P R O P E R T I E S
        /// <summary>
        /// Field to know the method that throw the exception
        /// </summary>
        public string MethodWhereHappened { get; set; }
        /// <summary>
        /// Field to know the class that throw the exception
        /// </summary>
        public string ClassWhereHappened { get; set; }
        /// <summary>
        /// Field to know messge exception
        /// </summary>
        public string ExceptionMessage { get; set; }
        /// <summary>
        /// Field to know stack trace
        /// </summary>
        public string StackTrace { get; set; }
        #endregion

        #region - C O N S T R U C T O R
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="methodWhereHappened"></param>
        /// <param name="classWhereHappened"></param>
        /// <param name="exceptionMessage"></param>
        /// <param name="stackTrace"></param>
        public LogException(string methodWhereHappened, string classWhereHappened, string exceptionMessage, string stackTrace)
        {
            MethodWhereHappened = methodWhereHappened;
            ClassWhereHappened = classWhereHappened;
            ExceptionMessage = exceptionMessage;
            StackTrace = stackTrace;
        }
        #endregion
    }
}
