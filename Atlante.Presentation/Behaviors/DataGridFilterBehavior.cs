using System;
using System.Collections;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Atlante.Presentation.Interfaces;
using Atlante.Presentation.Controls;

namespace Atlante.Presentation.Behaviors
{
    public class DataGridFilterBehavior : DependencyObject
    {
        private DataGrid _dataGrid;
        private IFilterList _filterList;
        private IEnumerable _itemsSource;

        public static readonly DependencyProperty FilterListProperty = DependencyProperty.RegisterAttached("FilterList", typeof(IFilterList), typeof(DataGridFilterBehavior), new UIPropertyMetadata(null, OnFilterListChanged));
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.RegisterAttached("ItemsSource", typeof(IEnumerable), typeof(DataGridFilterBehavior), new UIPropertyMetadata(null, OnItemsSourceChanged));
        //public static readonly DependencyProperty MultipleFilterListProperty = DependencyProperty.RegisterAttached("MultipleFilterList", typeof(IEnumerable<IFilterList>), typeof(DataGridFilterBehavior), new UIPropertyMetadata(null, OnMultipleFilterListChanged));

        public static IFilterList GetFilterList(DataGrid collectionViewSource)
        {
            return (IFilterList)collectionViewSource.GetValue(FilterListProperty);
        }

        public static void SetFilterList(DataGrid collectionViewSource, IFilterList value)
        {
            collectionViewSource.SetValue(FilterListProperty, value);
        }

        public static IEnumerable GetItemsSource(DependencyObject element)
        {
            return (IEnumerable)element.GetValue(ItemsSourceProperty);
        }

        public static void SetItemsSource(DependencyObject element, IEnumerable value)
        {
            element.SetValue(ItemsSourceProperty, value);
        }

        //public static IEnumerable<IFilterList> GetMultipleFilterList(DataGrid collectionViewSource)
        //{
        //    return (IEnumerable<IFilterList>)collectionViewSource.GetValue(MultipleFilterListProperty);
        //}

        //public static void SetFilterList(DataGrid collectionViewSource, IEnumerable<IFilterList> value)
        //{
        //    collectionViewSource.SetValue(MultipleFilterListProperty, value);
        //}

        private static void OnFilterListChanged(DependencyObject element, DependencyPropertyChangedEventArgs e)
        {
            var behavior = new DataGridFilterBehavior(element as DataGrid, e.NewValue as IFilterList);
            behavior.ExecuteFilter();
        }

        private static void OnItemsSourceChanged(DependencyObject element, DependencyPropertyChangedEventArgs e)
        {
            var behavior = new DataGridFilterBehavior(element as IFilterList, e.NewValue as IEnumerable);
            behavior.ExecuteFilter();
        }

        //private static void OnMultipleFilterListChanged(DependencyObject element, DependencyPropertyChangedEventArgs e)
        //{
        //    //var behavior = new DataGridFilterBehavior(element as DataGrid, e.NewValue as IFilterList);
        //    //behavior.ExecuteFilter();
        //}

        private DataGridFilterBehavior(DataGrid dataGrid, IFilterList filterList)
        {
            _dataGrid = dataGrid;
            _filterList = filterList;

            _filterList.OnSelectionChanged += new Action(FilterSelectionChanged);
        }

        private DataGridFilterBehavior(IFilterList filterList, IEnumerable itemsSource )
        {
            _itemsSource = itemsSource;
            _filterList = filterList;

            _filterList.OnSelectionChanged += new Action(FilterSelectionChanged);
        }

        private void FilterSelectionChanged()
        {
            this.ExecuteFilter();
        }

        private void ExecuteFilter()
        {
            if (_filterList.FilterSource == null)
                return;

            //var collection = CollectionViewSource.GetDefaultView(_dataGrid.ItemsSource);
            var collection = CollectionViewSource.GetDefaultView(_itemsSource);
            collection.Filter = FilterPredicate;
        }

        private bool FilterPredicate(object item)
        {
            if (string.IsNullOrEmpty(_filterList.FilterProperty))
                return true;

            object propertyValue = item;
            foreach (var propertyName in _filterList.FilterProperty.Split('.'))
            {
                propertyValue = this.GetPropertyValue(propertyValue, propertyName);

                if (propertyValue == null)
                    break;
            }

            if (propertyValue == null)
                return true;

            foreach (var selectableItem in _filterList.FilterSource)
            {
                if (!selectableItem.IsSelected)
                    continue;

                if (propertyValue.ToString() == selectableItem.Value.ToString())
                    return true;
            }

            return false;
        }

        private object GetPropertyValue(object item, string propertyName)
        {
            Type itemType = item.GetType();
            PropertyInfo propertyInfo = itemType.GetProperty(propertyName);
            return propertyInfo.GetValue(item, new object[] { });
        }
    }
}
