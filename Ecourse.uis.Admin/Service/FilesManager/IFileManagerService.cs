using ECourse.Admin.Models;
using Microsoft.AspNetCore.Components.Forms;

namespace ECourse.Admin.Service.FilesManager
{
    public interface IFileManagerService
    {
        public Task<ResponseDto> UploadFileAsync(IBrowserFile browserFile, string serviceName,string previousIcon);
        public Task<ResponseDto> UploadFileAsync(IList<IBrowserFile> browserFiles, string serviceName);
        public Task<ResponseDto> DeleteFileAsync(string fileName);
    }
}
