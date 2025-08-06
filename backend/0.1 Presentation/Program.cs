using Application.Helpers;
using Application.Providers;
using Application.Schemas.Profiles;
using Application.Services.Implementations;
using Application.Services.Interfaces;
using Azure.Identity;
using Domain.Repository;
using Infrastructure;
using Infrastructure.Providers;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Presentation.Middlewares;

var host = new HostBuilder()
    .ConfigureAppConfiguration(configBuilder =>
    {
        var keyVaultUri = new Uri("https://aoristo-key-vault.vault.azure.net/");
        configBuilder.AddAzureKeyVault(keyVaultUri, new DefaultAzureCredential());

        configBuilder.AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                     .AddEnvironmentVariables();
    })
    .ConfigureFunctionsWebApplication(worker =>
    {
        worker.UseMiddleware<ExceptionHandlingMiddleware>();
        worker.UseMiddleware<AuthorizationMiddleware>();
    })
    .ConfigureServices((context, services) =>
    {
        var configuration = context.Configuration;

        var connectionString = configuration["ConnectionStr"];

        services.AddDbContextPool<AqualinaAPIContext>(options =>
            options.UseSqlServer(connectionString));

        var jwtOptions = new JwtOptions
        {
            Key = configuration["JWTSecret"],
            Issuer = configuration["Jwt:Issuer"],
            Audience = configuration["Jwt:Audience"] 
        };
        services.AddSingleton(jwtOptions);



        services.AddSingleton<INotificationSender, FirebaseProvider>();

        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IApartmentRepository, ApartmentRepository>();
        services.AddScoped<IInvitationRepository, InvitationRepository>();
        services.AddScoped<IInvitationService, InvitationService>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddSingleton<IHashingService, HashingService>();
        services.AddScoped<ITokenRepository, TokenRepository>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IVehicleService, VehicleService>();
        services.AddScoped<IVehicleRepository, VehicleRepository>();
        services.AddScoped<IVehicleRequestService, VehicleRequestService>();
        services.AddScoped<IRequestRepository, RequestRepository>();
        services.AddScoped<IApartmentService, ApartmentService>();
        services.AddScoped<ITowerRepository, TowerRepository>();
        services.AddScoped<ITowerService, TowerService>();
        services.AddSingleton<IBlobStorageService>(sp =>
            new BlobStorageService(configuration["Azure:Storage:Blob:ServiceUri"]!));

        services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<UserProfile>();
            cfg.AddProfile<VehicleProfile>();
            cfg.AddProfile<TowerProfile>();
        });

        services.AddHttpContextAccessor();
    })
    .Build();

host.Run();
