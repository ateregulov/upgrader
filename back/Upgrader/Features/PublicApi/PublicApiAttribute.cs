using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using OrisAppBack.Other.Settings;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Upgrader.Features.PublicApi;

public class PublicApiAttribute : TypeFilterAttribute<PublicApiFilter>
{

}

public class PublicApiFilter : IAuthorizationFilter
{
    private const string PRIVATE_KEY_HEADER = "X-PrivateKey";
    private readonly string _privateKey;

    public PublicApiFilter(IOptions<AppSettings> appSettings)
    {
        _privateKey = appSettings.Value.SecuritySettings.PrivateKey;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (!context.HttpContext.Request.Headers.TryGetValue(PRIVATE_KEY_HEADER, out var privateKey))
        {
            context.Result = new UnauthorizedObjectResult("Private key is missing");
            return;
        }

        if (privateKey != _privateKey)
        {
            context.Result = new UnauthorizedObjectResult("Private key is invalid");
            return;
        }
    }
}

public class PublicApiOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (context.MethodInfo.GetCustomAttributes(typeof(PublicApiAttribute), true).Length == 0
        && context.MethodInfo.DeclaringType.GetCustomAttributes(typeof(PublicApiAttribute), true).Length == 0)
            return;

        operation.Parameters ??= [];

        operation.Parameters.Add(new OpenApiParameter
        {
            Name = "X-PrivateKey",
            In = ParameterLocation.Header,
            Required = true,
            Description = "Private key for authentication",
            Schema = new OpenApiSchema { Type = "string" }
        });
    }
}