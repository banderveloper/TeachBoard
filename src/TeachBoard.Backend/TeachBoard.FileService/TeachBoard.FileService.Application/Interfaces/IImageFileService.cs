using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace TeachBoard.FileService.Application.Interfaces;

public interface IImageFileService
{
    /// <summary>
    /// Get image link from fileName
    /// </summary>
    /// <param name="fileName">File name</param>
    /// <returns>Image URL on hosting</returns>
    string GetImageLink(string fileName);
    
    /// <summary>
    /// Upload image to hosting
    /// </summary>
    /// <param name="file">File to upload</param>
    /// <param name="fileName">File name for storing</param>
    /// <returns>Uploaded image url</returns>
    Task<string> UploadImageAsync(IFormFile file, string fileName);
    
    /// <summary>
    /// Delete image from hosting by name
    /// </summary>
    /// <param name="fileName">File name to delete</param>
    /// <returns>Bool value, deleted or not</returns>
    Task<bool> DeleteImageAsync(string fileName);
}