using ConvertFile.Api.Interfaces;
using ConvertFile.Api.Services;
using ConvertFile.Api.Services.Readers;
using ConvertFile.Api.Services.Converters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() 
    { 
        Title = "ConvertFile API", 
        Version = "v1",
        Description = "API para conversão de arquivos entre diferentes formatos"
    });
});

// Registrar Readers (Open/Closed Principle - fácil adicionar novos readers)
builder.Services.AddScoped<IFileReader, FixedPositionReader>();
builder.Services.AddScoped<IFileReader, DelimitedReader>();
builder.Services.AddScoped<IFileReader, JsonReader>();

// Registrar Writers
builder.Services.AddScoped<IFileWriter, FixedPositionWriter>();
builder.Services.AddScoped<IFileWriter, DelimitedWriter>();
builder.Services.AddScoped<IFileWriter, JsonWriter>();

// Registrar serviço principal (Dependency Inversion Principle)
builder.Services.AddScoped<IFileConverterService, FileConverterService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

// Tornar a classe Program acessível para testes
public partial class Program { }
