using Atlante.Common;
using Atlante.Mef.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text;

namespace ToolsManager.Tools
{
    [Export(typeof(ITool))]
    [ExportMetadata("ToolName", "MergeFiles")]
    public class ToolMergeFiles : ToolBase, ITool
    {
        private const string TOOL_NAME = "MergeFiles";
        private const string TOOL_DESCRIPTION = "Merge Files";
        private const string PARAM_APPLICATION_NAME = "Merge Application";

        private const string APP_WINMERGE = "WinMerge";
        private const string APP_SLICKDIFF = "Slick Diff";

        public string ApplicationName { get; set; }
        public string Path1 { get; set; }
        public string Path2 { get; set; }
        public string FilterExclude { get; set; }
        public string FilterInclude { get; set; }
        public bool Recursive { get; set; }
        public bool IsAsynchronous { get; set; }

        public string ToolName
        {
            get { return TOOL_NAME; }
        }

        public string ToolDescription
        {
            get { return TOOL_DESCRIPTION; }
        }

        public bool CanShareFiles
        {
            get { return false; }
        }

        public IEnumerable<ParameterBase> GetParametersDefinition()
        {
            var parameters = new List<ParameterBase>();

            parameters.Add(new ParameterBase(PARAM_APPLICATION_NAME, string.Empty, ParameterType.Collection, this.GetApplicationOptions()));
            parameters.Add(new ParameterBase(() => this.Path1, string.Empty, ParameterType.FilePath));
            parameters.Add(new ParameterBase(() => this.Path2, string.Empty, ParameterType.FilePath));
            parameters.Add(new ParameterBase(() => this.FilterExclude, string.Empty, ParameterType.String));
            parameters.Add(new ParameterBase(() => this.FilterInclude, string.Empty, ParameterType.String));
            parameters.Add(new ParameterBase(() => this.Recursive, string.Empty, ParameterType.Boolean));
            parameters.Add(new ParameterBase(() => this.IsAsynchronous, "true", ParameterType.Boolean));

            return parameters;
        }

        public void SetParameters(Parameters parameters)
        {
            this.ApplicationName = parameters[PARAM_APPLICATION_NAME].Value;
            this.Path1 = parameters.GetValue<string>(() => this.Path1);
            this.Path2 = parameters.GetValue<string>(() => this.Path2);
            this.FilterExclude = parameters.GetValue<string>(() => this.FilterExclude);
            this.FilterInclude = parameters.GetValue<string>(() => this.FilterInclude);
            this.Recursive = parameters.GetValue<bool>(() => this.Recursive);
            this.IsAsynchronous = parameters.GetValue<bool>(() => this.IsAsynchronous);
        }

        public override IMessages Run()
        {
            return SystemUtilities.StartProcess(this.GetApplicationPath(), this.GetApplicationParameters(), 1000);
        }

        private string GetApplicationPath()
        {
            switch (this.ApplicationName)
            {
                case APP_SLICKDIFF:
                    var vsPath = RegistryUtilities.ReadDefaultValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\vs.exe");
                    return vsPath.Replace("vs.exe", "vsdiff.exe");
                case APP_WINMERGE:
                    return RegistryUtilities.ReadDefaultValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\WinMerge.exe");
                default:
                    return string.Empty;
            }
        }

        private string GetApplicationParameters()
        {
            switch (this.ApplicationName)
            {
                case APP_SLICKDIFF:
                    return this.GetSlickDiffParameters();
                case APP_WINMERGE:
                    return this.GetWinMergeParameters();
                default:
                    return string.Empty;
            }
        }

        private string GetSlickDiffParameters()
        {
            StringBuilder builder = new StringBuilder();

            if (this.Recursive)
                builder.Append("-t ");

            if (!string.IsNullOrEmpty(this.FilterExclude))
                builder.Append(string.Format("-excludefilespec \"{0}\" ", this.FilterExclude));

            builder.Append(string.Format("\"{0}\" \"{1}\"", this.Path1, this.Path2));

            return builder.ToString();
        }

        private string GetWinMergeParameters()
        {
            StringBuilder builder = new StringBuilder();

            if (this.Recursive)
                builder.Append("/r ");

            if (!string.IsNullOrEmpty(this.FilterInclude))
                builder.Append(string.Format("/f \"{0}\" ", this.FilterInclude));

            builder.Append(string.Format("\"{0}\" \"{1}\"", this.Path1, this.Path2));

            return builder.ToString();
        }

        private IList<string> GetApplicationOptions()
        {
            var options = new List<string>();

            options.Add(APP_WINMERGE);
            options.Add(APP_SLICKDIFF);

            return options;
        }
    }
}
