using System.Text;
using Microsoft.EntityFrameworkCore;
using Upgrader.Auth;
using Upgrader.Db;
using Upgrader.Features.Balance;

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
System.Console.OutputEncoding = Encoding.UTF8;
System.Console.InputEncoding = Encoding.UTF8;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(5445);
});

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowSpecificOrigin",
        policy =>
        {
            policy
                .WithOrigins(
                    "https://hh.tlabs.cc",
                    "http://localhost:5444"
                )
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
                .SetIsOriginAllowed(origin => true);
        }
    );
});

builder.Services.AddControllers();

builder.Services.AddDbContext<MyContext>();

builder.Services.AddScoped<BalanceService>();

var app = builder.Build();

app.UseMiddleware<AuthMiddleware>();

app.UseCors("AllowSpecificOrigin");

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<MyContext>();
    context.Database.Migrate();
    DataInitializer.Seed(services.GetService<IServiceProvider>()).Wait();
}

app.Run();