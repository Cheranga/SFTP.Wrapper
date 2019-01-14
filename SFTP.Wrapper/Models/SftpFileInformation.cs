namespace SFTP.Wrapper.Models
{
    public class SftpFileInformation
    {
        public SftpFileInformation(string name, string fullName, long length)
        {
            Name = name;
            FullName = fullName;
            Length = length;
        }

        public string Name { get; }
        public string FullName { get; }
        public long Length { get; }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(FullName) && Length > 0;
        }
    }
}