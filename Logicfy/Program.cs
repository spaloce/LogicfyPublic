using AutoMapper;
using FluentValidation.AspNetCore;
using Logicfy.Data;
using Logicfy.Data.Repositories;
using Logicfy.Data.Repositories.Interfaces;
using Logicfy.Data.UnitOfWork;
using Logicfy.Services;
using Logicfy.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// ------------------------------------------------------
// 1) SERILOG (En üstte kurulmalı)
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

        // JSON içindeki camelCase kullanıcıya gider
        options.SerializerSettings.ContractResolver =
            new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();
    });

// ------------------------------------------------------
// 4) AUTOMAPPER
// ------------------------------------------------------
builder.Services.AddAutoMapper(typeof(Program)); // Profil dosyalarını otomatik bulur

// ------------------------------------------------------
// 5) FLUENT VALIDATION
// ------------------------------------------------------
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
// → Validator sınıflarını ekleyeceğiz (2. adımda DTO bölümünde)

// ------------------------------------------------------
// 6) CORS (Frontend ile haberleşme için)
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
// 7) SWAGGER
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
});

// ------------------------------------------------------
// 8) (OPSİYONEL) IDENTITY – Kimlik yapısı olacaksa
// ------------------------------------------------------
// builder.Services.AddIdentityCore<Kullanici>(options =>
// {
//     options.Password.RequireDigit = false;
//     options.Password.RequireNonAlphanumeric = false;
//     options.Password.RequireUppercase = false;
//     options.Password.RequireLowercase = false;
//     options.Password.RequiredLength = 6;
// })
// .AddEntityFrameworkStores<ApplicationDbContext>();


// ------------------------------------------------------
// 9) REPOSITORY & UNIT OF WORK KAYITLARI
// ------------------------------------------------------

// === REPOSITORY + UNIT OF WORK ===
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// === PROGRAMLAMA DİLİ ===
builder.Services.AddScoped<IProgramlamaDiliService, ProgramlamaDiliService>();

// === UNİTE ===
builder.Services.AddScoped<IUniteService, UniteService>();

// === KISIM ===
builder.Services.AddScoped<IKisimService, KisimService>();

// === DERS ===
builder.Services.AddScoped<IDersService, DersService>();

// === SORU ===
builder.Services.AddScoped<ISoruService, SoruService>();

// === KULLANICI ===
builder.Services.AddScoped<IKullaniciService, KullaniciService>();

// === PROGRESS MOTORU ===
builder.Services.AddScoped<IKullaniciProgressService, KullaniciProgressService>();



// ------------------------------------------------------
// UYGULAMA PIPELINE
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

app.UseAuthorization();

app.MapControllers();

app.Run();
