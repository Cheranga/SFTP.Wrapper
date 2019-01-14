namespace SFTP.Wrapper.Requests
{
    public class DownloadFileRequest : IValidatable
    {
        public DownloadFileRequest(string file)
        {
            File = file;
        }

        public string File { get; }

        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(File);
        }
    }
}