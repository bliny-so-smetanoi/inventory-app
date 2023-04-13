using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace InventoryApp.AppStart.ConfigureServices
{
    public class ConfigureServicesBase
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            var secrets = configuration.GetSection("Secrets");

            var key = Encoding.ASCII.GetBytes(secrets.GetValue<string>("JWTSecret"));
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                };
            });


            services.AddControllers();
            services.AddMemoryCache();
            services.AddRouting();
            services.AddHttpContextAccessor();
            services.AddResponseCaching();
        }

        private static byte[] StringToByteArray(string hex)
        {
            var numberChars = hex.Length;
            var bytes = new byte[numberChars / 2];

            for (var i = 0; i < numberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }

            return bytes;
        }

    }
}
