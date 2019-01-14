using System.Collections.Generic;
using System.Linq;

namespace SFTP.Wrapper.Requests
{
    public class BulkDownloadFileRequest : IValidatable
    {
        public BulkDownloadFileRequest(IEnumerable<DownloadFileRequest> requests)
        {
            Requests = requests;
        }

        public IEnumerable<DownloadFileRequest> Requests { get; }

        public bool IsValid()
        {
            return Requests != null && Requests.All(x => x.IsValid());
        }
    }
}