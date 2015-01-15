using Atlante.Common;
using Atlante.Mef.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Xml.Linq;
using System.Linq;
using System.Xml.XPath;

namespace ToolsManager.Tools
{
    [Export(typeof(ITool))]
    [ExportMetadata("ToolName", "VerifyConfig")]
    public class ToolVerifyConfig : ToolBase, ITool
    {
        private const string TOOL_NAME = "VerifyConfig";
        private const string TOOL_DESCRIPTION = "Verify Config";

        private string FileName { get; set; }
        private string XPath { get; set; }
        private string AttrName1 { get; set; }
        private string AttrValue1 { get; set; }
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
            parameters.Add(new ParameterBase(() => this.XPath, string.Empty, ParameterType.String));
            parameters.Add(new ParameterBase(() => this.AttrName1, string.Empty, ParameterType.String));
            parameters.Add(new ParameterBase(() => this.AttrValue1, string.Empty, ParameterType.String));
            parameters.Add(new ParameterBase(() => this.IsAsynchronous, "true", ParameterType.Boolean));

            return parameters;
        }

        public void SetParameters(Parameters parameters)
        {
            this.FileName = parameters.GetValue<string>(() => this.FileName);
            this.XPath = parameters.GetValue<string>(() => this.XPath);
            this.AttrName1 = parameters.GetValue<string>(() => this.AttrName1);
            this.AttrValue1 = parameters.GetValue<string>(() => this.AttrValue1);
            this.IsAsynchronous = parameters.GetValue<bool>(() => this.IsAsynchronous);
        }

        public override IMessages Run()
        {
            Messages messages = new Messages();

            try
            {
                if (!File.Exists(this.FileName))
                {
                    messages.AddWarning(string.Format("The file {0} doesn't exist.", this.FileName));
                    return messages;
                }

                XDocument doc = XDocument.Load(this.FileName);

                var parentElement = doc.XPathSelectElement(XPath);
                if (parentElement == null)
                {
                    messages.AddWarning(string.Format("The path '{0}' doesn't exist into the file.", this.XPath));
                    return messages;
                }

                var values = parentElement.Descendants().Select(x => x.Attribute(this.AttrName1).Value);
                if (values == null)
                {
                    messages.AddWarning(string.Format("The attribute '{0}' doesn't exist into the file.", this.AttrName1));
                    return messages;
                }

                foreach (var value in values)
                {
                    if (value == this.AttrValue1)
                        return messages;                        
                }

                messages.AddWarning(string.Format("The attribute {0} doesn't exist.", this.AttrValue1));
            }
            catch (Exception e)
            {
                messages.AddException(e);
            }

            return messages;
        }
    }
}
