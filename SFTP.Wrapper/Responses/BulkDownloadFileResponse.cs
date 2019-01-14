using System.Collections.Generic;
using System.Linq;

namespace SFTP.Wrapper.Responses
{
    public class BulkDownloadFileResponse
    {
        public BulkDownloadFileResponse(IEnumerable<DownloadFileResponse> files)
        {
            Files = files?.ToList() ?? new List<DownloadFileResponse>();
        }

        public List<DownloadFileResponse> Files { get; }
    }
}