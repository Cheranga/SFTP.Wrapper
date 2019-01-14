namespace SFTP.Wrapper.Requests
{
    public class DeleteDirectoryRequest : IValidatable
    {
        public DeleteDirectoryRequest(string directory)
        {
            Directory = directory;
        }

        public string Directory { get; }

        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Directory);
        }
    }
}