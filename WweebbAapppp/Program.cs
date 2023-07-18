using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WweebbAapppp;
using WweebbAapppp.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WweebbAapppp.Hubs;
//using Microsoft.AspNetCore.Server.Kestrel.Https;
//using Microsoft.AspNetCore.Server.Kestrel;
using Microsoft.AspNetCore;
using System.Net;
using System.Net.WebSockets;
//using Microsoft.Extensions.Configuration;
//using Microsoft.AspNetCore.Cors;
using WweebbAapppp.MyWebSocket;
using WweebbAapppp.Models;
using System.IO;


var builder = WebHost.CreateDefaultBuilder(args); //WebApplication.CreateBuilder(args); 

builder.ConfigureKestrel(serverOptions =>
{
    string certificatePath = Path.Combine(Directory.GetCurrentDirectory(), "SecretSSL/certificate.pfx");
    string keyPassword = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "SecretSSL/password.txt"));

    //serverOptions.Listen(IPAddress.Loopback, 5185);
    serverOptions.Listen(IPAddress.Parse("192.168.0.102"), 5185);

    //serverOptions.Listen(IPAddress.Loopback, 7089, listenOptions =>
    serverOptions.Listen(IPAddress.Parse("192.168.0.102"), 7089, listenOptions =>
    {
        listenOptions.UseHttps(certificatePath, keyPassword); //, "certificatePassword");
    });
});

builder.ConfigureServices((hostContext, services) =>
{
    services.AddCors(options =>
    {
        options.AddPolicy("MyCorsPolicy", builder =>
        {
            builder.WithOrigins("https://192.168.0.102:44486")
            //builder.WithOrigins("https://localhost:44486")
            //builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
        });
    });
    services.AddHttpContextAccessor();
    services.AddControllers();
    services.AddDbContext<VehicleQuotesContext>(options =>
    options
                    .UseSqlServer(hostContext.Configuration.GetConnectionString("VehicleQuotesContext"))
                    .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()))
                    .EnableSensitiveDataLogging(), ServiceLifetime.Scoped
            );
    services.AddDataProtection();

    services
    .AddIdentityCore<IdentityUser>(options =>
    {
        // options.SignIn.RequireConfirmedAccount = false;
        options.SignIn.RequireConfirmedEmail = true;
        options.User.RequireUniqueEmail = true;
        options.Password.RequireDigit = false;
        options.Password.RequiredLength = 6;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireLowercase = false;
        options.Tokens.EmailConfirmationTokenProvider = "Default";
    })

    .AddEntityFrameworkStores<VehicleQuotesContext>()
    .AddDefaultTokenProviders();

    services.AddScoped<RefreshTokenService>();
    services.AddScoped<JwtService>();

    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = ValidationParameters.GetValidationParameters(hostContext.Configuration);
    });

    //services.AddMyWebSocketManager();

    services.AddSingleton<IConnectionManager, ConnectionManager>();
    services.AddTransient<MyWebSocketManager>();
    //services.AddTransient<MyWebSocketMiddleware>();


    services.AddSignalR();
});



builder.Configure((env, app) =>
{
    app.UseCors("MyCorsPolicy");

    if (!env.HostingEnvironment.IsDevelopment())
    {
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();

    app.UseWebSockets();
    
    //app.UseMyWebSocketManager("/ws");
    //app.UseMiddleware<MyWebSocketMiddleware>("/chatmessages");


    /*    app.Use(async (context, next) =>
        {
            if (context.Request.Path == "/ws")
            {
                if (context.WebSockets.IsWebSocketRequest)
                {
                    static async Task Echo(WebSocket webSocket)
                    {
                        var buffer = new byte[1024 * 4];
                        var receiveResult = await webSocket.ReceiveAsync(
                            new ArraySegment<byte>(buffer), CancellationToken.None);

                        while (!receiveResult.CloseStatus.HasValue)
                        {
                            await webSocket.SendAsync(
                                new ArraySegment<byte>(buffer, 0, receiveResult.Count),
                                receiveResult.MessageType,
                                receiveResult.EndOfMessage,
                                CancellationToken.None);

                            receiveResult = await webSocket.ReceiveAsync(
                                new ArraySegment<byte>(buffer), CancellationToken.None);
                        }

                        await webSocket.CloseAsync(
                            receiveResult.CloseStatus.Value,
                            receiveResult.CloseStatusDescription,
                            CancellationToken.None);
                    }

                    if (context.User.Identity.IsAuthenticated)
                    {
                        using var webSocket = await context.WebSockets.AcceptWebSocketAsync();
                        await Echo(webSocket);
                    }
                    else
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    }
                }
                else
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                }
            }
            else
            {
                await next(context);
            }

        });*/

    app.UseEndpoints(endpoints =>
    {

        endpoints.MapHub<MessageHub>("/message");

        endpoints.MapControllerRoute(
            name: "default",
            pattern: "{controller}/{action=Index}/{id?}"
        );

        endpoints.MapControllers();

        endpoints.MapFallbackToFile("index.html");
    });
});

/*builder.Services.AddCors(options =>
{
    options.AddPolicy("MyCorsPolicy", builder =>
    {
        builder.WithOrigins("https://localhost:44486")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
    });
});*/

//builder.Services.AddHttpContextAccessor();

/*builder.Services.AddControllers();

builder.Services.AddDbContext<VehicleQuotesContext>(options =>
options
                    .UseSqlServer(builder.Configuration.GetConnectionString("VehicleQuotesContext"))
                    .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()))
                    .EnableSensitiveDataLogging()
            );*/

//builder.Services.AddDataProtection();

/*builder.Services
    .AddIdentityCore<IdentityUser>(options =>
    {
        // options.SignIn.RequireConfirmedAccount = false;
        options.SignIn.RequireConfirmedEmail = true;
        options.User.RequireUniqueEmail = true;
        options.Password.RequireDigit = false;
        options.Password.RequiredLength = 6;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireLowercase = false;
        options.Tokens.EmailConfirmationTokenProvider = "Default";
    })
    .AddEntityFrameworkStores<VehicleQuotesContext>()
    .AddDefaultTokenProviders();*/

/*builder.Services.AddScoped<RefreshTokenService>();
builder.Services.AddScoped<JwtService>();*/

/*builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"], 
            ValidAudience = builder.Configuration["Jwt:Audience"], 
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });
*/
//builder.Services.AddSignalR();

var app = builder.Build();


//app.UseCors("MyCorsPolicy");


// Configure the HTTP request pipeline.
/*if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}*/

/*app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapHub<ChatHub>("/chatHub");*/


/*app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapControllers();

app.MapFallbackToFile("index.html");*/

app.Run();
