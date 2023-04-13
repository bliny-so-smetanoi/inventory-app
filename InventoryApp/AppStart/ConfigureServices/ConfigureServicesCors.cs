namespace InventoryApp.AppStart.ConfigureServices
{
    public class ConfigureServicesCors
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            var origins = configuration.GetSection("Cors")?.GetSection("Origins")?.Value?.Split(',');
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                {
                    builder.AllowAnyHeader();
                    builder.AllowAnyMethod();
                    if (origins is not { Length: > 0 }) return;

                    if (origins.Contains("*"))
                    {
                        builder.AllowAnyHeader();
                        builder.AllowAnyMethod();
                        builder.SetIsOriginAllowed(host => true);
                        builder.AllowCredentials();
                    }
                    else
                    {
                        foreach (var origin in origins)
                        {
                            builder.WithOrigins(origin);
                        }
                    }
                });
            });
        }

    }
}
