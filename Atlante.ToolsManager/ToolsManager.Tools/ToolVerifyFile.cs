using Atlante.Common;
using Atlante.Mef.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace ToolsManager.Tools
{
    [Export(typeof(ITool))]
    [ExportMetadata("ToolName", "VerifyFile")]
    public class ToolVerifyFile : ToolBase, ITool
    {
        private const string TOOL_NAME = "VerifyFile";
        private const string TOOL_DESCRIPTION = "Verify File";

        public string FilePath { get; set; }
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

            parameters.Add(new ParameterBase(() => this.FilePath, string.Empty, ParameterType.FilePath));
            parameters.Add(new ParameterBase(() => this.IsAsynchronous, "true", ParameterType.Boolean));

            return parameters;
        }

        public void SetParameters(Parameters parameters)
        {
            this.FilePath = parameters.GetValue<string>(() => this.FilePath);
            this.IsAsynchronous = parameters.GetValue<bool>(() => this.IsAsynchronous);
        }

        public override IMessages Run()
        {
            return FileUtilities.FileExists(this.FilePath);
        }
    }
}
