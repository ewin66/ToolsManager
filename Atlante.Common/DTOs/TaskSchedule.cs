using System;
using System.Xml.Serialization;
using System.ComponentModel;

namespace Atlante.Common
{
    [Serializable]
    public class TaskSchedule : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private DateTime _time;
        private bool _isEnabled;
        private int _recurEvery;
        private string _daysOfWeek;
        private ScheduleFrequency _frecuency;

        [XmlAttribute( "Time" )]
        public DateTime Time
        {
            get { return _time; }
            set
            {
                _time = value;
                this.NotifyPropertyChanged( "Time" );
            }
        }

        [XmlAttribute( "IsEnabled" )]
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                _isEnabled = value;
                this.NotifyPropertyChanged( "IsEnabled" );
            }
        }

        [XmlAttribute( "RecurEvery" )]
        public int RecurEvery
        {
            get { return _recurEvery; }
            set
            {
                if ( _recurEvery == value )
                    return;

                _recurEvery = value;
                this.NotifyPropertyChanged( "RecurEvery" );
            }
        }

        [XmlAttribute( "Days" )]
        public string DaysOfWeek
        {
            get { return _daysOfWeek; }
            set
            {
                if ( _daysOfWeek == value )
                    return;

                _daysOfWeek = value;
                this.NotifyPropertyChanged( "DaysOfWeek" );
            }
        }

        [XmlAttribute( "Frequency" )]
        public ScheduleFrequency Frequency
        {
            get { return _frecuency; }
            set
            {
                if ( _frecuency == value )
                    return;

                _frecuency = value;
                this.NotifyPropertyChanged( "Frequency" );
            }
        }

        public TaskSchedule( )
        {
            this.Time = DateTime.Now;
        }

        private void NotifyPropertyChanged( string property )
        {
            if ( this.PropertyChanged != null )
                this.PropertyChanged( this, new PropertyChangedEventArgs( property ) );
        }
    }
}
