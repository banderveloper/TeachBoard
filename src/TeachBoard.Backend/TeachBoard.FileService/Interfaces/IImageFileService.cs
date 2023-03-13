using CloudinaryDotNet.Actions;

namespace TeachBoard.FileService.Interfaces;

public interface IImageFileService
{
    Task<string> GetImageLinkAsync(string publicId);
    Task<ImageUploadResult> UploadAsync(IFormFile file);
    Task<DeletionResult> DeleteAsync(string publicId);
}