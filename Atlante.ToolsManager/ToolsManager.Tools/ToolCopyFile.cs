using Atlante.Common;
using Atlante.Mef.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace ToolsManager.Tools
{
    [Export(typeof(ITool))]
    [ExportMetadata("ToolName", "CopyFile")]
    public class ToolCopyFile : ToolBase, ITool
    {
        private const string TOOL_NAME = "CopyFile";
        private const string TOOL_DESCRIPTION = "Copy File";

        public string SourcePath { get; set; }
        public string TargetPath { get; set; }
        public bool Overwrite { get; set; }
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
            parameters.Add(new ParameterBase(() => this.Overwrite, string.Empty, ParameterType.Boolean));
            parameters.Add(new ParameterBase(() => this.IsAsynchronous, "true", ParameterType.Boolean));

            return parameters;
        }

        public void SetParameters(Parameters parameters)
        {
            this.SourcePath = parameters.GetValue<string>(() => this.SourcePath);
            this.TargetPath = parameters.GetValue<string>(() => this.TargetPath);
            this.Overwrite = parameters.GetValue<bool>(() => this.Overwrite);
            this.IsAsynchronous = parameters.GetValue<bool>(() => this.IsAsynchronous);
        }

        public override IMessages Run()
        {
            return FileUtilities.CopyFile(this.SourcePath, this.TargetPath, this.Overwrite);
        }
    }
}
