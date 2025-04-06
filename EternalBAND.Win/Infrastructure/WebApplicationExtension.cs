using EternalBAND.Api.Hubs;
using EternalBAND.Win.Middleware;
using Microsoft.Extensions.FileProviders;

namespace EternalBAND.Win.Infrastructure
{
    public static class WebApplicationExtension
    {
        public static void UseApplication(this WebApplication app, WebApplicationBuilder builder)
        {
            var rootPath = builder.Environment.ContentRootPath;
            app.UseMiddleware<ErrorHandlingMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
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
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseStatusCodePagesWithRedirects("/{ErrorEndpoint}/{0}");
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                   Path.Combine(builder.Environment.ContentRootPath, "Css")),
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
            app.MapRazorPages();
            app.MapHub<ChatHub>("/chatHub");

            app.Run();
        }
    }
}
