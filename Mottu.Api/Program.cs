using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Mottu.Api.Data;
using Mottu.Api.Mappers;
using Mottu.Api.Repositories;
using Mottu.Api.Repositories.Implementations;
using Mottu.Api.Repositories.Interfaces;
using Mottu.Api.Services;
using Mottu.Api.Services.Implementations;
using Mottu.Api.Services.Interfaces;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Mottu.Api.MLModels;
using Mottu.Api.Models.DTOs.Request;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

// --- 1. Configuração da Conexão com o Banco de Dados ---
var connectionString = builder.Configuration.GetConnectionString("OracleDb");
builder.Services.AddDbContext<AppDbContext>(options =>
{
    if (!string.IsNullOrEmpty(connectionString))
    {
        options.UseOracle(connectionString);
    }
    else
    {
        options.UseInMemoryDatabase("InMemoryDb");
    }
});

// --- 2. Injeção de Dependência (DI) - Repositórios ---
builder.Services.AddScoped<IFilialRepository, FilialRepository>();
builder.Services.AddScoped<IPatioRepository, PatioRepository>();
builder.Services.AddScoped<IMotoRepository, MotoRepository>();
builder.Services.AddScoped<ISensorRepository, SensorRepository>();
builder.Services.AddScoped<ILocalizacaoRepository, LocalizacaoRepository>();
builder.Services.AddScoped<IEntregadorRepository, EntregadorRepository>();
builder.Services.AddScoped<ILocacaoRepository, LocacaoRepository>();

// --- 2. Injeção de Dependência (DI) - Services ---
builder.Services.AddScoped<IFilialService, FilialService>();
builder.Services.AddScoped<IPatioService, PatioService>();
builder.Services.AddScoped<ISensorService, SensorService>();
builder.Services.AddScoped<ILocalizacaoService, LocalizacaoService>();
builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<IEntregadorService, EntregadorService>();
builder.Services.AddScoped<IMotoService, MotoService>();
builder.Services.AddScoped<ILocacaoService, LocacaoService>();

// --- 3. Configuração do AutoMapper ---
builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

// --- 4. Serviços Padrão da API e Filtros de Resposta ---
builder.Services.AddControllers(options =>
{
    options.Filters.Add(new ProducesResponseTypeAttribute(typeof(ValidationProblemDetails), 400));
    options.Filters.Add(new ProducesResponseTypeAttribute(typeof(void), 401));
    options.Filters.Add(new ProducesResponseTypeAttribute(typeof(void), 403));
});
builder.Services.AddEndpointsApiExplorer();

// Delivery prediction service (ML.NET)
builder.Services.AddSingleton<DeliveryPredictionService>();

// --- 5. Configuração do Swagger para Documentação ---
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1", // Versão da API
        Title = "Mottu API - Gerenciamento de Pátios",
        Description = "API RESTful para gerenciar Filiais, Pátios e o histórico de localização de Motos através de Sensores.",
        Contact = new OpenApiContact
        {
            Name = "Mottu",
            Email = "Mottu@mottu.com"
        }
    });

    // Especifica explicitamente a versão do OpenAPI 3.0.1
    // Isso garante que o campo "openapi": "3.0.1" seja incluído no JSON gerado
    options.SwaggerGeneratorOptions.Servers.Clear();
    options.EnableAnnotations();
    
    // Configuração adicional para garantir que o OpenAPI seja gerado corretamente
    options.CustomSchemaIds(type => type.FullName);

    // Segurança por API Key
    var apiKeyScheme = new OpenApiSecurityScheme
    {
        Name = "X-API-KEY",
        Type = SecuritySchemeType.ApiKey,
        In = ParameterLocation.Header,
        Description = "API Key needed to access the endpoints. Use header 'X-API-KEY: {key}'"
    };
    options.AddSecurityDefinition("ApiKey", apiKeyScheme);
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { apiKeyScheme, new string[] { } }
    });

    // XML Comments
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
});

// --- 6. Health Checks, API Versioning, Authentication, etc. ---
builder.Services.AddHealthChecks();

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader(),
        new HeaderApiVersionReader("x-api-version")
    );
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var key = builder.Configuration.GetValue<string>("Jwt:Key") ?? "ChangeThisDevKey1234567890";
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
    };
});

// --- Constrói o app ---
var app = builder.Build();

// --- 6.5. Popular o banco de dados com dados iniciais ---
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    try
    {
        logger.LogInformation("Iniciando população do banco de dados...");
        
        // Verificar se há dados antigos incompatíveis
        var filiaisCount = await context.Filiais.CountAsync();
        var motosCount = await context.Motos.CountAsync();
        logger.LogInformation($"Dados atuais no banco: Filiais: {filiaisCount}, Motos: {motosCount}");
        
        // Se não há dados ou se quiser forçar, popula o banco
        // Para forçar a população mesmo com dados existentes, mude para forceSeed: true
        // ATENÇÃO: Se o banco retornar vazio, pode ser que haja dados antigos incompatíveis
        // Nesse caso, mude para forceSeed: true para forçar a população
        await DataSeeder.SeedAsync(context, forceSeed: (filiaisCount == 0 && motosCount == 0));
        
        // Verificar novamente após o seed
        var filiaisApos = await context.Filiais.CountAsync();
        var motosApos = await context.Motos.CountAsync();
        logger.LogInformation($"Dados após seed: Filiais: {filiaisApos}, Motos: {motosApos}");
        logger.LogInformation("População do banco de dados concluída com sucesso.");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Ocorreu um erro ao popular o banco de dados.");
    }
}

// --- 7. Pipeline da Aplicação ---
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mottu API v1");
        c.RoutePrefix = string.Empty; // abre Swagger na raiz
    });
}

app.MapHealthChecks("/health").AllowAnonymous();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
