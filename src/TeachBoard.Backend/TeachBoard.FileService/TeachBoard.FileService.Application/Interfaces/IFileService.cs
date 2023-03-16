using Microsoft.AspNetCore.Http;

namespace TeachBoard.FileService.Application.Interfaces;

public interface IFileService
{
    /// <summary>
    /// Download file from hosting
    /// </summary>
    /// <param name="fileName">File name</param>
    /// <returns>Byte array of downloaded file</returns>
    Task<byte[]> DownloadFileAsync(string fileName);
    
    /// <summary>
    /// Upload file to hosting
    /// </summary>
    /// <param name="file">File to upload</param>
    /// <param name="fileName">File cloud name</param>
    /// <returns>Bool value either uploaded</returns>
    Task<bool> UploadFileAsync(IFormFile file, string fileName);
}