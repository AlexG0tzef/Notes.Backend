using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Notes.Application;
using Notes.Application.Common.Mappings;
using Notes.Application.Interfaces;
using Notes.Application.Notes.Queries.GetNoteDetails;
using Notes.Application.Notes.Queries.GetNoteList;
using Notes.Persistence;
using Notes.WebAPI.Middleware;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

RegisterServices(builder.Services);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    try
    {
        var context = serviceProvider.GetRequiredService<NotesDbContext>();
        DbInitializer.Initialize(context);
    }
    catch (Exception ex)
    {

    }
}

Configure(app);

app.Run();


void RegisterServices(IServiceCollection services)
{
    services.AddAutoMapper(config =>
    {
        config.AddProfile(new AssemblyMappingProfile(Assembly.GetExecutingAssembly()));
        config.AddProfile(new AssemblyMappingProfile(typeof(INoteDbContext).Assembly));
    });
    services.AddApplication();
    services.AddPersistence(builder.Configuration);

    services.AddControllers();

    services.AddCors(options =>
    {
        options.AddPolicy("AllowAll", policy =>
        {
            policy.AllowAnyHeader();
            policy.AllowAnyMethod();
            policy.AllowAnyOrigin();
        });
    });
    services.AddAuthentication(config =>
    {
        config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer("Bearer", options => 
    {
        options.Authority = "https://localhost:7218";
        options.Audience = "NotesWebAPI";
        options.RequireHttpsMetadata = false;
    });
    services.AddSwaggerGen(config => 
    {
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        config.IncludeXmlComments(xmlPath);
    });
}

void Configure(WebApplication app)
{
    app.UseSwagger();
    app.UseSwaggerUI(config => 
    {
        config.RoutePrefix = string.Empty;
        config.SwaggerEndpoint("swagger/v1/swagger.json", "Note API");
    });
    app.UseCustomExceptionHandler();
    app.UseRouting();
    app.UseHttpsRedirection();
    app.UseCors("AllowAll");
    app.UseAuthentication();
    app.UseAuthorization();

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
    });
}
