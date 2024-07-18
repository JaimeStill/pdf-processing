using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Pdf.Api.Converters;

namespace Pdf.Api.Extensions;
public static class ConfigurationExtensions
{

    public static IMvcBuilder ConfigureApiServices(this WebApplicationBuilder builder) =>
        builder
            .Services
            .AddControllers()
            .AddJsonOptions(HttpJsonOptions);

    public static IServiceCollection ConfigureCorsServices(this WebApplicationBuilder builder) =>
        builder
            .Services
            .AddCors(options =>
                options.AddDefaultPolicy(policy =>
                    policy
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
                        .WithOrigins(
                            builder
                                .Configuration
                                .GetConfigArray("CorsOrigins")
                        )
                        .WithExposedHeaders(
                            "Access-Control-Allow-Origin",
                            "Access-Control-Allow-Credentials",
                            "Content-Disposition"
                        )
                )
            );

    static Action<JsonOptions> HttpJsonOptions => (JsonOptions options) =>
        JsonOptions(options.JsonSerializerOptions);

    static JsonSerializerOptions JsonOptions(JsonSerializerOptions options)
    {
        options.Converters.Add(new JsonStringEnumConverter());
        options.Converters.Add(new DateOnlyJsonConverter());
        options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.ReferenceHandler = ReferenceHandler.IgnoreCycles;

        return options;
    }

    static string[] GetConfigArray(this IConfiguration config, string section) =>
        config
            .GetSection(section)
            .Get<string[]>()
        ?? [];
}