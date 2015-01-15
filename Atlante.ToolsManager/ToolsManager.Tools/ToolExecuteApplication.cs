using Atlante.Common;
using Atlante.Mef.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace ToolsManager.Tools
{
    [Export(typeof(ITool))]
    [ExportMetadata("ToolName", "ExecuteApplication")]
    public class ToolExecuteApplication : ToolBase, ITool
    {
        private const string TOOL_NAME = "ExecuteApplication";
        private const string TOOL_DESCRIPTION = "Execute Application";

        public string ApplicationPath { get; set; }
        public string ApplicationParameters { get; set; }
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

            parameters.Add(new ParameterBase(() => this.ApplicationPath, string.Empty, ParameterType.FilePath));
            parameters.Add(new ParameterBase(() => this.ApplicationParameters, string.Empty, ParameterType.String));
            parameters.Add(new ParameterBase(() => this.IsAsynchronous, "true", ParameterType.Boolean));

            return parameters;
        }

        public void SetParameters(Parameters parameters)
        {
            this.ApplicationPath = parameters.GetValue<string>(() => this.ApplicationPath);
            this.ApplicationParameters = parameters.GetValue<string>(() => this.ApplicationParameters);
            this.IsAsynchronous = parameters.GetValue<bool>(() => this.IsAsynchronous);
        }

        public override IMessages Run()
        {
            return SystemUtilities.StartProcess(this.ApplicationPath, this.ApplicationParameters, 1000);
        }
    }
}
