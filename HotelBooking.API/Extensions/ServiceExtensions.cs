using HotelBooking.API.Interfaces;
using HotelBooking.API.Services;
using HotelBooking.Db.Interfaces;
using HotelBooking.Db.Repositories;

namespace HotelBooking.API.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ICityRepository, CityRepository>();
        services.AddScoped<IHotelRepository, HotelRepository>();
        services.AddScoped<IRoomRepository, RoomRepository>();
        services.AddScoped<IBookingRepository, BookingRepository>();
        services.AddScoped<IReviewRepository, ReviewRepository>();
        services.AddScoped<IFeaturedDealRepository, FeaturedDealRepository>();
        services.AddScoped<IHotelImageRepository, HotelImageRepository>();
        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<ICityService, CityService>();
        services.AddScoped<IHotelService, HotelService>();
        services.AddScoped<IRoomService, RoomService>();
        services.AddScoped<IBookingService, BookingService>();
        services.AddScoped<IReviewService, ReviewService>();
        services.AddScoped<IFeaturedDealService, FeaturedDealService>();
        services.AddScoped<IHotelImageService, HotelImageService>();
        return services;
    }
}
