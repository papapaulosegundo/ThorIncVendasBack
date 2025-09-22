using Scalar.AspNetCore;
using ThorAPI.Repositories;
using ThorAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.AddScoped<UsuarioRepository>();
builder.Services.AddScoped<UsuarioService>();

builder.Services.AddScoped<TagRepository>();
builder.Services.AddScoped<TagService>();

builder.Services.AddScoped<TagTipoRepository>();
builder.Services.AddScoped<TagTipoService>();

builder.Services.AddScoped<ProdutoRepository>();
builder.Services.AddScoped<ProdutoService>();

builder.Services.AddScoped<EnderecoRepository>();
builder.Services.AddScoped<EnderecoService>();

var app = builder.Build();

app.MapOpenApi();

if (app.Environment.IsDevelopment()) {
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
