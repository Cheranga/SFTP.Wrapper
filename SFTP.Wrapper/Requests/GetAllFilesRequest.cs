namespace SFTP.Wrapper.Requests
{
    public class GetAllFilesRequest : IValidatable
    {
        public GetAllFilesRequest(string directory)
        {
            Directory = directory;
        }

        public string Directory { get; }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(Directory);
        }
    }
}