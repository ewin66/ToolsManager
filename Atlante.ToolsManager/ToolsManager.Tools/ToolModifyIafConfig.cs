using Atlante.Common;
using Atlante.Mef.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace ToolsManager.Tools
{
    [Export(typeof(ITool))]
    [ExportMetadata("ToolName", "ModifyIafConfig")]
    public class ToolModifyIafConfig : ToolBase, ITool
    {
        private const string TOOL_NAME = "ModifyIafConfig";
        private const string TOOL_DESCRIPTION = "Modify IafConfig";

        public string FileName { get; set; }
        public string EnvironmentId { get; set; }
        public string UseGridsByDefault { get; set; }
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

            parameters.Add(new ParameterBase(() => this.FileName, string.Empty, ParameterType.FilePath));
            parameters.Add(new ParameterBase(() => this.EnvironmentId, string.Empty, ParameterType.String));
            parameters.Add(new ParameterBase(() => this.UseGridsByDefault, string.Empty, ParameterType.String));
            parameters.Add(new ParameterBase(() => this.IsAsynchronous, "true", ParameterType.Boolean));

            return parameters;
        }

        public void SetParameters(Parameters parameters)
        {
            this.FileName = parameters.GetValue<string>(() => this.FileName);
            this.EnvironmentId = parameters.GetValue<string>(() => this.EnvironmentId);            
            this.UseGridsByDefault = parameters.GetValue<string>(() => this.UseGridsByDefault);
            this.IsAsynchronous = parameters.GetValue<bool>(() => this.IsAsynchronous);
        }

        public override IMessages Run()
        {
            Messages messages = new Messages();

            try
            {
                dynamic parser = new XmlParser(this.FileName);

                var environments = parser.Environments.Environment as IEnumerable<XmlParser>;

                dynamic environment = parser.Environments.Environment;

                while (environment.HasNext())
                {
                    if (environment["id"] != this.EnvironmentId)
                        environment.MoveNext();
                    else
                        break;
                }

                environment.IafDesktop.Create("Rendering", false).Create("UseGridsByDefault", this.UseGridsByDefault, false);

                parser.Save();
            }
            catch (Exception e)
            {
                messages.AddException(e);
            }

            return messages;
        }
    }
}
