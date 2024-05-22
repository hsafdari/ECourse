using ECourse.Admin.Models;

namespace ECourse.Admin.Service.FilesManager
{
    public interface IFTPService
    {
        Task<ResponseDto> UploadAsync(string localFileAddress);
        void Delete(string remoteFileName);
    }
}
