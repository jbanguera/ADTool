using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADTool.Log;
using ADTool.CustomFile;
using System.IO;
using ADTool.Environment;
using ADTool.Encryption;

namespace EvaluatorTest
{
    class Program
    {
        static void Main(string[] args)
        {

            /*
             * Testing the function logs
             
            LogManager<LogJsonFile> logManager = new LogManager<LogJsonFile>();
            logManager.LoadLog(Path.Combine(FileManager.GetApplicationPath(), "logFileTmpNotExists"));
            var listLog = logManager.GetLog();
            logManager.AddLog(new LogJsonFile("jairo","read","warning read",LogType.Warning, new LogException("...","...","...","..."), null));
            logManager.SaveLog();
            
             */

            var pass = new CryptLib().Encrypt("CHLfYKzLRqRq7kJ");

            EnvironmentManager<EnvironmentTest> envManager = new EnvironmentManager<EnvironmentTest>();
            envManager.LoadEnvironment(Path.Combine(FileManager.GetApplicationPath(), "env.cfg"));
            EnvironmentTest envActive = envManager.ActivEnvironment();
            int tmp = 0;
        }
    }
}
