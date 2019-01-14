namespace SFTP.Wrapper.Configs
{
    public class SftpConfig
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Host) &&
                   !string.IsNullOrEmpty(UserName) &&
                   !string.IsNullOrEmpty(Password);
        }
    }
}