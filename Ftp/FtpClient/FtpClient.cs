using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADTool.Data;
using ADTool.Encryption;
using FluentFTP;

namespace ADTool.Ftp.FtpClient
{
    /// <summary>
    /// Class to access and perform operation on FTP server
    /// </summary>
    public class FtpClient
    {
        #region - P R O P E R T I E S
        /// <summary>
        /// Data connection
        /// </summary>
        private DataConnection connection = null;

        /// <summary>
        /// Client FTP
        /// </summary>
        private FluentFTP.FtpClient _FTPClient = null;
        #endregion

        #region - C O N S T R U C T O R S
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connection"> Data connection FTP</param>
        public FtpClient(DataConnection connection)
        {
            Connection = connection;
        }

        #endregion

        #region - M E T H O D S 

        /// <summary>
        /// Check if remote directory exists directory
        /// </summary>
        /// <param name="remotePath">Remote absolute path directory</param>
        /// <returns></returns>
        public bool DirectoryExists(string remotePath)
        {
            _FTPClient.GetListing("/");
            return _FTPClient.DirectoryExists(remotePath);
        }

        /// <summary>
        /// Create directory in FTP Server
        /// </summary>
        /// <param name="remotePath">Remote absolute path directory</param>
        public void CreateDirectory(string remotePath)
        {
            _FTPClient.CreateDirectory(remotePath);
        }

        /// <summary>
        /// Get list full name item inside remote folder
        /// </summary>
        /// <param name="remotePath">Remote absolute path directory</param>
        /// <returns></returns>
        public IEnumerable<string> GetListFullName(string remotePath)
        {
            IEnumerable<string> result = _FTPClient.GetListing(remotePath).Select(item => item.FullName);
            return result;
        }

        /// <summary>
        /// Remove File on FTP server
        /// </summary>
        /// <param name="pathFileOnFTP">absolute path where is the file to remove</param>
        /// <returns></returns>
        public bool RemoveFileOnFTPServer(string pathFileOnFTP)
        {
            _FTPClient.GetListing("/");

            if (_FTPClient.FileExists(pathFileOnFTP))
                _FTPClient.DeleteFile(pathFileOnFTP);
            return !_FTPClient.FileExists(pathFileOnFTP);
        }

        /// <summary>
        /// Upload file on FTP server and retry 3 times before giving up
        /// </summary>
        /// <param name="sourcePath">source path file</param>
        /// <param name="destinationPath">destination path file</param>
        public void UploadFile(string sourcePath, string destinationPath)
        {
            _FTPClient.RetryAttempts = 3;
            _FTPClient.UploadFile(sourcePath, destinationPath, FtpExists.Overwrite, false, FtpVerify.Retry);
        }

        /// <summary>
        /// Active FTP client
        /// </summary>
        public void Connecte()
        {
            if (!IsActiveFTPClient())
            {
                _FTPClient = new FluentFTP.FtpClient(connection.ServerName, connection.User, new ADTool.Encryption.CryptLib().Decrypt(connection.Password));
                _FTPClient.Connect();
            }
        }

        /// <summary>
        /// Disconnect FTP client and free memory used
        /// </summary>
        public void Disconnect()
        {
            if (IsActiveFTPClient()) _FTPClient.Disconnect();
            _FTPClient = null;
        }

        /// <summary>
        /// Check if FTP client is connected yet
        /// </summary>
        /// <returns>boolean value to know if still we're connected to FTP server</returns>
        public bool IsActiveFTPClient()
        {
            return _FTPClient != null && _FTPClient.IsConnected;
        }

        /// <summary>
        /// Get or Set connection FTP server object
        /// </summary>
        public DataConnection Connection
        {
            get
            {
                return connection;
            }

            set
            {
                connection = value;
            }
        }
        #endregion
    }
}
