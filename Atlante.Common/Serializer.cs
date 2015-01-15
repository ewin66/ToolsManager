using System;
using System.IO;
using System.Xml.Serialization;

namespace Atlante.Common
{
    public static class Serializer
    {
        public static void Serialize<T>( T obj, string path )
        {
            try
            {
                string directory = Path.GetDirectoryName( path );

                if ( !string.IsNullOrEmpty( directory ) )
                    Directory.CreateDirectory( directory );

                using ( StreamWriter writer = new StreamWriter( path, false ) )
                {
                    try
                    {
                        XmlSerializer serializer = new XmlSerializer( typeof( T ) );
                        serializer.Serialize( writer, obj );
                    }
                    catch ( Exception e )
                    {
                        Logger.AddException( e );
                    }
                }
            }
            catch ( Exception e )
            {
                Logger.AddException( e );
            }
        }

        public static void Serialize<T, E>( T obj, string path )
        {
            try
            {
                string directory = Path.GetDirectoryName( path );

                if ( !string.IsNullOrEmpty( directory ) )
                    Directory.CreateDirectory( directory );

                using ( StreamWriter writer = new StreamWriter( path, false ) )
                {
                    try
                    {
                        XmlSerializer serializer = new XmlSerializer( typeof( T ), new Type[ ] { typeof( E ) } );
                        serializer.Serialize( writer, obj );
                    }
                    catch ( Exception e )
                    {
                        Logger.AddException( e );
                    }
                }
            }
            catch ( Exception e )
            {
                Logger.AddException( e );
            }
        }

        public static T Deserialize<T>( string path )
        {
            T result = default( T );

            try
            {
                using ( StreamReader reader = new StreamReader( path ) )
                {
                    try
                    {
                        XmlSerializer serializer = new XmlSerializer( typeof( T ) );
                        result = (T) serializer.Deserialize( reader );
                    }
                    catch ( Exception e )
                    {
                        Logger.AddException( e );
                    }
                }
            }
            catch ( Exception e )
            {
                Logger.AddException( e );
            }

            return result;
        }
    }
}
