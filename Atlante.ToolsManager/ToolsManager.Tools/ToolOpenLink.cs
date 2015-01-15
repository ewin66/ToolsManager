using Atlante.Common;
using Atlante.Mef.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;

namespace ToolsManager.Tools
{
    [Export(typeof(ITool))]
    [ExportMetadata("ToolName", "OpenLink")]
    public class ToolOpenLink : ToolBase, ITool
    {
        private const string TOOL_NAME = "OpenLink";
        private const string TOOL_DESCRIPTION = "Open Link";

        public string Url { get; set; }
        public string Script { get; set; }
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

            parameters.Add(new ParameterBase(() => this.Url, string.Empty, ParameterType.String));
            parameters.Add(new ParameterBase(() => this.Script, string.Empty, ParameterType.String));
            parameters.Add(new ParameterBase(() => this.IsAsynchronous, "false", ParameterType.Boolean));

            return parameters;
        }

        public void SetParameters(Parameters parameters)
        {
            this.Url = parameters.GetValue<string>(() => this.Url);
            this.Script = parameters.GetValue<string>(() => this.Script);
            this.IsAsynchronous = parameters.GetValue<bool>(() => this.IsAsynchronous);
        }

        public override IMessages Run()
        {
            Messages messages = new Messages();

            try
            {
                if (!string.IsNullOrEmpty(this.Script))
                    return AutomationUtilities.OpenUrl(this.Url, this.Script);
                else
                    Process.Start(this.Url);
            }
            catch (Exception e)
            {
                messages.AddException(e);
            }

            return messages;
        }
    }
}
