using System.Threading.Tasks;
using SFTP.Wrapper.Requests;
using SFTP.Wrapper.Responses;

namespace SFTP.Wrapper
{
    public interface ISftpManager
    {
        Task<ResultStatus<GetAllFilesResponse>> GetAllFilesAsync(GetAllFilesRequest request);
        Task<ResultStatus<DownloadFileResponse>> DownloadFileAsync(DownloadFileRequest request);
        Task<ResultStatus<BulkDownloadFileResponse>> BulkDownloadFilesAsync(BulkDownloadFileRequest request);
        Task<ResultStatus<DeleteFileResponse>> DeleteFileAsync(DeleteFileRequest request);
        Task<ResultStatus<DeleteDirectoryResponse>> DeleteDirectoryAsync(DeleteDirectoryRequest request);
        Task<ResultStatus<UploadFileResponse>> UploadFileAsync(UploadFileRequest request);
    }
}