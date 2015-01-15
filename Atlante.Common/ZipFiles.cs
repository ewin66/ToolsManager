using System;
using System.IO;
using Ionic.Zip;

namespace Atlante.Common
{
    public static class ZipFiles
    {
        public static string FILE_EXTENSION = "zip";

        public static Stream Compress( string sourcePath )
        {
            sourcePath = FileUtilities.GetAbsolutePath( sourcePath );

            if ( File.Exists( sourcePath ) )
                return ZipFiles.CompressFile( sourcePath );
            else if ( Directory.Exists( sourcePath ) )
                return CompressDirectory( sourcePath );
            else
            {
                Logger.AddWarning( string.Format( "The source {0} can't be compressed because doesn't exist", sourcePath ) );
                return null;
            }
        }

        private static Stream CompressFile( string sourceFileName )
        {
            Stream outStream = new MemoryStream( );

            try
            {
                using ( ZipFile compressor = new ZipFile( ) )
                {
                    ZipEntry entry = compressor.AddFile( sourceFileName );
                    entry.FileName = Path.GetFileName( sourceFileName );

                    compressor.Save( outStream );
                }
            }
            catch ( Exception e )
            {
                Logger.AddException( e );
                return null;
            }

            outStream.Position = 0;

            return outStream;
        }

        private static Stream CompressDirectory( string directoryName )
        {
            Stream outStream = new MemoryStream( );

            try
            {
                using ( ZipFile compressor = new ZipFile( ) )
                {
                    compressor.AddDirectory( directoryName );

                    compressor.Save( outStream );
                }
            }
            catch ( Exception e )
            {
                Logger.AddException( e );
                return null;
            }

            outStream.Position = 0;

            return outStream;
        }

        public static void Decompress( string targetDirectoryName, string sourceFileName )
        {
            if ( !File.Exists( sourceFileName ) )
            {
                Logger.AddWarning( string.Format( "The file {0} can't be decompressed because doesn't exist", sourceFileName ) );
                return;
            }

            try
            {
                using ( ZipFile zip = ZipFile.Read( sourceFileName ) )
                {
                    // zip.FlattenFoldersOnExtract = true;
                    zip.ExtractExistingFile = ExtractExistingFileAction.OverwriteSilently;
                    zip.ExtractAll( targetDirectoryName );
                }
            }
            catch ( Exception e )
            {
                Logger.AddException( e );
            }
        }

        public static void DecompressFile( string targetDirectoryName, Stream stream )
        {
            if ( !FileUtilities.ForceCreateDirectory( targetDirectoryName ) )
                return;

            try
            {
                using ( ZipFile zip = ZipFile.Read( stream ) )
                {
                    zip.FlattenFoldersOnExtract = true;
                    zip.ExtractExistingFile = ExtractExistingFileAction.OverwriteSilently;
                    zip.ExtractAll( targetDirectoryName );
                }
            }
            catch ( Exception e )
            {
                Logger.AddException( e );
            }
        }

        public static void DecompressDirectory( string targetDirectoryName, Stream stream )
        {
            if ( !FileUtilities.ForceCreateDirectory( targetDirectoryName ) )
                return;

            try
            {
                using ( ZipFile zip = ZipFile.Read( stream ) )
                {
                    zip.ExtractExistingFile = ExtractExistingFileAction.OverwriteSilently;
                    zip.ExtractAll( targetDirectoryName );
                }
            }
            catch ( Exception e )
            {
                Logger.AddException( e );
            }
        }
    }
}
