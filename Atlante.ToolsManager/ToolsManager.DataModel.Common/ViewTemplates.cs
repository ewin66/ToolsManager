using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Xml.Serialization;

namespace ToolsManager.DataModel.Common
{
    [Serializable]
    public class ViewTemplate
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public string ConfigFile { get; set; }
        public string Type { get; set; }

        public List<string> EnabledTools { get; set; }

        public ViewTemplate( )
        {
            this.EnabledTools = new List<string>( );
        }
    }

    [Serializable]
    public class ViewTemplates
    {
        public List<ViewTemplate> Items { get; set; }

        public ViewTemplate this[ int index ]
        {
            get { return this.Items[ index ]; }
        }

        public ViewTemplate this[ Guid id ]
        {
            get { return this.ViewTemplateAt( id ); }
        }

        [XmlIgnore( )]
        public NotifyCollectionChangedEventHandler CollectionChanged { get; set; }

        public ViewTemplates( )
        {
            this.Items = new List<ViewTemplate>( );
        }

        public void Add( ViewTemplate item )
        {
            this.Items.Add( item );

            if ( this.CollectionChanged != null )
                this.CollectionChanged( this, new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Add, item ) );
        }

        public void Remove( ViewTemplate item )
        {
            this.Items.Remove( item );

            if ( this.CollectionChanged != null )
                this.CollectionChanged( this, new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Remove, item ) );
        }

        private ViewTemplate ViewTemplateAt( Guid id )
        {
            return this.Items.Where( v => v.Id == id ).FirstOrDefault( );
        }
    }
}
