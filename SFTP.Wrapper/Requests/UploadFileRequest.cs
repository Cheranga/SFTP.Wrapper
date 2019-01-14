using System.IO;

namespace SFTP.Wrapper.Requests
{
    public class UploadFileRequest : IValidatable
    {
        public UploadFileRequest(Stream streamToUpload, string whereToUpload)
        {
            StreamToUpload = streamToUpload;
            WhereToUpload = whereToUpload;
        }

        public Stream StreamToUpload { get; }
        public string WhereToUpload { get; }

        public bool IsValid()
        {
            return StreamToUpload != null && !string.IsNullOrWhiteSpace(WhereToUpload);
        }
    }
}