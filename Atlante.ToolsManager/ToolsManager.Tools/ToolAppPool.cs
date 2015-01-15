using Atlante.Common;
using Atlante.Mef.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace ToolsManager.Tools
{
    [Export(typeof(ITool))]
    [ExportMetadata("ToolName", "AppPool")]
    public class ToolAppPool : ToolBase, ITool
    {
        private const string TOOL_NAME = "AppPool";
        private const string TOOL_DESCRIPTION = "App Pool";
        private const string PARAM_APPPOOL_ACTION = "AppPoolAction";

        public string AppPoolName { get; set; }
        public AppPoolAction AppPoolAction { get; set; }
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

            parameters.Add(new ParameterBase(() => this.AppPoolName, string.Empty, ParameterType.String));
            parameters.Add(new ParameterBase(PARAM_APPPOOL_ACTION, string.Empty, ParameterType.Collection, this.GetServiceOptions()));
            parameters.Add(new ParameterBase(() => this.IsAsynchronous, "true", ParameterType.Boolean));

            return parameters;
        }

        public void SetParameters(Parameters parameters)
        {
            this.AppPoolName = parameters.GetValue<string>(() => this.AppPoolName);
            this.AppPoolAction = (AppPoolAction)Enum.Parse(typeof(AppPoolAction), parameters[PARAM_APPPOOL_ACTION].Value);
            this.IsAsynchronous = parameters.GetValue<bool>(() => this.IsAsynchronous);
        }

        public override IMessages Run()
        {
            switch (this.AppPoolAction)
            {
                case AppPoolAction.Start:
                    return ServiceUtilities.StartAppPool(this.AppPoolName);
                case AppPoolAction.Stop:
                    return ServiceUtilities.StopAppPool(this.AppPoolName);
                case AppPoolAction.Recycle:
                    return ServiceUtilities.RecycleAppPool(this.AppPoolName);
                default:
                    return new Messages();
            }
        }

        private IList<string> GetServiceOptions()
        {
            var options = new List<string>();

            options.Add(AppPoolAction.Start.ToString());
            options.Add(AppPoolAction.Stop.ToString());
            options.Add(AppPoolAction.Recycle.ToString());

            return options;
        }
    }
}
