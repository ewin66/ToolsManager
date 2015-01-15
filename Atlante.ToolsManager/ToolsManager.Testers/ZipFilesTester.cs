using System.IO;
using NUnit.Framework;
using Atlante.Common;

namespace ToolsManager.Testers
{
    [TestFixture]
    public class ZipFilesTester
    {
        [Test]
        public static void CompressFileTest( )
        {
            string sourceFilePath = Path.Combine( Directory.GetCurrentDirectory( ), @"DirectoryTest\FileTest1.txt" );
            string targetFilePath = Path.Combine( Path.GetTempPath( ), "FileTest1.txt" );

            if ( !File.Exists( sourceFilePath ) )
            {
                Assert.Fail( string.Format( "File {0} does not exists", sourceFilePath ) );
                return;
            }

            Stream outStream = null;
            try
            {
                outStream = ZipFiles.Compress( sourceFilePath );

                if ( outStream == null )
                {
                    Assert.Fail( "File cannot be compressed." );
                    return;
                }

                if ( File.Exists( targetFilePath ) )
                    File.Delete( targetFilePath );

                ZipFiles.DecompressFile( Path.GetTempPath( ), outStream );
            }
            finally
            {
                if ( outStream != null )
                    outStream.Close( );
            }

            Assert.AreEqual( Directory.GetFiles( Path.GetTempPath( ), "FileTest1.txt" ).Length, 1 );
        }

        [Test]
        public static void CompressDirectoryTest( )
        {
            string sourceDirectoryPath = Path.Combine( Directory.GetCurrentDirectory( ), @"DirectoryTest" );
            string targetDirectoryPath = Path.Combine( Path.GetTempPath( ), "DirectoryTest" );

            if ( !Directory.Exists( sourceDirectoryPath ) )
            {
                Assert.Fail( string.Format( "Directory {0} does not exists", sourceDirectoryPath ) );
                return;
            }

            Stream outStream = null;
            try
            {
                outStream = ZipFiles.Compress( sourceDirectoryPath );

                if ( outStream == null )
                {
                    Assert.Fail( "Directory cannot be compressed." );
                    return;
                }

                if ( Directory.Exists( targetDirectoryPath ) )
                    Directory.Delete( targetDirectoryPath, true );

                ZipFiles.DecompressDirectory( targetDirectoryPath, outStream );
            }
            finally
            {
                if ( outStream != null )
                    outStream.Close( );
            }

            Assert.AreEqual( Directory.Exists( targetDirectoryPath ), true );
            Assert.AreEqual( File.Exists( Path.Combine( targetDirectoryPath, "FileTest1.txt" ) ), true );
            Assert.AreEqual( File.Exists( Path.Combine( targetDirectoryPath, "FileTest2.txt" ) ), true );
            Assert.AreEqual( Directory.Exists( Path.Combine( targetDirectoryPath, "DirectoryLevel2" ) ), true );
            Assert.AreEqual( File.Exists( Path.Combine( targetDirectoryPath, "DirectoryLevel2", "FileTest3.txt" ) ), true );
        }
    }
}
