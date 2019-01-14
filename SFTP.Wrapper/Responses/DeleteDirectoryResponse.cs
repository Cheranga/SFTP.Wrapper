namespace SFTP.Wrapper.Responses
{
    public class DeleteDirectoryResponse
    {
        public DeleteDirectoryResponse(string directory)
        {
            Directory = directory;
        }

        public string Directory { get; }
    }
}