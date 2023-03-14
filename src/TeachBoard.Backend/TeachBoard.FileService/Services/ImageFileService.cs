using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using TeachBoard.FileService.Configurations;
using TeachBoard.FileService.Exceptions;
using TeachBoard.FileService.Interfaces;

namespace TeachBoard.FileService.Services;

public class ImageFileService : IImageFileService
{
    private readonly Cloudinary _cloudinary;

    public ImageFileService(ImageApiConfiguration configuration)
    {
        Console.WriteLine("IMage file service cloudname: " + configuration.CloudName);
        Console.WriteLine("IMage file service key: " + configuration.Key);
        Console.WriteLine("IMage file service secret: " + configuration.Secret);
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

    public async Task<ImageUploadResult> UploadImageAsync(IFormFile file, string publicId)
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

        return await _cloudinary.UploadAsync(uploadParams);
    }

    public async Task<DeletionResult> DeleteImageAsync(string publicId)
    {
        var deletionParams = new DeletionParams(publicId);
        var deletionResult = await _cloudinary.DestroyAsync(deletionParams);
        return deletionResult;
    }
}