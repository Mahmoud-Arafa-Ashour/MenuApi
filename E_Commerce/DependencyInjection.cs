using E_Commerce.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace E_Commerce;

public static class DependencyInjection
{
    public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        var conn = configuration.GetConnectionString("DefaultConnection") ??
            throw new InvalidOperationException("Connection String 'DefaultConnection' not found.");
        services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(conn));
        services.AddMappsterServices();
        services.AddAuthconfigServices(configuration);
        services.AddConfigServices();
        services.AddScoped<ICategoryServices, CategoryServices>();
        services.AddScoped<IEmailSender, EmailServices>();
        services.AddScoped<IAuthServices, AuthServices>();
        services.AddScoped<IUserServices, UserServices>();
        services.AddScoped<IItemServices, ItemServices>();
        services.AddScoped<IDiscountServices, DiscountServices>();
        services.AddScoped<IOfferServices, OfferServices>();
        services.AddScoped<IOfferItemServices, OfferItemServices>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddSingleton<IJwtProvidor, JwtProvidor>();
        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.Configure<EmailSettings>(configuration.GetSection(nameof(EmailSettings)));
        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));
        services.AddProblemDetails();
        services.AddHttpContextAccessor();
        //Add Bucket Rate Limiter
        services.AddRateLimiter(rateLimiterOptions =>
        {
            rateLimiterOptions.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            rateLimiterOptions.AddTokenBucketLimiter("Token", Options =>
            {
                Options.TokenLimit = 10;
                Options.QueueLimit = 2;
                Options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                Options.ReplenishmentPeriod = TimeSpan.FromSeconds(10);
                Options.TokensPerPeriod = 2;
                Options.AutoReplenishment = true;
            });
        });

        //Add Concurrency Rate Limiter
        services.AddRateLimiter(rateLimiterOptions => 
        {
            rateLimiterOptions.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            rateLimiterOptions.AddConcurrencyLimiter("Concurrency", options => 
            {
                options.PermitLimit = 5;
                options.QueueLimit = 2;
                options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
            });
        });
        return services;
    }
    public static IServiceCollection AddMappsterServices(this IServiceCollection services)
    {
        var mappingConfiguration = TypeAdapterConfig.GlobalSettings;
        mappingConfiguration.Scan(Assembly.GetExecutingAssembly());
        services.AddSingleton<IMapper>(new Mapper(mappingConfiguration));
        return services;
    }
    public static IServiceCollection AddConfigServices(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(Program).Assembly);
        services.AddFluentValidationAutoValidation();
        return services;
    }
    public static IServiceCollection AddAuthconfigServices(this IServiceCollection services , IConfiguration configuration)
    {
        services.AddIdentity<ApplicationUser, ApplicationRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
        services.AddTransient<IAuthorizationHandler, PermissionAttributeHandler>();
        services.AddTransient<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();
        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }
            )
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("xwTYBtjkzBnMf8ZYyk8Gck4a18gy2bN7")),
                    ValidIssuer = "Menu",
                    ValidAudience = "Menu Users"
                };
            });
        services.Configure<IdentityOptions>(Options =>
        {
            Options.Password.RequiredLength = 8;
            Options.SignIn.RequireConfirmedEmail = false;
        });
        return services;
    }
}
