using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using Atlante.Mef.Interfaces;

namespace ToolsManager.DataServices.Client
{
    public class TasksManager
    {
        [ImportMany]
        public IEnumerable<ITool> Tools { get; private set; }

        public IList<string> AvailableTools { get; private set; }

        private static TasksManager _manager;

        private TasksManager( )
        {
            this.LoadData( );
            this.LoadAvailableTools( );
        }

        public static TasksManager Create( )
        {
            if ( _manager == null )
                _manager = new TasksManager( );

            return _manager;
        }

        public ITool CreateTaskEngineInstance( string category )
        {
            foreach ( var task in this.Tools )
                if ( task.ToolName.Equals( category ) )
                    return task.CreateInstance();

            return null;
        }

        private void LoadData( )
        {
            var catalog = new AggregateCatalog( );

            catalog.Catalogs.Add( new DirectoryCatalog( Directory.GetCurrentDirectory( ) ) );

            var container = new CompositionContainer( catalog );

            try
            {
                container.ComposeParts( this );
            }
            catch ( CompositionException compositionException )
            {
                Console.WriteLine( compositionException.ToString( ) );
            }
        }

        private void LoadAvailableTools( )
        {
            this.AvailableTools = new List<string>( );

            foreach ( var tool in this.Tools )
                this.AvailableTools.Add( tool.ToolName );
        }        
    }
}
