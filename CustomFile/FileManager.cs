using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace ADTool.CustomFile
{
    /// <summary>
    /// Class for manage files
    /// </summary>
    public class FileManager
    {
        #region - C O N S T R U C T O R
        /// <summary>
        /// Private constructor
        /// </summary>
        private FileManager() { }
        #endregion

        #region - M E T H O D S
        /// <summary>
        /// Obtiene la ruta actual del ensamblado
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static string GetAssemblyPath()
        {
            var path = string.Empty;
            var path2 = string.Empty;
            try
            {
                var codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                path = Uri.UnescapeDataString(uri.Path);
                path2 = Path.GetDirectoryName(path);
            }
            catch (Exception ex) { throw ex; }
            return path2;
        }

        /// <summary>
        /// Devuelve la ruta de la aplicación
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static string GetApplicationPath()
        {
            var path = String.Empty;
            try
            {
                path = System.Web.HttpRuntime.UsingIntegratedPipeline && System.Web.HttpContext.Current != null ?
                    System.Web.HttpContext.Current.Server.MapPath("~/") : GetAssemblyPath();
            }
            catch (Exception ex) { throw ex; }
            return path;
        }

        /// <summary>
        ///  Get netowrk path of assembly
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static string GetNetPath()
        {
            var sAbsolutePath = string.Empty;
            try
            {
                sAbsolutePath = GetAssemblyPath();
                var sMaquina = Dns.GetHostName();
                string sIp = Dns.GetHostEntry(sMaquina).AddressList[0].ToString();
                sAbsolutePath = $@"\\{sIp}\{sAbsolutePath.Replace(":", "$")}";
            }
            catch (Exception ex) { throw ex; }
            return sAbsolutePath;
        }

        /// <summary>
        /// Get files from subfolder
        /// </summary>
        /// <param name="subFolderName"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static Dictionary<string, string> GetFilesFromAppPath(string subFolderName)
        {
            var scriptList = new Dictionary<string, string>();
            try
            {
                foreach (var filePath in Directory.GetFiles(Path.Combine(GetApplicationPath(), subFolderName)))
                    scriptList.Add(System.IO.Path.GetFileName(filePath), GetFile(filePath));
            }
            catch (Exception ex) { throw ex; }
            return scriptList;
        }

        /// <summary>
        /// Append <see cref="string "/> to file
        /// </summary>
        /// <param name="pathFile"></param>
        /// <param name="msg"></param>
        /// <exception cref="Exception"></exception>
        public static void AppendToFile(string pathFile, string msg)
        {
            try
            {
                File.AppendAllText(pathFile, msg);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Get network path machine IP
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static string GetNetPathMachineIp()
        {
            string sAbsolutePath = String.Empty;
            try
            {
                var sMaquina = Dns.GetHostName();
                var sIp = Dns.GetHostEntry(sMaquina).AddressList[0].ToString();
                sAbsolutePath = $@"\\{sIp} ";
            }
            catch (Exception ex) { throw ex; }
            return sAbsolutePath;
        }

        /// <summary>
        /// Get file from app path
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static string GetFileFromAppPath(string fileName)
        {
            var path = String.Empty;
            try
            {
                path = Path.Combine(GetApplicationPath(), fileName);
            }
            catch (Exception ex) { throw ex; }
            return GetFile(path);
        }

        /// <summary>
        /// Get file from path passed by parameter
        /// </summary>
        /// <param name="sPathFile"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static string GetFile(string sPathFile)
        {
            string sFileName = "";
            try
            {
                if (!File.Exists(sPathFile)) throw new FileNotFoundException("Not exists file");
                StreamReader sFile = File.OpenText(sPathFile);
                sFileName = sFile.ReadToEnd();
                sFile.Close();
            }
            catch (Exception ex) { throw ex; }
            return sFileName;
        }

        /// <summary>
        /// Check if exists file
        /// </summary>
        /// <param name="sPathFile"></param>
        /// <returns></returns>
        public static bool ExistsFile(string sPathFile)
        {
            return File.Exists(sPathFile);
        }

        /// <summary>
        /// Add text content to file, if not exists file is created
        /// </summary>
        /// <param name="sPathFile"></param>
        /// <param name="sFileContent"></param>
        /// <param name="bAppend"></param>
        /// <exception cref="Exception"></exception>
        public static void SaveTextFile(string sPathFile, string sFileContent, bool bAppend)
        {
            TextWriter sFile = null;
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(sFileContent);
                var sPath = GetPath(sPathFile);
                if (!Directory.Exists(sPath))
                    Directory.CreateDirectory(sPath);
                sFile = new StreamWriter(sPathFile, bAppend);
                sFile.Write(sb.ToString());
            }
            catch (Exception ex) { throw ex; }
            finally { sFile.Close(); }
        }

        private static string GetPath(string sPathFile)
        {
            var i = sPathFile.LastIndexOf(@"\", StringComparison.InvariantCulture);
            return sPathFile.Substring(0, sPathFile.Length - (sPathFile.Length - i));
        }

        /// <summary>
        /// Delete file
        /// </summary>
        /// <param name="sPathFile"></param>
        /// <exception cref="Exception"></exception>
        public static void DeleteFile(string sPathFile)
        {
            try
            {
                File.Delete(sPathFile);
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        /// Devuelve TRUE si NO hay diferencias y FALSE si las hay
        /// </summary>
        /// <param name="file1Path"></param>
        /// <param name="file2Path"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static bool CompareTwoFiles(string file1Path, string file2Path)
        {
            try
            {
                string[] linesA = File.ReadAllLines(file1Path);
                string[] linesB = File.ReadAllLines(file2Path);
                IEnumerable<string> onlyB = linesB.Except(linesA);
                return onlyB.ToString().Equals(string.Empty);
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// Check if is valid the path passed by parameter
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsValidPath(string path)
        {
            try
            {
                //- base format
                if (string.IsNullOrEmpty(path.Trim())) return false;
                if (!path.Substring(path.Length - 2, 1).Equals(@"\")) path = $"{path}{@"\"}";
                //- end base format
                Regex driveCheck = new Regex(@"^[a-zA-Z]:\\$");
                if (!driveCheck.IsMatch(path.Substring(0, 3))) return false;
                string strTheseAreInvalidFileNameChars = new string(Path.GetInvalidPathChars());
                strTheseAreInvalidFileNameChars += @":/?*" + "\"";
                Regex containsABadCharacter = new Regex("[" + Regex.Escape(strTheseAreInvalidFileNameChars) + "]");
                if (containsABadCharacter.IsMatch(path.Substring(3, path.Length - 3)))
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Get Machine name
        /// </summary>
        /// <returns></returns>
        public static string GetMachineName()
        {
            try
            {
                return Dns.GetHostName();
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// Save text content in file, if file has content, it's overwrote
        /// </summary>
        /// <param name="fileContent"></param>
        /// <param name="fileName"></param>
        public static void SaveTextFileInAppPath(string fileContent, string fileName)
        {
            var path = Path.Combine(GetAssemblyPath(), fileName);
            SaveTextFile(path, fileContent, false);
        }

        /// <summary>
        /// Load embeded resource
        /// </summary>
        /// <param name="embbededresourceName"></param>
        /// <returns></returns>
        public static string LoadEmbededResource(string embbededresourceName)
        {
            Assembly res = Assembly.GetExecutingAssembly();
            var resource = res.GetManifestResourceNames().First(r => r.IndexOf(embbededresourceName, StringComparison.InvariantCulture) != -1);
            Stream stream = res.GetManifestResourceStream(resource);
            if (stream == null) return null;
            StreamReader strReader = new StreamReader(stream);
            return strReader.ReadToEnd();
        }
        #endregion
    }
}