using Atlante.Common;
using Atlante.Mef.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Text;
using System.Xml.Linq;

namespace ToolsManager.Tools
{
    [Export(typeof(ITool))]
    [ExportMetadata("ToolName", "RunTester")]
    public class ToolRunTester : ToolBase, ITool
    {
        private const string TOOL_NAME = "RunTester";
        private const string TOOL_DESCRIPTION = "Run Tester";

        public string AssemblyPath { get; private set; }
        public string OutputPath { get; private set; }
        public bool HideWindow { get; private set; }
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

            parameters.Add(new ParameterBase(() => this.AssemblyPath, string.Empty, ParameterType.FilePath));
            parameters.Add(new ParameterBase(() => this.OutputPath, string.Empty, ParameterType.FilePath));
            parameters.Add(new ParameterBase(() => this.HideWindow, string.Empty, ParameterType.Boolean));
            parameters.Add(new ParameterBase(() => this.IsAsynchronous, "true", ParameterType.Boolean));

            return parameters;
        }

        public void SetParameters(Parameters parameters)
        {
            this.AssemblyPath = parameters.GetValue<string>(() => this.AssemblyPath);
            this.OutputPath = parameters.GetValue<string>(() => this.OutputPath);
            this.HideWindow = parameters.GetValue<bool>(() => this.HideWindow);
            this.IsAsynchronous = parameters.GetValue<bool>(() => this.IsAsynchronous);
        }

        public override IMessages Run()
        {
            Messages messages = new Messages();

            string fileName = Path.Combine(Directory.GetCurrentDirectory(), this.AssemblyPath);

            if (!File.Exists(fileName))
            {
                messages.AddWarning(string.Format("The tester assembly {0} doesn't exist.", fileName));
                return messages;
            }

            try
            {
                var nunitConsolePath = Path.Combine(RegistryUtilities.ReadDefaultValue(@"HKEY_CURRENT_USER\SOFTWARE\nunit.org\NUnit\2.5.7", "InstallDir"), @"bin\net-2.0\nunit-console.exe");

                if (!File.Exists(nunitConsolePath))
                {
                    messages.AddWarning(string.Format("The nunit console path {0} doesn't exist.", nunitConsolePath));
                    return messages;
                }

                SystemUtilities.StartProcess(nunitConsolePath, string.Format("{0} /xml:{1}", fileName, this.OutputPath, this.HideWindow), 1000);

                messages = this.ReadMessages(this.OutputPath);
            }
            catch (Exception e)
            {
                messages.AddException(e);
            }

            return messages;
        }

        private Messages ReadMessages(string messagesFile)
        {
            var messages = new Messages();
            var parser = XElement.Load(messagesFile);

            int succeedCount = 0;
            int failedCount = 0;

            StringBuilder succeedMessages = new StringBuilder();
            StringBuilder failedMessages = new StringBuilder();

            succeedMessages.AppendLine("Succed tests for: " + this.AssemblyPath);
            failedMessages.AppendLine("Failed tests for: " + this.AssemblyPath);

            foreach (var testCase in parser.Descendants("test-case"))
            {
                var resultState = testCase.Attribute("result").Value;
                var name = testCase.Attribute("name").Value;

                if (resultState == "Success")
                {
                    succeedCount++;
                    succeedMessages.AppendLine(string.Format("\nTest Success: {0}", name));

                    continue;
                }

                var message = testCase.Element("failure").Element("message").Value;
                var stackTrace = testCase.Element("failure").Element("stack-trace").Value;

                failedCount++;
                failedMessages.AppendLine(string.Format("\nTest Fail: {0} \nMessage: {1} \nStack-Trace:{2} ", name, message, stackTrace));
            }

            if (succeedCount > 0)
            {
                succeedMessages.AppendLine(string.Format("\nSucceed tests: {0}", succeedCount));
                messages.AddInformation(succeedMessages.ToString());
            }

            if (failedCount > 0)
            {
                failedMessages.AppendLine(string.Format("\nFailed tests: {0}", failedCount));
                messages.AddWarning(failedMessages.ToString());
            }

            return messages;
        }
    }
}
