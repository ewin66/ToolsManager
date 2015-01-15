using log4net;
using System;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

namespace Atlante.Common
{
    public static class Logger
    {
        private static ILog Trace { get; set; }

        public static ObservableCollection<Message> Errors { get; private set; }
        public static ObservableCollection<Message> Warnings { get; private set; }
        public static ObservableCollection<Message> Information { get; private set; }

        static Logger()
        {
            Logger.Errors = new ObservableCollection<Message>();
            Logger.Warnings = new ObservableCollection<Message>();
            Logger.Information = new ObservableCollection<Message>();

            Trace = LogManager.GetLogger("AtlanteLog");
            log4net.Config.XmlConfigurator.Configure();
        }

        public static void AddMessages(IMessages messages)
        {
            foreach (Message message in messages)
            {
                switch (message.Type)
                {
                    case MessageType.error:
                        Errors.Add(message);
                        break;
                    case MessageType.warning:
                        Warnings.Add(message);
                        break;
                    case MessageType.information:
                        Information.Add(message);
                        break;
                }
            }
        }

        public static void AddException(Exception e)
        {
            string message;
            if (e.InnerException == null)
                message = e.Message;
            else
                message = string.Format("{0}. {1}", e.Message, e.InnerException.Message);

            Logger.Errors.Add(new Message() { Type = MessageType.error, Description = message, Exception = e.GetType().ToString(), Trace = e.StackTrace });

            Trace.Error(message, e);
        }

        public static void AddWarning(string description)
        {
            Logger.Warnings.Add(new Message() { Type = MessageType.warning, Description = description });

            Trace.Warn(description);
        }

        public static void AddInformation(string description)
        {
            Logger.Information.Add(new Message() { Type = MessageType.information, Description = description });

            Trace.Info(description);
        }

        public static void AddTrace(string description, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            Trace.Info(string.Format("{0} | Method: {1} | Line: {2} | {3}", description, memberName, sourceLineNumber, sourceFilePath));
        }
    }
}
