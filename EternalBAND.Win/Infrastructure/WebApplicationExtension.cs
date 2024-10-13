using EternalBAND.Api.Hubs;
using EternalBAND.Win.Middleware;
using Microsoft.Extensions.FileProviders;

namespace EternalBAND.Win.Infrastructure
{
    public static class WebApplicationExtension
    {
        public static void UseApplication(this WebApplication app, string rootPath)
        {
            app.UseMiddleware<ErrorHandlingMiddleware>();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger(u =>
                {
                    u.RouteTemplate = "swagger/{documentName}/swagger.json";
                });

                app.UseSwaggerUI(c =>
                {
                    c.RoutePrefix = "swagger";
                    c.SwaggerEndpoint(url: "/swagger/v1/swagger.json", name: "Bandbul");
                });
                app.UseMigrationsEndPoint();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                       Path.Combine(rootPath, "Css")),
                RequestPath = "/Css"
            });

            app.UseRouting();
            app.UseCors("AllowAnyOrigin");
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Anasayfa}/{id?}");
            app.MapControllers();
            if (!app.Environment.IsDevelopment())
            {
                app.MapRazorPages();
            }
            app.MapHub<ChatHub>("/chatHub");

            app.Run();
        }
    }
}
