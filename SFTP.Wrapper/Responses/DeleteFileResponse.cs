namespace SFTP.Wrapper.Responses
{
    public class DeleteFileResponse
    {
        public DeleteFileResponse(string file)
        {
            File = file;
        }

        public string File { get; }
    }
}