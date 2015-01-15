using Atlante.Common;
using Atlante.Mef.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Xml.Linq;

namespace ToolsManager.Tools
{
    [Export(typeof(ITool))]
    [ExportMetadata("ToolName", "OpenDml")]
    public class ToolOpenDml : ToolBase, ITool
    {
        private const string TOOL_NAME = "OpenDml";
        private const string TOOL_DESCRIPTION = "Open Dml";

        public string DmlPath { get; set; }
        public string IafPath { get; set; }
        public string Enviroment { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
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

            parameters.Add(new ParameterBase(() => this.DmlPath, string.Empty, ParameterType.FilePath));
            parameters.Add(new ParameterBase(() => this.IafPath, string.Empty, ParameterType.FilePath));
            parameters.Add(new ParameterBase(() => this.Enviroment, string.Empty, ParameterType.String));
            parameters.Add(new ParameterBase(() => this.Username, string.Empty, ParameterType.String));
            parameters.Add(new ParameterBase(() => this.Password, string.Empty, ParameterType.Password));
            parameters.Add(new ParameterBase(() => this.IsAsynchronous, "true", ParameterType.Boolean));

            return parameters;
        }

        public void SetParameters(Parameters parameters)
        {
            this.DmlPath = parameters.GetValue<string>(() => this.DmlPath);
            this.IafPath = parameters.GetValue<string>(() => this.IafPath);
            this.Enviroment = parameters.GetValue<string>(() => this.Enviroment);
            this.Username = parameters.GetValue<string>(() => this.Username);
            this.Password = parameters.GetValue<string>(() => this.Password);
            this.IsAsynchronous = parameters.GetValue<bool>(() => this.IsAsynchronous);
        }

        public override IMessages Run()
        {
            string parameters = string.Format(" -e:{0} -u:{1} -p:{2} -i:{3} -d:{4} ", this.Enviroment, this.Username, this.Password, this.IafPath, this.DmlPath);

            SystemUtilities.StartProcess("Tools.DmlRunner.exe", parameters, 1000);

            return this.ReadMessages("Tools.DmlRunnerLog.xml");
        }

        private Messages ReadMessages(string messagesFile)
        {
            Messages messages = new Messages();

            if (!File.Exists(messagesFile))
            {
                messages.AddWarning(string.Format("The report file {0} doesn't exist", messagesFile));
                return messages;
            }

            var parser = XElement.Load(messagesFile);

            foreach (var message in parser.Descendants("anyType"))
            {
                var resultState = message.Attribute("Type").Value;

                if (resultState != "error")
                    continue;

                var description = message.Attribute("Description").Value;

                messages.AddException(new Exception(description));
            }

            return messages;
        }
    }
}
