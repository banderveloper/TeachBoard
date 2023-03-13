using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using TeachBoard.FileService.Configurations;
using TeachBoard.FileService.Interfaces;

namespace TeachBoard.FileService.Services;

public class ImageFileService : IImageFileService
{
    private readonly Cloudinary _cloudinary;

    public ImageFileService(ApiConfiguration configuration)
    {
        _cloudinary = new Cloudinary(new Account(configuration.CloudName, configuration.Key, configuration.Secret));
    }
    
    public async Task<string> GetImageLinkAsync(string publicId)
    {
        var transformation = new Transformation().Width(500).Crop("scale");
        
        // generate the image URL using the public ID and transformation (if any)
        string imageUrl = _cloudinary.Api.UrlImgUp.Transform(transformation)
            .BuildUrl(publicId);

        return imageUrl;
    }

    [HttpPost]
    public async Task<ImageUploadResult> UploadAsync(IFormFile file)
    {
        var uploadResult = new ImageUploadResult();

        if (file.Length <= 0) return uploadResult;

        await using var stream = file.OpenReadStream();

        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(file.FileName, stream), // (nameOfTheFile, its content in stream);
            Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face"),
            //Folder = "files", //  folder where it will be located on CloudinaryWebSite
            PublicId = "hello"
        };

        uploadResult = await _cloudinary.UploadAsync(uploadParams);

        return uploadResult;
    }

    public Task<DeletionResult> DeleteAsync(string publicId)
    {
        throw new NotImplementedException();
    }
}