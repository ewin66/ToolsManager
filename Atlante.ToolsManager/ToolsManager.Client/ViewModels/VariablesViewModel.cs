using Atlante.Common;
using Atlante.Presentation;
using Atlante.Presentation.Interfaces;
using Atlante.Presentation.Objects;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ToolsManager.DataModel.Common;

namespace ToolsManager.Client.ViewModels
{
	public class VariablesViewModel : IPropertiesEditor
	{
		public IList<IProperty> Properties { get; set; }

		public bool ReadOnlyProperties
		{
			get { return false; }
		}

		public VariablesViewModel()
		{
			this.LoadProperties();
		}

		public void SaveChanges()
		{
			var variables = new ToolsVariables();

			foreach (var property in this.Properties)
			{
				variables.Items.Add(new ToolsVariable() { Key = property.Key, Value = property.Value.ToString() });
			}

			Serializer.Serialize<ToolsVariables>(variables, @"Config\ToolsVariables.xml");
		}

		private void LoadProperties()
		{
			this.Properties = new ObservableCollection<IProperty>();

			var variables = Serializer.Deserialize<ToolsVariables>(@"Config\ToolsVariables.xml");
			if (variables == null)
				return;

			foreach (var variable in variables.Items)
			{
				this.Properties.Add(new Property() { Key = variable.Key, Value = variable.Value, EditorType = PropertyEditorType.StringEditor });
			}
		}

		public void PropertyValueChanged(string propertyKey, object newValue)
		{
		}
	}
}
