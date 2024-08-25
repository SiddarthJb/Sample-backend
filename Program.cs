using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Z1;
using Z1.Auth;
using Z1.Chat;
using Z1.Core;
using Z1.Core.Configurations;
using Z1.Core.Data;
using Z1.Match;
using Z1.Profiles;
using Z1.Core.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Description = "Bearer Authentication with JWT Token",
        Type = SecuritySchemeType.Http
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List<string>()
        }
    });
});
builder.Services.AddHttpContextAccessor();
builder.Services.ConfigureAppSettings(builder.Configuration);

builder.Services.AddDbContext<AppDbContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), sqlServerOptions =>
    {
        sqlServerOptions.UseNetTopologySuite();
    });
});
builder.Services.AddScoped<DbContext, AppDbContext>();
builder.Services.AddSingleton<IUserIdProvider, UserIdProvider>();

builder.Services.ConfigureAuthServices();
builder.Services.ConfigureProfileServices();
builder.Services.ConfigureMatchServices();
builder.Services.ConfigureChatServices();
builder.Services.AddAuthentication();
builder.Services.ConfigureAuthentication(builder.Configuration);

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddSignalR();
builder.Services.AddHostedService<Matcher>();
builder.Services.AddSingleton<IBlobService, AzureStorage>();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();
app.UseExceptionHandler();
// custom jwt auth middleware
app.UseMiddleware<JwtMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<MatchHub>("/MatchHub").RequireAuthorization();

app.Run();
