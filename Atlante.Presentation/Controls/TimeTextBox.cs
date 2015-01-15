using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Atlante.Common.Extensions;

namespace Atlante.Presentation.Controls
{
    public class TimeTextBox : TextBox
    {
        private const int HOUR_INDEX = 0;
        private const int MINUTE_INDEX = 3;
        private const int SECOND_INDEX = 6;
        private const int DIGITS_COUNT = 2;

        public bool IsHourSelected
        {
            get { return base.SelectionStart >= HOUR_INDEX && this.SelectionStart <= HOUR_INDEX + DIGITS_COUNT; }
        }

        public bool IsMinuteSelected
        {
            get { return base.SelectionStart >= MINUTE_INDEX && this.SelectionStart <= MINUTE_INDEX + DIGITS_COUNT; }
        }

        public bool IsSecondSelected
        {
            get { return base.SelectionStart >= SECOND_INDEX && this.SelectionStart <= SECOND_INDEX + DIGITS_COUNT; }
        }

        public int HourValue
        {
            get { return Convert.ToInt32( base.Text.Substring( HOUR_INDEX, DIGITS_COUNT ) ); }
            set
            {
                base.Text = base.Text.Replace( HOUR_INDEX, DIGITS_COUNT, this.CompleteDigits( value ) );
                this.Time = DateTime.Parse( base.Text );
            }
        }

        public int MinuteValue
        {
            get { return Convert.ToInt32( base.Text.Substring( MINUTE_INDEX, DIGITS_COUNT ) ); }
            set
            {
                base.Text = base.Text.Replace( MINUTE_INDEX, DIGITS_COUNT, this.CompleteDigits( value % 60 ) );
                this.Time = DateTime.Parse( base.Text );
            }
        }

        public int SecondValue
        {
            get { return Convert.ToInt32( base.Text.Substring( SECOND_INDEX, DIGITS_COUNT ) ); }
            set
            {
                base.Text = base.Text.Replace( SECOND_INDEX, DIGITS_COUNT, this.CompleteDigits( value % 60 ) );
                this.Time = DateTime.Parse( base.Text );
            }
        }

        public DateTime Time
        {
            get { return (DateTime) GetValue( TimeProperty ); }
            set { SetValue( TimeProperty, value ); }
        }

        public static readonly DependencyProperty TimeProperty = DependencyProperty.Register( "Time", typeof( DateTime ),
                                                                                               typeof( TimeTextBox ),
                                                                                               new UIPropertyMetadata( DateTime.Now, OnTimeChanged ) );

        private static void OnTimeChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
        {
            var textBox = ( d as TimeTextBox );
            var newDate = (DateTime) e.NewValue;

            if ( newDate == DateTime.MinValue )
                newDate = DateTime.Now;

            textBox.Text = newDate.ToString( "HH:mm:ss" );
            textBox.Time = newDate;
        }

        protected override void OnPreviewKeyDown( KeyEventArgs e )
        {
            switch ( e.Key )
            {
                case Key.Tab:
                    //if ( e.Modifier == SHIFT )
                    //    this.MovePrevious( );
                    this.MoveNext( );
                    e.Handled = true;
                    break;
                case Key.Up:
                    this.Increase( 1 );
                    e.Handled = true;
                    break;
                case Key.Down:
                    this.Increase( -1 );
                    e.Handled = true;
                    break;
                case Key.Left:
                    e.Handled = !this.CanMoveLeft( );
                    break;
                case Key.Right:
                    e.Handled = !this.CanMoveRight( );
                    break;
                case Key.NumPad0:
                case Key.NumPad1:
                case Key.NumPad2:
                case Key.NumPad3:
                case Key.NumPad4:
                case Key.NumPad5:
                case Key.NumPad6:
                case Key.NumPad7:
                case Key.NumPad8:
                case Key.NumPad9:
                    e.Handled = !this.RemoveCurrentNumber( );
                    break;
                default:
                    e.Handled = true;
                    break;
            }
        }

        private void MoveNext( )
        {
            if ( this.IsHourSelected )
                base.SelectionStart = MINUTE_INDEX;
            else if ( IsMinuteSelected )
                base.SelectionStart = SECOND_INDEX;
            else if ( IsSecondSelected )
                base.SelectionStart = HOUR_INDEX;

            this.SelectionLength = 2;
        }

        private void MovePrevious( )
        {
            if ( this.IsHourSelected )
                base.SelectionStart = SECOND_INDEX;
            else if ( IsMinuteSelected )
                base.SelectionStart = HOUR_INDEX;
            else if ( IsSecondSelected )
                base.SelectionStart = MINUTE_INDEX;

            this.SelectionLength = 2;
        }

        private bool CanMoveLeft( )
        {
            if ( this.SelectionStart >= HOUR_INDEX + 1 && this.SelectionStart < HOUR_INDEX + DIGITS_COUNT + 1 )
                return true;

            if ( this.SelectionStart >= MINUTE_INDEX + 1 && this.SelectionStart < MINUTE_INDEX + DIGITS_COUNT + 1 )
                return true;

            if ( this.SelectionStart >= SECOND_INDEX + 1 && this.SelectionStart < SECOND_INDEX + DIGITS_COUNT + 1 )
                return true;

            return false;
        }

        private bool CanMoveRight( )
        {
            if ( this.SelectionStart >= HOUR_INDEX && this.SelectionStart < HOUR_INDEX + DIGITS_COUNT )
                return true;

            if ( this.SelectionStart >= MINUTE_INDEX && this.SelectionStart < MINUTE_INDEX + DIGITS_COUNT )
                return true;

            if ( this.SelectionStart >= SECOND_INDEX && this.SelectionStart < SECOND_INDEX + DIGITS_COUNT )
                return true;

            return false;
        }

        private void Increase( int delta )
        {
            if ( this.IsHourSelected )
            {
                var value = ( this.HourValue + delta ) % 24;
                if ( value < 0 )
                    value = 23;

                this.HourValue = value;
                this.SelectText( HOUR_INDEX, DIGITS_COUNT );
            }
            else if ( IsMinuteSelected )
            {
                var value = ( this.MinuteValue + delta ) % 60;
                if ( value < 0 )
                    value = 59;

                this.MinuteValue = value;
                this.SelectText( MINUTE_INDEX, DIGITS_COUNT );
            }
            else if ( IsSecondSelected )
            {
                var value = ( this.SecondValue + delta ) % 60;
                if ( value < 0 )
                    value = 59;

                this.SecondValue = value;
                this.SelectText( SECOND_INDEX, DIGITS_COUNT );
            }
        }

        private bool RemoveCurrentNumber( )
        {
            if ( !this.CanMoveRight( ) )
                return false;

            int start = this.SelectionStart;
            this.Text = this.Text.Remove( this.SelectionStart, 1 );
            this.SelectionStart = start;

            return true;
        }

        private void SelectText( int index, int count )
        {
            this.SelectionStart = index;
            this.SelectionLength = DIGITS_COUNT;
        }

        private string CompleteDigits( int value )
        {
            string twoDigitsValue = value.ToString( );

            if ( twoDigitsValue.Length == 1 )
                twoDigitsValue = "0" + twoDigitsValue;

            return twoDigitsValue;
        }
    }
}
