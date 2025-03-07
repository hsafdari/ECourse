using ECourse.Admin.Models;

namespace ECourse.Admin.Service.FilesManager
{
    public interface IFTPService
    {
        Task<ResponseDto> UploadAsync(string localFileAddress);
        Task<ResponseDto> Delete(string remoteFileName);
    }
}
