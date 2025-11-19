using AutoMapper;
using FluentValidation.AspNetCore;
using Logicfy.Data;
using Logicfy.Data.Repositories;
using Logicfy.Data.Repositories.Interfaces;
using Logicfy.Data.UnitOfWork;
using Logicfy.Models;
using Logicfy.Services;
using Logicfy.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


// IDENTITY
builder.Services.AddIdentity<Kullanici, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// ------------------------------------------------------
// 1) SERILOG
// ------------------------------------------------------
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/logicfy_log_.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// ------------------------------------------------------
// 2) DB CONTEXT
// ------------------------------------------------------
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ------------------------------------------------------
// 3) CONTROLLERS + JSON
// ------------------------------------------------------
builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling =
            Newtonsoft.Json.ReferenceLoopHandling.Ignore;

        options.SerializerSettings.ContractResolver =
            new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();
    });

// ------------------------------------------------------
// 4) AUTOMAPPER
// ------------------------------------------------------
builder.Services.AddAutoMapper(typeof(Program));

// ------------------------------------------------------
// 5) FLUENT VALIDATION
// ------------------------------------------------------
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();

// ------------------------------------------------------
// 6) CORS
// ------------------------------------------------------
builder.Services.AddCors(options =>
{
    options.AddPolicy("LogicfyCors", policy =>
    {
        policy.WithOrigins(
            "http://localhost:3000",
            "http://localhost:5173",
            "http://localhost:5174"
        )
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });
});

// ------------------------------------------------------
// 7) JWT + COOKIE AUTH (HİBRİD ÇÖZÜM)
// ------------------------------------------------------
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;   // ⭐ JWT varsayılan
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;       // ⭐ JWT varsayılan
})
.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "denizsoftyazilim.com",
        ValidAudience = "denizsoft_yazilim",
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes("DenizsoftTayfun:)DenizsoftTayfun:)DenizsoftTayfun:)DenizsoftTayfun:)DenizsoftTayfun:)"))
    };
})
.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
{
    options.Cookie.Name = ".DSPERPAuthToken";
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromHours(3500);
    options.LoginPath = "/Login";
    options.SlidingExpiration = true;
});

// ------------------------------------------------------
// 8) AUTHORIZATION
// ------------------------------------------------------
builder.Services.AddAuthorization();

// ------------------------------------------------------
// 9) SWAGGER
// ------------------------------------------------------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Logicfy API",
        Version = "v1",
        Description = "Logicfy öğrenim platformu API dokümantasyonu"
    });

    // Swagger için JWT desteği ekle
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Description = "JWT Bearer token",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    c.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() }
    });
});

// ------------------------------------------------------
// 10) REPOSITORY & UNIT OF WORK
// ------------------------------------------------------
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IProgramlamaDiliService, ProgramlamaDiliService>();
builder.Services.AddScoped<IUniteService, UniteService>();
builder.Services.AddScoped<IKisimService, KisimService>();
builder.Services.AddScoped<IDersService, DersService>();
builder.Services.AddScoped<ISoruService, SoruService>();
builder.Services.AddScoped<IKullaniciService, KullaniciService>();
builder.Services.AddScoped<IKullaniciProgressService, KullaniciProgressService>();
builder.Services.AddScoped<IKullaniciLearningPathService, KullaniciLearningPathService>();

builder.Services.AddSingleton<JwtTokenHelper>();

// ------------------------------------------------------
// 11) PIPELINE
// ------------------------------------------------------
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();
app.UseCors("LogicfyCors");

app.UseHttpsRedirection();

// ⭐ Doğru sıra
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
