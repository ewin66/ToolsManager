using Atlante.Common;
using Atlante.Mef.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;

namespace ToolsManager.Tools
{
    [Export(typeof(ITool))]
    [ExportMetadata("ToolName", "OpenFile")]
    public class ToolOpenFile : ToolBase, ITool
    {
        private const string TOOL_NAME = "OpenFile";
        private const string TOOL_DESCRIPTION = "Open File";

        public string FileName { get; set; }
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
            get { return true; }
        }

        public IEnumerable<ParameterBase> GetParametersDefinition()
        {
            var parameters = new List<ParameterBase>();

            parameters.Add(new ParameterBase(() => this.FileName, string.Empty, ParameterType.String));
            parameters.Add(new ParameterBase(() => this.IsAsynchronous, "true", ParameterType.Boolean));

            return parameters;
        }

        public void SetParameters(Parameters parameters)
        {
            this.FileName = parameters.GetValue<string>(() => this.FileName);
            this.IsAsynchronous = parameters.GetValue<bool>(() => this.IsAsynchronous);
        }

        public override IMessages Run()
        {
            Messages messages = new Messages();
            if (!File.Exists(this.FileName))
            {
                messages.AddWarning(string.Format("File {0} does not exists", this.FileName));
                return messages;
            }

            return SystemUtilities.ExecuteShellCommand(this.FileName);
        }
    }
}
