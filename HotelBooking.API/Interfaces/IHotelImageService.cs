using HotelBooking.API.DTOs.Hotels;

namespace HotelBooking.API.Interfaces;

/// <summary>
/// Interface for image-related operations in the system.
/// </summary>
public interface IHotelImageService
{
    /// <summary>
    /// Retrieves all images for a hotel by its Id.
    /// </summary>
    /// <param name="hotelId">The Id of the hotel to search images for.</param>
    /// <returns>A collection of HotelImageResponse, null otherwise.</returns>
    Task<IEnumerable<HotelImageResponse>> GetImagesByHotelIdAsync(int hotelId);

    /// <summary>
    /// Creates an hotel image.
    /// </summary>
    /// <param name="request">The hotel image details.</param>
    /// <returns>HotelImageResponse.</returns>
    Task<HotelImageResponse> CreateImageAsync(HotelImageRequest request);

    /// <summary>
    /// Updates an image by its Id.
    /// </summary>
    /// <param name="id">The Id of the image to update.</param>
    /// <param name="request">The hotel image details.</param>
    /// <returns>HotelImageResponse.</returns>
    Task<HotelImageResponse> UpdateImageAsync(int id, HotelImageRequest request);

    /// <summary>
    /// Deletes an image by its Id.
    /// </summary>
    /// <param name="id">The Id of the image to delele.</param>
    /// <returns></returns>
    Task DeleteImageAsync(int id);
}
