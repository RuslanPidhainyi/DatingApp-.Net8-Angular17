using System;
using API.Helpers;
using API.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;

namespace API.Services;

public class PhotoService : IPhotoService
{
    private readonly Cloudinary _coudinary;
    public PhotoService(IOptions<CloudinarySettings> config)
    {
        var acc = new Account(config.Value.CloudName, config.Value.ApiKey, config.Value.ApiSecret);

        _coudinary = new Cloudinary(acc);
    }


    public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file)
    {
        var uploadResult = new ImageUploadResult(); //zmienna - do przychowywania wynikow przesyłania obrazów

        //sprawdzimy czy mamy plik. Jezeli długosc pliku jest 0, to dzialamy na nim
        if (file.Length > 0)
        {
            using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face"), //pomocne wlasciwosci dla CSS. - te wlasiwosci zrobą nam konkretnego rozmiaru naszego obrazu i focus bedzie na twarze
                Folder = "da-net8"
            };

            uploadResult = await _coudinary.UploadAsync(uploadParams);
        }

        return uploadResult;
    }

    public async Task<DeletionResult> DeletePhotoAsync(string publicId)
    {
        var deleteParams = new DeletionParams(publicId);

        return await _coudinary.DestroyAsync(deleteParams);
    }
}
