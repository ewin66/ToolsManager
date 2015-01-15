using System.IO;
using System.ServiceModel;

namespace ToolsManager.DataServices.Server
{
    [MessageContract]
    public class UploadRequest
    {
        [MessageHeader( MustUnderstand = true )]
        public string FileName { get; set; }

        [MessageBodyMember( Order = 1 )]
        public Stream FileData { get; set; }
    }

    [MessageContract]
    public class UploadResponse
    {
        [MessageBodyMember( Order = 1 )]
        public bool Succeeded { get; set; }
    }

    [MessageContract]
    public class DownloadRequest
    {
        [MessageHeader( MustUnderstand = true )]
        public string FileName { get; set; }
    }

    [MessageContract]
    public class DownloadResponse
    {
        [MessageBodyMember( Order = 1 )]
        public Stream FileData { get; set; }
    }

    [ServiceContract]
    public interface IFileUploadService
    {
        [OperationContract]
        UploadResponse UploadFile( UploadRequest request );

        [OperationContract]
        DownloadResponse DownloadFile( DownloadRequest request );

        [OperationContract]
        void DeleteFile( string FileName );

        [OperationContract]
        bool FileExists( string FileName );
    }
}
