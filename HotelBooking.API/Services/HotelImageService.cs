using HotelBooking.API.Common;
using HotelBooking.API.DTOs.Hotels;
using HotelBooking.API.Interfaces;
using HotelBooking.Db.Interfaces;
using HotelBooking.Db.Models;

namespace HotelBooking.API.Services;

public class HotelImageService : IHotelImageService
{
    private readonly IHotelImageRepository _hotelImageRepository;
    private readonly IHotelRepository _hotelRepository;
    private readonly ILogger<HotelImageService> _logger;

    public HotelImageService(
        IHotelImageRepository hotelImageRepository,
        IHotelRepository hotelRepository,
        ILogger<HotelImageService> logger)
    {
        _hotelImageRepository = hotelImageRepository;
        _hotelRepository = hotelRepository;
        _logger = logger;
    }

    public async Task<Result<IEnumerable<HotelImageResponse>>> GetImagesByHotelIdAsync(int hotelId)
    {
        var images = await _hotelImageRepository.GetByHotelIdAsync(hotelId);
        return Result<IEnumerable<HotelImageResponse>>.Success(images.Select(MapToResponse));
    }

    public async Task<Result<HotelImageResponse>> CreateImageAsync(HotelImageRequest request)
    {
        var hotel = await _hotelRepository.GetByIdAsync(request.HotelId);
        if (hotel == null)
        {
            return Result<HotelImageResponse>.NotFound($"Hotel with Id {request.HotelId} not found.");
        }

        var image = new HotelImage
        {
            HotelId = request.HotelId,
            ImageUrl = request.ImageUrl,
            Caption = request.Caption,
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow
        };

        await _hotelImageRepository.AddAsync(image);
        _logger.LogInformation("Image created for hotel {HotelId}", request.HotelId);
        return Result<HotelImageResponse>.Success(MapToResponse(image));
    }

    public async Task<Result<HotelImageResponse>> UpdateImageAsync(int id, HotelImageRequest request)
    {
        var image = await _hotelImageRepository.GetByIdAsync(id);
        if (image == null)
        {
            return Result<HotelImageResponse>.NotFound($"Image with Id {id} not found.");
        }
        image.ImageUrl = request.ImageUrl;
        image.Caption = request.Caption;
        image.UpdatedDate = DateTime.UtcNow;

        await _hotelImageRepository.UpdateAsync(image);
        return Result<HotelImageResponse>.Success(MapToResponse(image));
    }

    public async Task<Result> DeleteImageAsync(int id)
    {
        var image = await _hotelImageRepository.GetByIdAsync(id);
        if (image == null)
        {
            return Result.NotFound($"Image with Id {id} not found.");
        }
        await _hotelImageRepository.DeleteAsync(image);
        _logger.LogInformation("Image {ImageId} deleted", id);
        return Result.Success();
    }

    private HotelImageResponse MapToResponse(HotelImage image)
    {
        return new HotelImageResponse
        {
            Id = image.Id,
            ImageUrl = image.ImageUrl,
            Caption = image.Caption
        };
    }
}
