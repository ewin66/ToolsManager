using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Xml.Serialization;
using System.Reflection;

namespace Atlante.Common
{
    [Serializable]
    public class ParameterBase
    {
        [XmlAttribute( "Key" )]
        public string Key { get; set; }

        [XmlAttribute( "Default" )]
        public string DefaultValue { get; set; }

        [XmlAttribute( "Type" )]
        public ParameterType ParameterType { get; set; }

        [XmlIgnore]
        public IList<string> Options { get; set; }

        public ParameterBase( string key, string defaultValue, ParameterType parameterType )
        {
            this.Key = key;
            this.DefaultValue = defaultValue;
            this.ParameterType = parameterType;
        }

        public ParameterBase( Expression<Func<object>> property, string defaultValue, ParameterType parameterType )
            : this( string.Empty, defaultValue, parameterType )
        {
            MemberExpression memberExpression;
            if ( property.Body is UnaryExpression )
                memberExpression = ( property.Body as UnaryExpression ).Operand as MemberExpression;
            else
                memberExpression = property.Body as MemberExpression;

            if ( memberExpression != null )
            {
                var propertyInfo = memberExpression.Member as PropertyInfo;
                if ( propertyInfo != null )
                    this.Key = propertyInfo.Name;
            }
        }

        public ParameterBase( string key, string defaultValue, ParameterType parameterType, IList<string> options )
            : this( key, defaultValue, parameterType )
        {
            this.Options = options;
        }
    }
}
