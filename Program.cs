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


builder.Services.AddScoped<UsuarioRepository>();
builder.Services.AddScoped<UsuarioService>();

builder.Services.AddScoped<TagRepository>();
builder.Services.AddScoped<TagService>();

builder.Services.AddScoped<TagTipoRepository>();
builder.Services.AddScoped<TagTipoService>();

builder.Services.AddScoped<ProdutoRepository>();
builder.Services.AddScoped<ProdutoService>();

builder.Services.AddScoped<CarrinhoRepository>();
builder.Services.AddScoped<CarrinhoService>();

builder.Services.AddScoped<EnderecoRepository>();
builder.Services.AddScoped<EnderecoService>();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseCors(MyCors);

app.MapOpenApi();

if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference();
}

app.UseRouting();

app.UseMiddleware<Auth>();

app.UseAuthorization();

app.MapControllers();
app.Run();
