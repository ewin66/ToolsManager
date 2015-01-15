using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Atlante.Common
{
    [Serializable]
    public class TaskInfo : INotifyPropertyChanged
    {
        private string _category;
        private string _description;
        private bool _isSelected;

        private double _posX;
        private double _posY;

        private Guid _successBranchTaskId;
        private Guid _errorBranchTaskId;

        private double _targetSuccessPosX;
        private double _targetSuccessPosY;
        private double _targetErrorPosX;
        private double _targetErrorPosY;

        private DateTime _lastExecution;
        private TaskStatus _taskStatus;
        private TaskSchedule _schedule;
        private Parameters _parameters;

        public event PropertyChangedEventHandler PropertyChanged;

        [XmlAttribute("Id")]
        public Guid Id { get; set; }

        [XmlAttribute("Category")]
        public string Category
        {
            get { return _category; }
            set
            {
                if (_category == value)
                    return;

                _category = value;
                this.NotifyPropertyChanged("Category");
            }
        }

        [XmlAttribute("Description")]
        public string Description
        {
            get { return _description; }
            set
            {
                if (_description == value)
                    return;

                _description = value;
                this.NotifyPropertyChanged("Description");
            }
        }

        [XmlAttribute("PosX")]
        public double PosX
        {
            get { return _posX; }
            set
            {
                if (_posX == value)
                    return;

                _posX = value;
                this.NotifyPropertyChanged("PosX");
            }
        }

        [XmlAttribute("PosY")]
        public double PosY
        {
            get { return _posY; }
            set
            {
                if (_posY == value)
                    return;

                _posY = value;
                this.NotifyPropertyChanged("PosY");
            }
        }

        [XmlAttribute("SuccessBranchTaskId")]
        public Guid SuccessBranchTaskId
        {
            get { return _successBranchTaskId; }
            set
            {
                if (_successBranchTaskId == value)
                    return;

                _successBranchTaskId = value;
                this.NotifyPropertyChanged("SuccessBranchTaskId");
            }
        }

        [XmlAttribute("ErrorBranchTaskId")]
        public Guid ErrorBranchTaskId
        {
            get { return _errorBranchTaskId; }
            set
            {
                if (_errorBranchTaskId == value)
                    return;

                _errorBranchTaskId = value;
                this.NotifyPropertyChanged("ErrorBranchTaskId");
            }
        }

        [XmlAttribute("LastRun")]
        public DateTime LastExecution
        {
            get { return _lastExecution; }
            set
            {
                if (LastExecution == value)
                    return;

                _lastExecution = value;
                this.NotifyPropertyChanged("LastExecution");
            }
        }

        [XmlAttribute("Status")]
        public TaskStatus Status
        {
            get { return _taskStatus; }
            set
            {
                if (_taskStatus == value)
                    return;

                _taskStatus = value;
                this.NotifyPropertyChanged("Status");
            }
        }

        public Parameters Parameters
        {
            get { return _parameters; }
            set
            {
                _parameters = value;
                this.NotifyPropertyChanged("Parameters");
            }
        }

        public string ImageName
        {
            get { return _category + ".png"; }
        }

        public bool HasSuccessBranch
        {
            get { return _successBranchTaskId != Guid.Empty; }
        }

        public bool HasErrorBranch
        {
            get { return _errorBranchTaskId != Guid.Empty; }
        }

        public double SourceSuccessPosX
        {
            get { return _posX + 135; }
        }

        public double SourceSuccessPosY
        {
            get { return _posY + 35; }
        }

        public double SourceErrorPosX
        {
            get { return _posX + 135; }
        }

        public double SourceErrorPosY
        {
            get { return _posY + 45; }
        }

        public double TargetSuccessPosX
        {
            get { return _targetSuccessPosX; }
            set
            {
                _targetSuccessPosX = value;
                this.NotifyPropertyChanged("TargetSuccessPosX");
            }
        }

        public double TargetSuccessPosY
        {
            get { return _targetSuccessPosY; }
            set
            {
                _targetSuccessPosY = value;
                this.NotifyPropertyChanged("TargetSuccessPosY");
            }
        }

        public double TargetErrorPosX
        {
            get { return _targetErrorPosX; }
            set
            {
                _targetErrorPosX = value;
                this.NotifyPropertyChanged("TargetErrorPosX");
            }
        }

        public double TargetErrorPosY
        {
            get { return _targetErrorPosY; }
            set
            {
                _targetErrorPosY = value;
                this.NotifyPropertyChanged("TargetErrorPosY");
            }
        }

        [XmlElement("Schedule")]
        public TaskSchedule Schedule
        {
            get { return _schedule; }
            set
            {
                _schedule = value;
                this.NotifyPropertyChanged("Schedule");
            }
        }

        [XmlIgnore]
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                this.NotifyPropertyChanged("IsSelected");
            }
        }

        public TaskInfo()
        {
            this.Id = Guid.NewGuid();
            this.Status = TaskStatus.None;

            this.Parameters = new Parameters();
        }

        private void NotifyPropertyChanged(string property)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
    }
}
