using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace Atlante.Common
{
    [Serializable]
    public class Parameters : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public List<Parameter> Items { get; set; }

        public Parameters()
        {
            this.Items = new List<Parameter>();
        }

        public Parameter this[string key]
        {
            get { return this.ParameterAt(key); }
        }

        public void Add(Parameter item)
        {
            this.Items.Add(item);

            if (PropertyChanged != null)
                item.PropertyChanged += new PropertyChangedEventHandler(this.PropertyChanged);
        }

        public void Remove(Parameter item)
        {
            this.Items.Remove(item);
        }

        public T GetValue<T>(Expression<Func<object>> property)
        {
            MemberExpression memberExpression;
            if (property.Body is UnaryExpression)
                memberExpression = (property.Body as UnaryExpression).Operand as MemberExpression;
            else
                memberExpression = property.Body as MemberExpression;

            if (memberExpression == null)
                return default(T);

            var propertyInfo = memberExpression.Member as PropertyInfo;
            if (propertyInfo == null)
                return default(T);

            var parameter = this.ParameterAt(propertyInfo.Name);
            if (parameter == null)
                return default(T);

            if (string.IsNullOrEmpty(parameter.Value))
                return default(T);

            return (T)Convert.ChangeType(parameter.Value, typeof(T));
        }

        private Parameter ParameterAt(string parameterKey)
        {
            foreach (Parameter parameter in this.Items)
                if (parameter.Key == parameterKey)
                    return parameter;

            return null;
        }

        private void NotifyPropertyChanged(string property)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
    }
}
