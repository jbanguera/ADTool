using System;
using System.Collections.Generic;
using ADTool.Serializer;
using ADTool.CustomFile;
using System.IO;
using System.Linq;

namespace ADTool.Log
{
    /// <summary>
    /// Class for manage all class that implement the Ilog interface.
    /// With this class you can Load, Get, Add, And Save your logs.
    /// All logs are saved with JSON format.
    /// <para></para>
    /// In this namespace there's a simple exemple class (<see cref="LogJsonFile"/>) for use this manage class (<see cref="LogManager{T}"/>)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LogManager<T> where T: ILog, new()
    {
        #region - P R O P E R T I E S
        /// <summary>
        /// field for storage the all log
        /// </summary>
        public IEnumerable<T> DataLogList { get; set; }
        /// <summary>
        /// field to storage the path file log
        /// </summary>
        private string pathFileLog;
        #endregion

        #region - C O N S T R U C T O R
        /// <summary>
        /// Constructor
        /// </summary>
        public LogManager()
        {
            DataLogList = null;
            pathFileLog = String.Empty;
        }
        #endregion

        #region - M E T H O D S
        /// <summary>
        /// Get all Log
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public IEnumerable<T> GetLog() {
            if (DataLogList == null) throw new Exception("The logs file has not been load yet");
            return DataLogList;
        }
        /// <summary>
        /// Add log to manager, but it's not save yet, you must execute the <see cref="SaveLog "/> method
        /// </summary>
        /// <param name="newLog"></param>
        /// <exception cref="Exception"></exception>
        public void AddLog(T newLog)
        {
            if (DataLogList == null) throw new Exception("The logs file has not been load yet");
            int newIdLog = DataLogList.Count() > 0 ? DataLogList.Max(l => l.IdLog) + 1 : 1;
            newLog.IdLog = newIdLog;
            ((List<T>)DataLogList).Add(newLog);
        }
        /// <summary>
        /// Load all log saved in path file passed by parameter
        /// </summary>
        /// <param name="pathFileLog"></param>
        /// <exception cref="Exception"></exception>
        public void LoadLog(string pathFileLog)
        {
            try
            {
                if (pathFileLog == String.Empty) throw new Exception("The path log file cannot be empty");
                string logsJson = FileManager.GetFile(this.pathFileLog = pathFileLog);
                if (logsJson == String.Empty) throw new Exception("The log file cannot be empty");
                object LogList = JsonSerializer.Deserialize(logsJson, typeof(LogManager<T>));
                DataLogList = ((LogManager<T>)LogList).DataLogList;
            }
            catch (FileNotFoundException) {
                try
                {
                    SaveEmptyLog(pathFileLog);
                    LoadLog(pathFileLog);
                }
                catch (Exception ex) { throw new Exception("It's not possible create a empty file log, for more information read the inner exception", ex); }
            }
            catch (Exception e) { throw e; }
        }
        /// <summary>
        /// Save all log load in file without adding, overwritting all text storaged
        /// </summary>
        /// <exception cref="Exception"></exception>
        public void SaveLog()
        {
            if (DataLogList == null || String.IsNullOrEmpty(this.pathFileLog) ) throw new Exception("The logs file has not been load yet");
            try
            {
                string datalogJson = JsonSerializer.Serialize(this);
                FileManager.SaveTextFile(this.pathFileLog, datalogJson, false);
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        /// Save empty log, is use for create file log if not exist
        /// </summary>
        /// <param name="pathFileLog"></param>
        /// <exception cref="Exception"></exception>
        private void SaveEmptyLog(string pathFileLog) {
            try
            {
                FileManager.SaveTextFile(pathFileLog, String.Empty, false);
                LogManager<T> logManagerTmp = new LogManager<T>();
                logManagerTmp.DataLogList = new List<T>();
                logManagerTmp.pathFileLog = pathFileLog;
                logManagerTmp.SaveLog();
            }
            catch (Exception ex) { throw ex; }
        }
        #endregion
    }
}
