using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Atlante.Common
{
    public static class FileUtilities
    {
        public static bool ForceCreateDirectory(string path)
        {
            if (string.IsNullOrEmpty(path))
                return false;

            if (!Path.IsPathRooted(path))
                path = Path.Combine(Directory.GetCurrentDirectory(), path);

            if (!string.IsNullOrEmpty(Path.GetExtension(path)))
                path = Path.GetDirectoryName(path);

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            return true;
        }

        public static void ForceDeleteFile(string path)
        {
            if (File.Exists(path))
                File.Delete(path);
        }

		public static IMessages FileExists(string filePath)
		{
			Messages messages = new Messages();

			if (!File.Exists(filePath))
				messages.AddWarning(string.Format("The file path {0} doesn't exist.", filePath));

			return messages;
		}

        public static IMessages CopyFile(string sourcePath, string targetPath, bool overwrite)
        {
            Messages messages = new Messages();

            if (!File.Exists(sourcePath))
            {
                messages.AddWarning(string.Format("The file {0} can't be copy because doesn't exist", sourcePath));
                return messages;
            }

            try
            {
                if (Path.GetExtension(targetPath) == string.Empty)
                    targetPath = Path.Combine(targetPath, Path.GetFileName(sourcePath));

                if (File.Exists(targetPath) && overwrite)
                    File.SetAttributes(targetPath, FileAttributes.Normal);

                File.Copy(sourcePath, targetPath, overwrite);
            }
            catch (Exception e)
            {
                messages.AddException(e);
            }

            return messages;
        }

        public static IMessages CopyFiles(string sourcePath, string targetPath, string[] files, bool overwrite)
        {
            Messages messages = new Messages();

            foreach (var sourceFilePath in files)
            {
                var targetFilePath = Path.Combine(targetPath, GetRelativePath(sourcePath, sourceFilePath));

                if (!Directory.Exists(Path.GetDirectoryName(targetFilePath)))
                    Directory.CreateDirectory(Path.GetDirectoryName(targetFilePath));

                try
                {
                    if (File.Exists(targetFilePath) && overwrite)
                        File.SetAttributes(targetFilePath, FileAttributes.Normal);

                    File.Copy(sourceFilePath, targetFilePath, overwrite);
                }
                catch (UnauthorizedAccessException e)
                {
                    messages.AddException(e, "Unauthorized to copy file after change readonly attribute");
                }
            }

            return messages;
        }

        public static IMessages CopyDirectory(string sourcePath, string targetPath, bool overwrite)
        {
            return CopyDirectory(sourcePath, targetPath, string.Empty, overwrite, false);
        }

        public static IMessages CopyDirectory(string sourcePath, string targetPath, string filters, bool overwrite, bool recursive)
        {
            Messages messages = new Messages();

            if (!Directory.Exists(sourcePath))
            {
                messages.AddWarning(string.Format("The directory {0} can't be copy because doesn't exist", sourcePath));
                return messages;
            }

            try
            {
                if (!Directory.Exists(targetPath))
                    Directory.CreateDirectory(targetPath);

                string[] filteredFiles = null;
                if (string.IsNullOrEmpty(filters))
                    filteredFiles = Directory.GetFiles(sourcePath, string.Empty, SearchOption.AllDirectories);
                else
                    filteredFiles = filters.Split(' ').SelectMany(filter => Directory.GetFiles(sourcePath, filter, SearchOption.AllDirectories)).ToArray();

                var copyMessages = CopyFiles(sourcePath, targetPath, filteredFiles, overwrite);

                messages.AddArray(messages);
            }
            catch (Exception e)
            {
                messages.AddException(e);
            }

            return messages;
        }

        public static void SaveFile(Stream stream, string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return;

            string directory = Path.GetDirectoryName(fileName);

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            try
            {
                const int bufferSize = 2048;
                var buffer = new byte[bufferSize];

                using (FileStream outStream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                {
                    int bytesRead = stream.Read(buffer, 0, bufferSize);

                    while (bytesRead > 0)
                    {
                        outStream.Write(buffer, 0, bytesRead);
                        bytesRead = stream.Read(buffer, 0, bufferSize);
                    }
                }
            }
            catch (Exception e)
            {
                Logger.AddException(e);
            }
        }

        public static string GetAbsolutePath(string path)
        {
            if (Path.IsPathRooted(path))
                return path;

            return Path.Combine(Directory.GetCurrentDirectory(), path);
        }

        public static string GetRelativePath(string basePath, string fullPath)
        {
            return fullPath.Replace(basePath, string.Empty);
        }

        public static string GetImageUriPath(string imageName)
        {
            return string.Format(@"pack://application:,,,/{0};component/Images/{1}", Assembly.GetCallingAssembly().GetName().Name, imageName);
        }

        public static Version GetVersion(FileInfo fileInfo)
        {

            Regex RegularExpressions = new Regex(@"\d{1,4}\.\d{1,4}\.\d{1,4}\.\d{1,4}", RegexOptions.IgnoreCase);

            Version v = null;

            MatchCollection matchCollection = RegularExpressions.Matches(fileInfo.Name);

            foreach (Match match in matchCollection)
                v = new Version(match.Value);

            if (v != null)
                return v;

            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(fileInfo.FullName);

            string ProductVersion = fvi.ProductVersion == null ? "" : fvi.ProductVersion;

            if (RegularExpressions.IsMatch(ProductVersion))
                return (new Version(ProductVersion));

            return null;
        }
    }
}
