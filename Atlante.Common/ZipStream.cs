using System;
using System.IO;
using System.IO.Compression;

namespace Atlante.Common
{
    internal static class ZipStream
    {
        public static string FILE_EXTENSION = "zcs";

        public static Stream Compress( string sourcePath )
        {
            if ( string.IsNullOrEmpty( sourcePath ) )
                return null;

            if ( !Directory.Exists( sourcePath ) && !File.Exists( sourcePath ) )
            {
                Logger.AddWarning( string.Format( "The path {0} can't be compressed because doesn't exist", sourcePath ) );

                return null;
            }

            Stream outStream;
            GZipStream compressor;

            try
            {
                using ( FileStream fileStream = new FileStream( sourcePath, FileMode.Open ) )
                {
                    outStream = File.Create( Path.GetTempFileName( ) );

                    compressor = new GZipStream( outStream, CompressionMode.Compress );

                    fileStream.CopyTo( compressor );
                }
            }
            catch ( Exception e )
            {
                Logger.AddException( e );
                return null;
            }

            return outStream;
        }

        public static void Decompress( string targetPath, Stream stream )
        {
            if ( string.IsNullOrEmpty( targetPath ) )
                return;

            string directory = Path.GetDirectoryName( targetPath );

            if ( !string.IsNullOrEmpty( directory ) )
                Directory.CreateDirectory( directory );

            GZipStream compressor = new GZipStream( stream, CompressionMode.Decompress );            

            try
            {
                //Decompress
                byte[ ] buffer = new byte[ stream.Length ];
                int count = compressor.Read( buffer, 0, buffer.Length );

                //Write to targetPath
                using ( FileStream outStream = new FileStream( targetPath, FileMode.Create ) )
                {
                    outStream.Write( buffer, 0, count );
                }
            }
            catch ( Exception e )
            {
                Logger.AddException( e );
            }
        }
    }
}
