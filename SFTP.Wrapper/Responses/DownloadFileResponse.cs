using System.IO;

namespace SFTP.Wrapper.Responses
{
    public class DownloadFileResponse
    {
        public bool Status { get; set; }
        public string FileName { get; set; }
        public Stream Stream { get; set; }
    }
}