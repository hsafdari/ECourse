using ECourse.Admin.Models;
using ECourse.Admin.Utility;
using Microsoft.AspNetCore.Components.Forms;

namespace ECourse.Admin.Service.FilesManager
{
    public class FileManagerService : IFileManagerService
    {
        private readonly IFTPService _FTPService;
        private readonly IHttpContextAccessor _HttpContext;
        private readonly IWebHostEnvironment _WebHostEnvironment;

        public FileManagerService(IFTPService fTPService, IHttpContextAccessor httpContext, IWebHostEnvironment webHostEnvironment)
        {
            _FTPService = fTPService;
            _HttpContext = httpContext;
            _WebHostEnvironment = webHostEnvironment;
        }

        public async Task<ResponseDto> DeleteFileAsync(string fileName)
        {
            switch (SD.UploadMode)
            {
                case FileUploadMode.FTP:
                    {
                        return await _FTPService.Delete(fileName);                        
                    }
                case FileUploadMode.AzureBlob:
                    {
                        break;

                    }
            }
            return new ResponseDto();
        }

        public async Task<ResponseDto> UploadFileAsync(IBrowserFile browserFile, string serviceName, string previousIcon)
        {
            if (!string.IsNullOrEmpty(previousIcon))
            {
                await DeleteFileAsync(previousIcon);
            }
            //string url = _HttpContext.HttpContext.Request.Host.Value;
            string path = $"{_WebHostEnvironment.WebRootPath}\\Uploads\\{serviceName}";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            path = $"{path}\\{Guid.NewGuid() + Path.GetExtension(browserFile.Name)}";
            await using FileStream fs = new(path, FileMode.Create);
            await browserFile.OpenReadStream().CopyToAsync(fs);
            fs.Dispose();
            switch (SD.UploadMode)
            {
                case FileUploadMode.FTP:
                    {
                        var result = await _FTPService.UploadAsync(path);
                        if (result.IsSuccess)
                        {
                            File.Delete(path);
                           
                        }
                        return result;

                    }
                case FileUploadMode.AzureBlob:
                    {
                        break;

                    }
            }
            return new ResponseDto();

        }

        public Task<ResponseDto> UploadFileAsync(IList<IBrowserFile> browserFiles, string serviceName)
        {
            throw new NotImplementedException();
        }
    }
}
