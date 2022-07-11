using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace ServIT;

public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
    //readonly IApiVersionDescriptionProvider provider;

    // public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) => this.provider = provider;

    public void Configure(SwaggerGenOptions options)
    {
        /*foreach (var desc in provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(desc.GroupName, new OpenApiInfo()
            {
                Title = $"Park API {desc.GroupName}",
                //Version = desc.ApiVersion.ToString()
            });
        }*/

/*            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "JWT Authentication header using the Bearer scheme.\r\n\r\n Enter 'Bearer' [space] and then your token in the text below.\r\n\r\nExample: \"Bearer 12345absce\"",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });*/

/*            options.AddSecurityRequirement(new OpenApiSecurityRequirement()
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id="Bearer"
                    },
                    Scheme="oauth2",
                    Name="Bearer",
                    In = ParameterLocation.Header,
                },
                new List<string>()
            }
        });*/
        var commentFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var cmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, commentFile);
        options.IncludeXmlComments(cmlCommentsFullPath);
    }
}
