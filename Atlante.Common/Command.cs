﻿using System;
using System.ComponentModel;
using System.Windows.Input;

namespace Atlante.Common
{
    public class Command : CommandBase, ICommand, INotifyPropertyChanged
    {
        private string _displayName;
        private string _toolTip;
        private string _imageName;

        public event PropertyChangedEventHandler PropertyChanged;

        public Action<object> ExecuteMethod { get; set; }
        public Predicate<object> CanExecuteMethod { get; set; }

        public string DisplayName
        {
            get { return _displayName; }
            set
            {
                _displayName = value;
                this.NotifyPropertyChanged("DisplayName");
            }
        }

        public string ToolTip
        {
            get { return _toolTip; }
            set
            {
                _toolTip = value;
                this.NotifyPropertyChanged("ToolTip");
            }
        }

        public string ImageName
        {
            get { return _imageName; }
            set
            {
                _imageName = value;
                this.NotifyPropertyChanged("ImageName");
            }
        }

        public Command(string name, Action<object> executeMethod)
            : this(name, string.Empty, string.Empty, executeMethod, null)
        {
            //
        }

        public Command(string name, string toolTip, string image, Action<object> executeMethod)
            : this(name, toolTip, image, executeMethod, null)
        {
            //
        }

        public Command(string name, string toolTip, string image, Action<object> executeMethod, Predicate<object> canExecuteMethod)
        {
            this.DisplayName = Translator.Translate(name);
            this.ToolTip = Translator.Translate(toolTip);
            this.ImageName = image;

            this.ExecuteMethod = executeMethod;
            this.CanExecuteMethod = canExecuteMethod;
        }

        public void UpdateCommand(string name, string toolTip, string image)
        {
            this.DisplayName = Translator.Translate(name);
            this.ToolTip = Translator.Translate(toolTip);
            this.ImageName = image;
        }

        public void Execute(object parameter)
        {
            if (this.ExecuteMethod == null)
                return;

            this.ExecuteMethod(parameter);
        }

        public bool CanExecute(object parameter)
        {
            if (this.CanExecuteMethod == null)
                return true;

            return this.CanExecuteMethod(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
