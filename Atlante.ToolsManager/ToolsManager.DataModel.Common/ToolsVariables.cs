using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Xml.Serialization;

namespace ToolsManager.DataModel.Common
{
    [Serializable]
    public class ToolsVariable
    {
        public string Key { get; set; }
        public string Value { get; set; }

		public ToolsVariable()
        {
        }
    }

    [Serializable]
	public class ToolsVariables
    {
        public List<ToolsVariable> Items { get; set; }

		public ToolsVariable this[int index]
        {
            get { return this.Items[ index ]; }
        }

        [XmlIgnore( )]
        public NotifyCollectionChangedEventHandler CollectionChanged { get; set; }

		public ToolsVariables()
        {
            this.Items = new List<ToolsVariable>( );
        }

        public void Add( ToolsVariable item )
        {
            this.Items.Add( item );

            if ( this.CollectionChanged != null )
                this.CollectionChanged( this, new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Add, item ) );
        }

        public void Remove( ToolsVariable item )
        {
            this.Items.Remove( item );

            if ( this.CollectionChanged != null )
                this.CollectionChanged( this, new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Remove, item ) );
        }
    }
}
