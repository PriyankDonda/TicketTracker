using TicketTracker.Services;

namespace TicketTracker.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ITicketService, TicketService>();
        services.AddScoped<ICommentService, CommentService>();
        services.AddScoped<IAttachmentService, AttachmentService>();
        
        return services;
    }
}