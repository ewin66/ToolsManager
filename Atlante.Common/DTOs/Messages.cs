using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Atlante.Common
{
    [Serializable]
    public class Messages : IMessages
    {
        [XmlArray( "Messages" )]
        public List<Message> Items { get; set; }

        [XmlIgnore]
        public bool HasErrors
        {
            get
            {
                var errors = this.Items.Where( m => m.Type == MessageType.error || m.Type == MessageType.warning ).FirstOrDefault( );

                if ( errors == null )
                    return false;

                return true;
            }
        }

        public Messages( )
        {
            this.Items = new List<Message>( );
        }

        public void Add( object message )
        {
            if ( !( message is Message ) )
                return;

            this.Items.Add( message as Message );
        }

        public void AddArray(Messages messages)
        {
            foreach (var message in messages.Items)
                this.Add(message);
        }

        public void AddException(Exception e)
        {
            string message;
            if (e.InnerException == null)
                message = e.Message;
            else
                message = string.Format("{0}. {1}", e.Message, e.InnerException.Message);

            this.AddException(e, message);
        }

        public void AddException( Exception e, string description )
        {
            Items.Add( new Message( ) { Type = MessageType.error, Description = description, Exception = e.GetType( ).ToString( ), Trace = e.StackTrace } );
        }

        public void AddWarning( string description )
        {
            this.Items.Add( new Message( ) { Type = MessageType.warning, Description = description } );
        }

        public void AddInformation( string description )
        {
            this.Items.Add( new Message( ) { Type = MessageType.information, Description = description } );
        }

        public IEnumerator GetEnumerator( )
        {
            return this.Items.GetEnumerator( );
        }
    }
}
