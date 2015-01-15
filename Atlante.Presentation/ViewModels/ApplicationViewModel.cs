using Atlante.Common;
using Atlante.Presentation.Extensions;
using Atlante.Presentation.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Media;

namespace Atlante.Presentation.ViewModels
{
    public class ApplicationViewModel : IStatusInfo, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private static ApplicationViewModel _statusbar;

        private ActionType _action;
        private bool _actionInProgress;
        private string _actionDescription;

        private string _addInDescription;
        private string _applicationDescription;

        public Stopwatch Watch { get; private set; }

        public string ApplicationName { get; private set; }
        public string ApplicationVersion { get; private set; }
        public string ApplicationCopyright { get; private set; }

        public ImageSource AppIcon { get; private set; }

        public IList<AppLibrary> AppLibraries { get; private set; }

        public string ApplicationDescription
        {
            get { return _applicationDescription; }
            set
            {
                if (_applicationDescription == value)
                    return;

                _applicationDescription = value;

                this.NotifyPropertyChanged("ApplicationDescription");
            }
        }

        public ActionType Action
        {
            get { return _action; }
            set
            {
                _action = value;

                this.NotifyPropertyChanged("Action");
            }
        }

        public string ActionDescription
        {
            get { return _actionDescription; }
            set
            {
                _actionDescription = value;

                this.NotifyPropertyChanged("ActionDescription");
            }
        }

        public bool ActionInProgress
        {
            get { return _actionInProgress; }
            set
            {
                _actionInProgress = value;

                this.NotifyPropertyChanged("ActionInProgress");
            }
        }

        private ApplicationViewModel()
        {
            Logger.AddTrace("Configuring Application Model");

            this.Action = ActionType.None;
            this.ActionInProgress = false;
            this.ActionDescription = string.Empty;

            this.ApplicationName = SystemUtilities.GetProductName(Assembly.GetEntryAssembly());
            this.ApplicationVersion = SystemUtilities.GetVersionNumber(Assembly.GetEntryAssembly());
            this.ApplicationCopyright = SystemUtilities.GetProductCopyright(Assembly.GetEntryAssembly());

            this.AppIcon = SystemUtilities.GetApplicationIcon(Assembly.GetEntryAssembly());

            this.AppLibraries = SystemUtilities.GetApplicationLibraries(Assembly.GetEntryAssembly());

            this.UpdateApplicationDescription();
        }

        public void SetAddInDescription(string addInDescription)
        {
            Logger.AddTrace("Setting AddIn Description");

            _addInDescription = addInDescription;

            this.UpdateApplicationDescription();
        }

        private void UpdateApplicationDescription()
        {
            Logger.AddTrace("Updating Application Description");

            this.ApplicationDescription = string.Format("{0} {1}", this.ApplicationName, this.ApplicationVersion);

            if (!string.IsNullOrEmpty(_addInDescription))
                this.ApplicationDescription = string.Format("{0} - {1}", this.ApplicationDescription, _addInDescription);
        }

        public static ApplicationViewModel Create()
        {
            Logger.AddTrace("Creating Application Model");

            if (_statusbar == null)
                _statusbar = new ApplicationViewModel();

            return _statusbar;
        }

        public static void BeginAction(ActionType action)
        {
            Logger.AddTrace("Begin Action");

            _statusbar.Watch = new Stopwatch();
            _statusbar.Watch.Start();

            _statusbar.Action = action;
            _statusbar.ActionInProgress = true;
            _statusbar.ActionDescription = Translator.Translate(action.GetDescription());
        }

        public static void EndAction(ActionType action)
        {
            Logger.AddTrace("Ending Action");

            _statusbar.Watch.Stop();

            _statusbar.Action = action;
            _statusbar.ActionInProgress = false;
            _statusbar.ActionDescription = string.Format("{0 } Successful: {1} seconds", Translator.Translate(action.GetDescription()), String.Format("{0:0.0000}", TimeSpan.FromTicks(_statusbar.Watch.ElapsedTicks).TotalSeconds));
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
