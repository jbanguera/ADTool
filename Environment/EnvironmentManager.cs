using ADTool.CustomFile;
using ADTool.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ADTool.Environment
{
    /// <summary>
    /// class for get environment manager generic
    /// </summary>
    public class EnvironmentManager<T> where T: IEnvironment, new()
    {
        #region - P R O P E R T I E S
        /// <summary>
        /// List of environment
        /// </summary>
        public IEnumerable<T> DataEnvironmentList { get; set; }
        #endregion

        #region - C O N S T R U C T O R
        /// <summary>
        /// private constructor
        /// </summary>
        public EnvironmentManager()
        {
            DataEnvironmentList = null;
        }
        #endregion

        #region - M E T H O D S
        /// <summary>
        /// Load all envrionment saved
        /// </summary>
        /// <param name="environmentsConfigFilePath"></param>
        /// <exception cref="Exception"></exception>
        public void LoadEnvironment(string environmentsConfigFilePath)
        {
            try
            {
                if (environmentsConfigFilePath == String.Empty) throw new Exception("The path environment file cannot be empty");
                string logsJson = FileManager.GetFile(environmentsConfigFilePath);
                if (logsJson == String.Empty) throw new Exception("The environment file config cannot be empty");
                object envManager = JsonSerializer.Deserialize(logsJson, typeof(EnvironmentManager<T>));
                DataEnvironmentList = ((EnvironmentManager<T>)envManager).DataEnvironmentList;
            }
            catch (Exception e) { throw e; }
        }
        /// <summary>
        /// Get All environment
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public IEnumerable<T> GetEnvironments()
        {
            if (DataEnvironmentList == null) throw new Exception("The environment file config file has not been load yet");
            return DataEnvironmentList;
        }
        /// <summary>
        /// Get the active environment
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"
        public T ActivEnvironment() {
            if (DataEnvironmentList == null) throw new Exception("The environment file config file has not been load yet");
            return DataEnvironmentList.First(env => env.ActiveEnvironment);
        }
        /// <summary>
        /// Execute this method for know is data from file config environment has been loaded
        /// </summary>
        /// <returns></returns>
        public bool IsEnvironmentLoaded()
        {
            return DataEnvironmentList == null ? false : true;
        }
        #endregion
    }
}
