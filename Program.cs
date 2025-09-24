using Scalar.AspNetCore;
using ThorAPI.Repositories;
using ThorAPI.Services;

var builder = WebApplication.CreateBuilder(args);

var MyCors = "_myCors";
builder.Services.AddCors(opts =>
{
    opts.AddPolicy(MyCors, p =>
        p.WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod());
});

builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<UsuarioService>();
builder.Services.AddScoped<TagService>();
builder.Services.AddScoped<TagTipoService>();
builder.Services.AddScoped<ProdutoService>();
builder.Services.AddScoped<CarrinhoService>();
builder.Services.AddScoped<EnderecoService>();


var app = builder.Build();

app.UseCors("vite");

app.MapOpenApi();

if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseCors(MyCors);

app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<Auth>();

app.Run();