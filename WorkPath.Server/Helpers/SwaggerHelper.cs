using Microsoft.OpenApi.Models;

namespace WorkPath.Server.Helpers;

public class SwaggerHelper
{
    public static OpenApiInfo ApiInformation => new OpenApiInfo
    {
        Version = "v1.1",
        Title = "WorkPath API",
        Description = "An ASP.NET Core Web API for managing WorkPath project",
        Contact = new OpenApiContact
        {
            Name = "Example Contact",
            Url = new Uri("https://example.com/contact")
        },
    };

    public static OpenApiSecurityScheme ApiSecurityDefinition => new OpenApiSecurityScheme
    {
        Description = $"JWT Authorization header using the Bearer scheme. \r\n\r\n " +
                      "Enter 'Bearer' [space] and then your token in the text input below." +
                      "\r\n\r\nExample: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
    };

    public static OpenApiSecurityRequirement ApiSecurityRequirement => new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,

            },
            new List<string>()
        }
    };
}