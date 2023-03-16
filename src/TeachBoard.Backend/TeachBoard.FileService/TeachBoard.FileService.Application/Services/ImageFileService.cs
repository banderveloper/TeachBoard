using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using TeachBoard.FileService.Application.Configurations;
using TeachBoard.FileService.Application.Exceptions;
using TeachBoard.FileService.Application.Interfaces;

namespace TeachBoard.FileService.Application.Services;

public class ImageFileService : IImageFileService
{
    private readonly Cloudinary _cloudinary;
    private readonly ILogger<ImageFileService> _logger;

    public ImageFileService(ImageApiConfiguration configuration, ILogger<ImageFileService> logger)
    {
        _logger = logger;
        _cloudinary = new Cloudinary(new Account(configuration.CloudName, configuration.Key, configuration.Secret));
    }

    public string GetImageLink(string publicId)
    {
        var transformation = new Transformation().Width(500).Crop("scale");

        // generate the image URL using the public ID and transformation (if any)
        var imageUrl = _cloudinary.Api.UrlImgUp.Transform(transformation)
            .BuildUrl(publicId);

        return imageUrl;
    }

    public async Task<string> UploadImageAsync(IFormFile file, string publicId)
    {
        if (file.Length <= 0)
            throw new BadRequestApiException
            {
                ErrorCode = ErrorCode.FileEmpty,
                PublicErrorMessage = "Image is empty"
            };

        await using var stream = file.OpenReadStream();

        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(file.FileName, stream), // (nameOfTheFile, its content in stream);
            Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face"),
            PublicId = publicId
        };

        var uploadResult = await _cloudinary.UploadAsync(uploadParams);

        // If upload successful - return url of created image
        if (uploadResult.Error is null) 
            return uploadResult.Url.ToString();
        
        // If not successful 
        _logger.LogError(
            $"Cloudinary upload image error. Status code: {uploadResult.StatusCode}. Error description: {uploadResult.Error.Message}");
            
        throw new HostingErrorException
        {
            ErrorCode = ErrorCode.HostingBadResponse,
            PublicErrorMessage = "Image hosting returned bad status code at image uploading"
        };
    }

    public async Task<bool> DeleteImageAsync(string publicId)
    {
        var deletionParams = new DeletionParams(publicId);
        var deletionResult = await _cloudinary.DestroyAsync(deletionParams);

        // If deleted - return true
        if (deletionResult.Error is null) 
            return true;
        
        // If not ok - throw error
        _logger.LogError(
            $"Cloudinary delete image error. Deletion status code: {deletionResult.StatusCode}. Error description: {deletionResult.Error.Message}");

        throw new HostingErrorException
        {
            ErrorCode = ErrorCode.HostingBadResponse,
            PublicErrorMessage = "Image hosting returned bad status code at image deletion"
        };

    }
}