using InventoryApp.Hubs;
using Microsoft.AspNetCore.HttpOverrides;

namespace InventoryApp.AppStart.Configures
{
    public class ConfigureEndpoints
    {
        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();
            app.UseCors(x =>
            {
                x.AllowAnyHeader();
                x.AllowAnyMethod();
                x.SetIsOriginAllowed(origin => true);
                x.AllowCredentials();
            });

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseWebSockets();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}"
                );
                endpoints.MapHub<ClassroomHub>("/hub/classroom");
            });
        }
    }
}
