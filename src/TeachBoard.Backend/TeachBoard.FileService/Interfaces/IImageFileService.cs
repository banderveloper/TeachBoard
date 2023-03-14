using CloudinaryDotNet.Actions;

namespace TeachBoard.FileService.Interfaces;

public interface IImageFileService
{
    string GetImageLink(string publicId);
    Task<ImageUploadResult> UploadImageAsync(IFormFile file, string publicId);
    Task<DeletionResult> DeleteImageAsync(string publicId);
}