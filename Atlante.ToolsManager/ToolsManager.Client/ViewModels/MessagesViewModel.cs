using Atlante.Common;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using ToolsManager.Client.Views;

namespace ToolsManager.Client.ViewModels
{
    public class MessagesViewModel : BaseViewModel, INotifyPropertyChanged
    {
        private ObservableCollection<Message> _messages;

        public event PropertyChangedEventHandler PropertyChanged;

        public MessageType SelectedMessageType { get; set; }

        public Command ViewTextCommand { get; set; }
        public Command ClearMessagesCommand { get; set; }
        public Command SelectedMessageTypeCommand { get; set; }

        public ObservableCollection<Message> Messages { get { return _messages; } }

        public override string Title
        {
            get { return Translator.Translate("UI_MESSAGE_LIST"); }
        }

        public int MessageCount
        {
            get { return Logger.Errors.Count + Logger.Warnings.Count + Logger.Information.Count; }
        }

        public MessagesViewModel()
        {
            this.UpdateMessages();
            this.ConfigureCommands();

            Logger.Errors.CollectionChanged += new NotifyCollectionChangedEventHandler(Messages_CollectionChanged);
            Logger.Warnings.CollectionChanged += new NotifyCollectionChangedEventHandler(Messages_CollectionChanged);
            Logger.Information.CollectionChanged += new NotifyCollectionChangedEventHandler(Messages_CollectionChanged);
        }

        private void Messages_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.UpdateMessages();
        }

        private void ConfigureCommands()
        {
            this.ViewTextCommand = new Command("View", "View text", "viewer24x24.png", this.ExecuteViewText);
            this.ClearMessagesCommand = new Command("Clear", "Clear Messages", "clearMessages.png", this.ExecuteClearMessages, this.CanExecuteClearMessages);
            this.SelectedMessageTypeCommand = new Command("SelectedMessageType", this.ExecuteSelectedMessageType);
        }

        private void UpdateMessages()
        {
            if (_messages == null)
                _messages = new ObservableCollection<Message>();

            _messages.Clear();

            switch (this.SelectedMessageType)
            {
                case MessageType.error:
                    foreach (var message in Logger.Errors)
                        _messages.Add(message);
                    break;
                case MessageType.warning:
                    foreach (var message in Logger.Warnings)
                        _messages.Add(message);
                    break;
                case MessageType.information:
                    foreach (var message in Logger.Information)
                        _messages.Add(message);
                    break;
            }

            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs("MessageCount"));
        }

        private bool CanExecuteClearMessages(object parameter)
        {
            switch (SelectedMessageType)
            {
                case MessageType.error:
                    return Logger.Errors.Count > 0;
                case MessageType.warning:
                    return Logger.Warnings.Count > 0;
                case MessageType.information:
                    return Logger.Information.Count > 0;
                default:
                    return false;
            }
        }

        private void ExecuteClearMessages(object parameter)
        {
            switch (SelectedMessageType)
            {
                case MessageType.error:
                    Logger.Errors.Clear();
                    break;
                case MessageType.warning:
                    Logger.Warnings.Clear();
                    break;
                case MessageType.information:
                    Logger.Information.Clear();
                    break;
            }
        }

        private void ExecuteSelectedMessageType(object parameter)
        {
            this.SelectedMessageType = (MessageType)int.Parse(parameter.ToString());

            this.UpdateMessages();
        }

        public void ExecuteViewText(object parameter)
        {
            if (parameter == null)
                return;

            var view = new TextVisualizerView(parameter.ToString());

            base.ShowDialog(Translator.Translate("UI_TEXT_VISUALIZER"), view);
        }
    }
}
