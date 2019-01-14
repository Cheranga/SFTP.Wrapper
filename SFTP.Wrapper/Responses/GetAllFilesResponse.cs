using System.Collections.Generic;
using System.Linq;
using SFTP.Wrapper.Models;

namespace SFTP.Wrapper.Responses
{
    public class GetAllFilesResponse
    {
        public GetAllFilesResponse(IEnumerable<SftpFileInformation> files)
        {
            Files = files?.ToList() ?? new List<SftpFileInformation>();
        }

        public List<SftpFileInformation> Files { get; }
    }
}