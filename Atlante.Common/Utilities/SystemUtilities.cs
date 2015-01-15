using Atlante.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Atlante.Common
{
    public class SystemUtilities
    {
        public static string GetProductName(Assembly assembly)
        {
            Logger.AddTrace("Getting Product Name");

            var productAttribute = assembly.GetCustomAttributes(typeof(AssemblyProductAttribute), false).FirstOrDefault();

            if (productAttribute == null)
                return string.Empty;

            return ((AssemblyProductAttribute)productAttribute).Product;
        }

        public static string GetProductCopyright(Assembly assembly)
        {
            Logger.AddTrace("Getting Product Copyright");

            var copyrightAttribute = assembly.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false).FirstOrDefault();

            if (copyrightAttribute == null)
                return string.Empty;

            return ((AssemblyCopyrightAttribute)copyrightAttribute).Copyright;
        }

        public static string GetVersionNumber(Assembly assembly)
        {
            Logger.AddTrace("Getting Product Version");

            return assembly.GetName().Version.ToString();
        }

        public static ImageSource GetApplicationIcon(Assembly assembly)
        {
            Logger.AddTrace("Getting Application Icon");

            Icon icon = Icon.ExtractAssociatedIcon(assembly.Location);
            return Imaging.CreateBitmapSourceFromHIcon(icon.Handle, new Int32Rect(0, 0, icon.Width, icon.Height), BitmapSizeOptions.FromEmptyOptions());
        }

        public static IList<AppLibrary> GetApplicationLibraries(Assembly assembly)
        {
            Logger.AddTrace("Getting Application Libraries");

            var libraries = new List<AppLibrary>();

            var libraryFiles = Directory.GetFiles(Path.GetDirectoryName(assembly.Location), "*.dll");

            foreach (string file in libraryFiles)
            {
                try
                {
                    var fileAssembly = Assembly.LoadFile(file);

                    libraries.Add(new AppLibrary() { Name = GetProductName(fileAssembly), Version = GetVersionNumber(fileAssembly) });
                }
                catch (Exception e)
                {
                    Logger.AddException(e);
                }
            }

            return libraries;
        }

        public static IMessages StartProcess(string processname, string parameters, int timeoutMilliseconds, bool hideWindow = true)
        {
            Messages messages = new Messages();

            Process process = new Process();
            try
            {
                if (processname == string.Empty)
                {
                    messages.AddWarning("Process name is null");
                    return messages;
                }

                process.StartInfo.Arguments = parameters;
                process.StartInfo.FileName = processname;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = hideWindow;
                process.Start();
                process.WaitForExit();

                if (process.ExitCode != 0)
                    messages.AddWarning(string.Format("Error executing {0} {1} with exit code {2}", processname, parameters, process.ExitCode));
            }
            catch (Exception e)
            {
                messages.AddException(e);
            }
            finally
            {
                process.Close();
                process.Dispose();
                process = null;
            }

            return messages;
        }

        public static void StopProcess(string processname)
        {
            Process[] processes = Process.GetProcessesByName(processname);
            try
            {
                foreach (Process process in processes)
                {
                    process.Kill();
                }
            }
            catch (Exception e)
            {
                Logger.AddException(e);
            }
            finally
            {

            }
        }

        /// <summary>
        /// Execute a shell command
        /// </summary>
        /// <param name="parameters">Command line parameters</param> 
        public static IMessages ExecuteShellCommand(string parameters)
        {
            Messages messages = new Messages();

            Process process = new Process();
            try
            {
                // invokes the cmd process specifying the command to be executed.
                string cmdProcess = string.Format(CultureInfo.InvariantCulture, @"{0}\cmd.exe", new object[] { Environment.SystemDirectory });

                // /C tells cmd that we want to execute the command that follows, and then exit.
                parameters = string.Format("/C {0}", parameters);

                process.StartInfo.FileName = cmdProcess;
                process.StartInfo.Arguments = parameters;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.UseShellExecute = false;
                process.Start();
                process.WaitForExit();
            }

            catch (Exception e)
            {
                messages.AddException(e);
            }
            finally
            {
                process.Close();
                process.Dispose();
                process = null;
            }

            return messages;
        }
    }
}
