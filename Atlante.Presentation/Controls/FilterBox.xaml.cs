using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Atlante.Presentation.Interfaces;
using Atlante.Presentation.Objects;

namespace Atlante.Presentation.Controls
{
    /// <summary>
    /// Interaction logic for FilterBox.xaml
    /// </summary>
    public partial class FilterBox : UserControl, IFilterList, INotifyPropertyChanged
    {
        private bool _isChecked;

        public event Action OnSelectionChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(string), typeof(FilterBox));
        public static readonly DependencyProperty FilterPropertyProperty = DependencyProperty.Register("FilterProperty", typeof(string), typeof(FilterBox), new PropertyMetadata(string.Empty, OnFilterPropertyChanged));
        public static readonly DependencyProperty FilterSourceProperty = DependencyProperty.Register("FilterSource", typeof(IList<SelectableItem>), typeof(FilterBox), new PropertyMetadata(null, OnFilterSourceChanged));

        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        public string FilterProperty
        {
            get { return (string)GetValue(FilterPropertyProperty); }
            set { SetValue(FilterPropertyProperty, value); }
        }

        public IList<SelectableItem> FilterSource
        {
            get { return (IList<SelectableItem>)GetValue(FilterSourceProperty); }
            set { SetValue(FilterSourceProperty, value); }
        }

        public bool IsChecked
        {
            get
            {
                return _isChecked;
            }
            set
            {
                _isChecked = value;

                if (this.PropertyChanged != null)
                    this.PropertyChanged(this, new PropertyChangedEventArgs("IsChecked"));
            }
        }

        public FilterBox()
        {
            InitializeComponent();

            this.DataContext = this;
        }

        private static void OnFilterPropertyChanged(DependencyObject element, DependencyPropertyChangedEventArgs args)
        {
            (element as FilterBox).FilterProperty = (string)args.NewValue;
        }

        private static void OnFilterSourceChanged(DependencyObject element, DependencyPropertyChangedEventArgs args)
        {
            (element as FilterBox).FilterSource = (IList<SelectableItem>)args.NewValue;
        }

        private void CheckedChanged(object sender, RoutedEventArgs e)
        {
            if (this.OnSelectionChanged != null)
                this.OnSelectionChanged();
        }
    }
}
