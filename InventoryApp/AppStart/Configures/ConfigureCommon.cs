using InventoryApp.AppStart.ConfigureServices;

namespace InventoryApp.AppStart.Configures
{
    public class ConfigureCommon
    {
        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseResponseCaching();
            app.UseStaticFiles();
            // app.UseSwagger();
            // app.UseSwaggerUI(ConfigureServicesSwagger.SwaggerClientSettings);
            // app.UseSwaggerUI(ConfigureServicesSwagger.SwaggerAdminSettings);

        }
    }
}
