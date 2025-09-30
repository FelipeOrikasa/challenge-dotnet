using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Mottu.Api.Data;
using Mottu.Api.Mappers; // Adicionamos o using para o MappingProfile
using Mottu.Api.Repositories;
using Mottu.Api.Repositories.Interfaces;
using Mottu.Api.Services;
using Mottu.Api.Services.Interfaces;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// --- 1. Configuração da Conexão com o Banco de Dados ---
var connectionString = builder.Configuration.GetConnectionString("OracleDb");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseOracle(connectionString));

// --- 2. Injeção de Dependência (DI) ---
// Adicionando os repositórios
builder.Services.AddScoped<IFilialRepository, FilialRepository>();
builder.Services.AddScoped<IPatioRepository, PatioRepository>();
builder.Services.AddScoped<IMotoRepository, MotoRepository>();
builder.Services.AddScoped<ISensorRepository, SensorRepository>(); // <- ADICIONADO
builder.Services.AddScoped<ILocalizacaoRepository, LocalizacaoRepository>(); // <- ADICIONADO

// Adicionando os serviços
builder.Services.AddScoped<IFilialService, FilialService>();
builder.Services.AddScoped<IPatioService, PatioService>();
builder.Services.AddScoped<IMotoService, MotoService>();
builder.Services.AddScoped<ISensorService, SensorService>(); // <- ADICIONADO
builder.Services.AddScoped<ILocalizacaoService, LocalizacaoService>(); // <- ADICIONADO

// --- 3. Configuração do AutoMapper ---
builder.Services.AddAutoMapper(typeof(MappingProfile));


// --- 4. Serviços Padrão da API ---
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// --- 5. Configuração do Swagger para Documentação ---
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Mottu API - Gerenciamento de Pátios",
        Description = "API RESTful para gerenciar Filiais, Pátios e o histórico de localização de Motos através de Sensores.",
        Contact = new OpenApiContact
        {
            Name = "Mottu",
            Email = "Mottu@mottu.com"
        }
    });

    // Usa o arquivo XML gerado para popular a documentação
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});


var app = builder.Build();

// Configuração do pipeline de requisições HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();