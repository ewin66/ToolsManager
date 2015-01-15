using System;
using System.Configuration;
using System.IO;
using System.ServiceModel;

namespace ToolsManager.DataServices.Server
{
    [ServiceBehavior( InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Single )]
    public class FileUploadService : IFileUploadService
    {
        private string _filesOutputPath;

        public FileUploadService( )
        {
            _filesOutputPath = ConfigurationManager.AppSettings[ "filesOutputPath" ];
        }

        public UploadResponse UploadFile( UploadRequest request )
        {
            string targetPath = Path.Combine( _filesOutputPath, request.FileName );

            string directory = Path.GetDirectoryName( targetPath );

            if ( !string.IsNullOrEmpty( directory ) )
                Directory.CreateDirectory( directory );

            try
            {
                const int bufferSize = 2048;
                var buffer = new byte[ bufferSize ];

                using ( FileStream outStream = new FileStream( targetPath, FileMode.Create, FileAccess.Write ) )
                {
                    int bytesRead = request.FileData.Read( buffer, 0, bufferSize );

                    while ( bytesRead > 0 )
                    {
                        outStream.Write( buffer, 0, bytesRead );
                        bytesRead = request.FileData.Read( buffer, 0, bufferSize );
                    }
                }

                return new UploadResponse( ) { Succeeded = true };
            }
            catch ( Exception )
            {
                return new UploadResponse( ) { Succeeded = false };
            }
        }

        public DownloadResponse DownloadFile( DownloadRequest request )
        {
            string filePath = Path.Combine( _filesOutputPath, request.FileName );

            var stream = new FileStream( filePath, FileMode.Open );

            OperationContext clientContext = OperationContext.Current;
            clientContext.OperationCompleted += new EventHandler( delegate( object sender, EventArgs args )
            {
                if ( stream != null )
                    stream.Dispose( );
            } );

            return new DownloadResponse( ) { FileData = stream };
        }

        public void DeleteFile( string fileName )
        {
            var filePath = Path.Combine( _filesOutputPath, fileName );

            if ( File.Exists( filePath ) )
                File.Delete( filePath );
        }

        public bool FileExists( string fileName )
        {
            var filePath = Path.Combine( _filesOutputPath, fileName );

            return File.Exists( filePath );
        }
    }
}



