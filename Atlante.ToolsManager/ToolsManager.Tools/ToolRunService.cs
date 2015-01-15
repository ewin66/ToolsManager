using Atlante.Common;
using Atlante.Mef.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace ToolsManager.Tools
{
    [Export(typeof(ITool))]
    [ExportMetadata("ToolName", "RunService")]
    public class ToolRunService : ToolBase, ITool
    {
        private const string TOOL_NAME = "RunService";
        private const string TOOL_DESCRIPTION = "Run Service";
        private const string PARAM_SERVICE_ACTION = "ServiceAction";

        public string ServiceName { get; set; }
        public ServiceAction ServiceAction { get; set; }
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

            parameters.Add(new ParameterBase(() => this.ServiceName, string.Empty, ParameterType.String));
            parameters.Add(new ParameterBase(PARAM_SERVICE_ACTION, string.Empty, ParameterType.Collection, this.GetServiceOptions()));
            parameters.Add(new ParameterBase(() => this.IsAsynchronous, "true", ParameterType.Boolean));

            return parameters;
        }

        public void SetParameters(Parameters parameters)
        {
            this.ServiceName = parameters.GetValue<string>(() => this.ServiceName);
            this.ServiceAction = (ServiceAction)Enum.Parse(typeof(ServiceAction), parameters[PARAM_SERVICE_ACTION].Value);
            this.IsAsynchronous = parameters.GetValue<bool>(() => this.IsAsynchronous);
        }

        public override IMessages Run()
        {
            switch (this.ServiceAction)
            {
                case ServiceAction.Start:
                    return ServiceUtilities.StartService(this.ServiceName, 1000);
                case ServiceAction.Stop:
                    return ServiceUtilities.StopService(this.ServiceName, 1000);
                case ServiceAction.Restart:
                    return ServiceUtilities.RestartService(this.ServiceName, 1000);
                default:
                    return new Messages();
            }
        }

        private IList<string> GetServiceOptions()
        {
            var options = new List<string>();

            options.Add(ServiceAction.Start.ToString());
            options.Add(ServiceAction.Stop.ToString());
            options.Add(ServiceAction.Restart.ToString());

            return options;
        }
    }
}
