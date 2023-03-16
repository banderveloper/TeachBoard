namespace TeachBoard.FileService.Interfaces;

public interface IFileService
{
    Task<byte[]> DownloadFileAsync(string fileName);
    Task<bool> UploadFileAsync(IFormFile file, string name);
}