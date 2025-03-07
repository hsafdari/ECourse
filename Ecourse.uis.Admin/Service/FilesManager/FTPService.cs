using ECourse.Admin.Models;
using FluentFTP;
using FluentFTP.Helpers;
using System;

namespace ECourse.Admin.Service.FilesManager
{
    public class FTPService : IFTPService
    {
        private readonly IConfiguration _Configuration;

        public string Domain { get; }
        private string FTPHost { get; set; }
        private string FTPUserName { get; set; }
        private string FTPPassword { get; set; }
        private int FTPPort { get; set; }
        public string RemoteRootFolder { get; set; }
        public string RootBrowseFile { get; private set; }

        public FTPService(IConfiguration configuration)
        {
            _Configuration = configuration;
            Domain = _Configuration["FTP:Domain"] ?? "";
            FTPHost = _Configuration["FTP:FTPHost"] ?? "";
            FTPUserName = _Configuration["FTP:FTPUserName"] ?? "";
            FTPPassword = _Configuration["FTP:FTPPassword"] ?? "";
            FTPPort = int.Parse(_Configuration["FTP:FTPPort"] ?? "21");
            RemoteRootFolder = _Configuration["FTP:RemoteRootFolder"] ?? "";
            RootBrowseFile = _Configuration["FTP:Rootbrowsefile"] ?? "";
        }
        public async Task<ResponseDto> Delete(string remoteFileName)
        {
            try
            {
                var client = new AsyncFtpClient(FTPHost, FTPUserName, FTPPassword, FTPPort);
                client.Config.RetryAttempts = int.Parse(_Configuration["FTP:RetryAttempts"] ?? "3");
                await client.AutoConnect();
                bool isFileExist = await client.FileExists(remoteFileName);
                if (isFileExist)
                {
                    await client.DeleteFile(remoteFileName);
                    return new ResponseDto()
                    {
                        IsSuccess = true,
                        Message = "Image Deleted Successfully!!!"                        
                    };
                }
                else
                {
                    return new ResponseDto()
                    {
                        IsSuccess = false,
                        Message = "Image Not Exist!!"
                    };
                }
            }
            catch (Exception ex)
            {
                return new ResponseDto()
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    Result = ex
                };
                throw;
            }
            
        }

        public async Task<ResponseDto> UploadAsync(string localFileAddress)
        {
            try
            {
                string NewFileName = Path.GetFileName(localFileAddress);
                string DirectoryFull = Path.GetDirectoryName(localFileAddress)??"/Uploads";
                int IndexRoot=DirectoryFull.LastIndexOf("wwwroot")+ "wwwroot".Length;
                if (IndexRoot == -1)
                {
                    //Error
                    new Exception("wwwroot does not exist");
                }
                string RemoteDirectoryName=DirectoryFull.Substring(IndexRoot).Replace("\\","/");
                string RemoteSite = RootBrowseFile + RemoteDirectoryName;
                RemoteDirectoryName = RemoteRootFolder + RemoteDirectoryName;
                var client = new AsyncFtpClient(FTPHost, FTPUserName, FTPPassword, FTPPort);
                client.Config.RetryAttempts = int.Parse(_Configuration["FTP:RetryAttempts"] ?? "3");                
                await client.AutoConnect();
                if (!await client.DirectoryExists(RemoteDirectoryName))
                {
                    await client.CreateDirectory(RemoteDirectoryName);
                }
                var result = await client.UploadFile(localFileAddress, $"{RemoteDirectoryName + "/" + NewFileName}");
                return new ResponseDto()
                {
                    IsSuccess = result.IsSuccess(),
                    Message = "Uploaded successfully",
                    Result = $"{"/"+ RemoteDirectoryName +"/"+ NewFileName};{Domain+"/"+ RemoteSite + "/" + NewFileName}"
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto()
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    Result = ex
                };
                throw;
            }

        }
    }
}
