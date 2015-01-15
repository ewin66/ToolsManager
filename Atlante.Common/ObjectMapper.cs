using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Atlante.Common
{
    public class ObjectMapper
    {
        public string AssemblyNamespace { get; private set; }
        public string AssemblyNamespaceDTO { get; private set; }

        public ObjectMapper(Type entityType, Type DTOType)
        {
            this.AssemblyNamespace = entityType.Namespace;
            this.AssemblyNamespaceDTO = DTOType.Namespace;
        }

        /// <summary>
        /// Map DTO objects to entity objects or entity objects to DTO objects.
        /// </summary>
        /// <param name="source">Object source</param>
        /// <returns>A new object of target</returns>
        public object Map(object source, bool fromDTO)
        {
            Type sourceType = source.GetType();
            PropertyInfo[] sourceProperties = sourceType.GetProperties();

            var targetName = this.GetTargetName(sourceType.Name, fromDTO);
            Type targetType = Type.GetType(targetName);

            if (targetType == null)
            {
                Logger.AddWarning(string.Format("The type {0} cannot be mapped. Verified that it exists.", targetName));
                return null;
            }

            object target = Activator.CreateInstance(targetType);

            PropertyInfo[] targetProperties = targetType.GetProperties();

            foreach (var sourceProperty in sourceProperties)
            {
                var targetProperty = Match(sourceProperty, targetProperties);

                if (targetProperty == null)
                    continue;

                var sourcePropertyValue = sourceProperty.GetValue(source, null);

                if (sourcePropertyValue == null)
                    continue;

                if (sourcePropertyValue.GetType().IsGenericType)
                    this.MapGenericList(sourcePropertyValue, targetProperty, target, fromDTO);
                else if (sourcePropertyValue.GetType().IsClass && !sourcePropertyValue.GetType().IsSealed)
                    this.MapClass(sourcePropertyValue, targetProperty, target, fromDTO);
                else
                    this.MapSystemTypes(sourcePropertyValue, targetProperty, target, fromDTO);
            }

            return target;
        }

        private void MapGenericList(object source, PropertyInfo targetProperty, object target, bool fromDTO)
        {
            IList listSource = source as IList;

            var itemTargetName = this.GetTargetName(listSource.GetType().GetGenericArguments()[0].Name, fromDTO);
            Type itemTargetType = Type.GetType(itemTargetName);

            if (itemTargetType == null)
            {
                Logger.AddWarning(string.Format("The type {0} cannot be mapped. Verified that it exists.", itemTargetType));
                return;
            }

            IList listTarget = Activator.CreateInstance(typeof(List<>).MakeGenericType(itemTargetType)) as IList;

            foreach (var itemSource in listSource)
            {
                var itemTarget = this.Map(itemSource, fromDTO);
                listTarget.Add(itemTarget);
            }

            targetProperty.SetValue(target, listTarget, null);
        }

        private void MapClass(object source, PropertyInfo targetProperty, object target, bool fromDTO)
        {
            ObjectMapper mapper;

            if (fromDTO)
                mapper = new ObjectMapper(targetProperty.PropertyType, source.GetType());
            else
                mapper = new ObjectMapper(source.GetType(), targetProperty.PropertyType);

            var targetPropertyValue = mapper.Map(source, fromDTO);

            targetProperty.SetValue(target, targetPropertyValue, null);
        }

        private void MapSystemTypes(object source, PropertyInfo targetProperty, object target, bool fromDTO)
        {
            object sourcePropertyConvertedValue = null;
            try
            {
                sourcePropertyConvertedValue = this.ChangeType(source, targetProperty.PropertyType);
            }
            catch (Exception)
            {
                Logger.AddWarning(string.Format("The value '{0}' cannot be changed to type {1}", source, targetProperty.PropertyType));
            }

            targetProperty.SetValue(target, sourcePropertyConvertedValue, null);
        }

        private string GetTargetName(string sourceTypeName, bool fromDTO)
        {
            if (fromDTO)
                return string.Format("{0}.{1}, {0}", this.AssemblyNamespace, sourceTypeName.Replace("DTO", string.Empty));
            else
                return string.Format("{0}.{1}DTO, {0}", this.AssemblyNamespaceDTO, sourceTypeName);
        }

        private PropertyInfo Match(PropertyInfo sourceProperty, PropertyInfo[] targetProperties)
        {
            foreach (var target in targetProperties)
                if (target.Name.Equals(sourceProperty.Name))
                    return target;

            return null;
        }

        private object ChangeType(object sourceValue, Type targetType)
        {
            if (targetType == typeof(bool))
            {
                if (sourceValue == null)
                    sourceValue = false;
                else if (sourceValue is bool)
                    sourceValue = (bool)sourceValue;
                else
                    sourceValue = Boolean.Parse(sourceValue.ToString());
            }
            else if (targetType == typeof(string))
            {
                sourceValue = Convert.ChangeType(sourceValue, targetType);
            }
            else if (targetType.IsEnum)
            {
                sourceValue = Enum.Parse(targetType, sourceValue.ToString(), true);
            }
            else if (targetType == typeof(Guid))
            {
                if (!(sourceValue is Guid))
                {
                    if (sourceValue is string)
                        sourceValue = new Guid(sourceValue.ToString());
                    else
                        sourceValue = new Guid((byte[])sourceValue);
                }
            }
            else
            {
                sourceValue = Convert.ChangeType(sourceValue, targetType);
            }
            return sourceValue;
        }
    }
}
