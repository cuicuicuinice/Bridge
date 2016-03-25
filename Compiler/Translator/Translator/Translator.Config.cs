using Bridge.Contract;
using Bridge.Translator.Utils;
using System.Diagnostics;
using System.IO;

namespace Bridge.Translator
{
    public partial class Translator
    {
        protected AssemblyConfigHelper assemblyConfigHelper;
        protected AssemblyConfigHelper AssemblyConfigHelper
        {
            get
            {
                if (this.assemblyConfigHelper == null)
                {
                    this.assemblyConfigHelper = new AssemblyConfigHelper(this.Log);
                }

                return this.assemblyConfigHelper;
            }
        }

        protected virtual IAssemblyInfo ReadConfig()
        {
            var config = this.AssemblyConfigHelper.ReadConfig(this.FolderMode, this.Location, this.Configuration);

            return config;
        }

        public virtual void RunEvent(string e)
        {
            var info = new ProcessStartInfo()
            {
                FileName = e
            };
            info.WindowStyle = ProcessWindowStyle.Hidden;

            if (!File.Exists(e))
            {
                throw new Exception("The specified file '" + e + "' couldn't be found." +
                    "\nWarning: Bridge.NET translator working directory: " + Directory.GetCurrentDirectory());
            }

            using (var p = Process.Start(info))
            {
                p.WaitForExit();

                if (p.ExitCode != 0)
                {
                    throw new Exception("Error: The command '" + e + "' returned with exit code: " + p.ExitCode);
                }
            }
        }
    }
}
