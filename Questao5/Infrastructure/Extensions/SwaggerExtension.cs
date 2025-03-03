using Microsoft.OpenApi.Models;

namespace Questao5.Infrastructure.Extensions
{
    public static class SwaggerExtension
    {
        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ContaCorrente API", Version = "v1" });
            });
        }
        public static IApplicationBuilder UseApiDoc(this IApplicationBuilder services)
        {
            return services.UseSwagger()
                .UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ContaCorrente API v1");
                    c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
                });
        }
    }
}
