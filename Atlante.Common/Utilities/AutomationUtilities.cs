using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WatiN.Core;

namespace Atlante.Common
{
    public static class AutomationUtilities
    {
        public static IMessages OpenUrl(string url, string script)
        {
            Messages messages = new Messages();

            if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(script))
                return messages;

            var browser = new IE(url);

            var commands = script.Split(';');
            foreach (var command in commands)
            {
                if (string.IsNullOrEmpty(command))
                    continue;

                Regex regex = new Regex(@"^\b(id=)(?<id>.*)\,\b(action=)(?<action>.*)\,\b(value=)(?<value>.*)$");
                Match parameters = regex.Matches(command)[0];

                var id = parameters.Groups["id"].Value;
                var action = parameters.Groups["action"].Value;
                var value = parameters.Groups["value"].Value;

                var message = AutomateAction(browser, id, action, value);

                if (message != null)
                    messages.Add(message);
            }

            return messages;
        }

        private static Message AutomateAction(Browser browser, string id, string action, string value)
        {
            switch (action)
            {
                case "settext":
                    return SetText(browser, id, value);
                case "click":
                    return Click(browser, id);
                default:
                    return new Message() { Type = MessageType.warning, Description = string.Format("Action {0} cannot be recognized", action) };
            }
        }

        private static Message SetText(Browser browser, string id, string value)
        {
            var textField = browser.TextField(Find.ById(id));
            if (textField == null)
                return new Message() { Type = MessageType.warning, Description = string.Format("Id {0} cannot be recognized", id) };

            textField.TypeText(value);

            return null;
        }

        private static Message Click(Browser browser, string id)
        {
            var button = browser.Button(Find.ById(id));
            if (button == null)
                return new Message() { Type = MessageType.warning, Description = string.Format("Id {0} cannot be recognized", id) };

            // set focus before click
            browser.NativeDocument.Body.SetFocus();

            button.Click();

            return null;
        }
    }
}
