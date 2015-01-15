using Atlante.Common;
using Atlante.Mef.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace ToolsManager.Tools
{
    [Export(typeof(ITool))]
    [ExportMetadata("ToolName", "CopyDirectory")]
    public class ToolCopyDirectory : ToolBase, ITool
    {
        private const string TOOL_NAME = "CopyDirectory";
        private const string TOOL_DESCRIPTION = "Copy Directory";

        public string SourcePath { get; set; }
        public string TargetPath { get; set; }
        public string Filters { get; set; }
        public bool Overwrite { get; set; }
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

            parameters.Add(new ParameterBase(() => this.SourcePath, string.Empty, ParameterType.FilePath));
            parameters.Add(new ParameterBase(() => this.TargetPath, string.Empty, ParameterType.FilePath));
            parameters.Add(new ParameterBase(() => this.Filters, string.Empty, ParameterType.String));
            parameters.Add(new ParameterBase(() => this.Overwrite, string.Empty, ParameterType.Boolean));
            parameters.Add(new ParameterBase(() => this.Recursive, string.Empty, ParameterType.Boolean));
            parameters.Add(new ParameterBase(() => this.IsAsynchronous, "true", ParameterType.Boolean));

            return parameters;
        }

        public void SetParameters(Parameters parameters)
        {
            this.SourcePath = parameters.GetValue<string>(() => this.SourcePath);
            this.TargetPath = parameters.GetValue<string>(() => this.TargetPath);
            this.Filters = parameters.GetValue<string>(() => this.Filters);
            this.Overwrite = parameters.GetValue<bool>(() => this.Overwrite);
            this.Recursive = parameters.GetValue<bool>(() => this.Recursive);
            this.IsAsynchronous = parameters.GetValue<bool>(() => this.IsAsynchronous);
        }

        public override IMessages Run()
        {
            return FileUtilities.CopyDirectory(this.SourcePath, this.TargetPath, this.Filters, this.Overwrite, this.Recursive);
        }
    }
}
