using CloudinaryDotNet.Actions;

namespace TeachBoard.FileService.Services;

public interface IImageFileService
{
    Task<string> GetFileLink(string publicId);
    Task<ImageUploadResult> UploadAsync(IFormFile file);
    Task<DeletionResult> DeleteAsync(string publicId);
}