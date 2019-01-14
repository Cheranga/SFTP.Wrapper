namespace SFTP.Wrapper.Responses
{
    public class UploadFileResponse
    {
        public UploadFileResponse(string uploadedFile)
        {
            UploadedFile = uploadedFile;
        }

        public string UploadedFile { get; }
    }
}