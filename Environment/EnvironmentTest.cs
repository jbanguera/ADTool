using System.Collections.Generic;
using ADTool.Data;

namespace ADTool.Environment
{

    /// <summary>
    /// Enviroment class test
    /// </summary>
    public class EnvironmentTest: IEnvironment
    {
        #region - C O N S T R U C T O R
        /// <summary>
        /// Constructor
        /// </summary>
        public EnvironmentTest()
        {
        }
        #endregion

        #region - P R O P E R T I E S
        /// <summary>
        /// Nombre del entorno
        /// </summary>
        public string EnvironmentName { get; set; }
        /// <summary>
        /// Indica si el entorno es activo o no
        /// </summary>
        public bool ActiveEnvironment { get; set; }
        /// <summary>
        /// Indica el tipo de entorno
        /// </summary>
        public DataEnvironmentTypeEnum DataEnvironmentType { get; set; }


        /// <summary>
        /// Lista con las distintas conexiones del entorno
        /// </summary>
        public IEnumerable<DataConnection> Connections { get; set; }
        /// <summary>
        /// Direccion FTP de archivos EDI
        /// </summary>
        public string FTPEDIFiles { get; set; }

        /// <summary>
        /// Separador de decimales de EDI
        /// </summary>
        public string DecimalSeparatorFTPEDIFiles { get; set; }

        /// <summary>
        /// Order Folder FTP de archivos EDI
        /// </summary>
        public string OrderFolderFTPEDIFiles { get; set; }

        /// <summary>
        /// Invoic Folder FTP de archivos EDI
        /// </summary>
        public string InvoicFolderFTPEDIFiles { get; set; }

        /// <summary>
        /// Working Folder FTP de archivos EDI Order
        /// </summary>
        public string WorkingFolderOrderFTPEDIFiles { get; set; }

        /// <summary>
        /// Processed Folder FTP de archivos EDI Order
        /// </summary>
        public string WorkingFolderInvoicFTPEDIFiles { get; set; }

        /// <summary>
        /// User FTP de archivos EDI
        /// </summary>
        public string UserFTPEDIFiles { get; set; }

        /// <summary>
        /// Password FTP de archivos EDI
        /// </summary>
        public string PasswordFTPEDIFiles { get; set; }

        #endregion

        #region - M E T H O D S
        /// <summary>
        /// Check if the enviroment is a production environment
        /// </summary>
        /// <returns></returns>
        public bool IsProduction()
        {
            return this.DataEnvironmentType == DataEnvironmentTypeEnum.Production;
        }
        #endregion
    }
}
