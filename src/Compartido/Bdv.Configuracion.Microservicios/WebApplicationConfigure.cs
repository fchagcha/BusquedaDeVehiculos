namespace Bdv.Configuracion.Microservicios
{
    public static class WebApplicationConfigure
    {
        public static WebApplication ConfigurePipeline(this WebApplication app, IConfiguration configuration)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Servicios.Api v1"));
            }

            app.UseExceptionless(configuration);

            app.UseCustomExceptionMiddleware();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseCors("CorsPolicy");

            app.MapControllers();

            return app;
        }
    }
}