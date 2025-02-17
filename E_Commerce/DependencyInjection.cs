using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using System;
namespace E_Commerce
{
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
            services.AddScoped<IAuthServices, AuthServices>();
            services.AddScoped<IUserServices, UserServices>();
            services.AddSingleton<IJwtProvidor, JwtProvidor>();
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
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
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
}
