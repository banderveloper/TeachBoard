namespace TeachBoard.FileService.Interfaces;

public interface IFileService
{
    Task UploadFileAsync(IFormFile file, string name);

    Task Test();
}