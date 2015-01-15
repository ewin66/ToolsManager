using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using Atlante.Presentation;
using Atlante.Presentation.Interfaces;
using Atlante.Presentation.Objects;
using ToolsManager.Client.Properties;

namespace ToolsManager.Client.ViewModels
{
    public class SettingsViewModel : IPropertiesEditor
    {
        public IList<IProperty> Properties { get; set; }

        public bool ReadOnlyProperties
        {
            get { return false; }
        }

        public SettingsViewModel( )
        {
            this.LoadSettings( );
        }

        public void SaveChanges( )
        {
            foreach ( SettingsProperty setting in Settings.Default.Properties )
            {
                var property = this.Properties.Where( p => p.Key == setting.Name ).FirstOrDefault( );

                if ( property != null )
                    Settings.Default[ setting.Name ] = property.Value;
            }

            Settings.Default.Save( );
        }

        private void LoadSettings( )
        {
            this.Properties = new ObservableCollection<IProperty>( );

            this.Properties.Add( new Property( ) { Key = "Theme", Value = Settings.Default.Theme, EditorType = PropertyEditorType.CollectionEditor, Options = new List<object>( ) { "BlackTheme", "BlueTheme" } } );
            this.Properties.Add( new Property( ) { Key = "Language", Value = Settings.Default.Language, EditorType = PropertyEditorType.CollectionEditor, Options = new List<object>( ) { "en-US", "es-ES" } } );
        }

        public void PropertyValueChanged(string propertyKey, object newValue)
        {
        }
    }
}
