namespace SFTP.Wrapper.Requests
{
    public class DeleteFileRequest : IValidatable
    {
        public DeleteFileRequest(string file)
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