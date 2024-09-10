using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using minimal_api.Dominio.DTOs;
using minimal_api.Dominio.Entidades;
using minimal_api.Dominio.Interfaces;
using minimal_api.Dominio.ModelViews;
using minimal_api.Dominio.Sevicos;
using minimal_api.Infraestrutura.Db;

#region builder
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IAdministradorServices, AdministradorServices>();
builder.Services.AddScoped<IVeiculoServices, VeiculoServices>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DbContexto>(options =>
{
    options.UseMySql(
        builder.Configuration.GetConnectionString("mysql"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("mysql"))
    );
});

var app = builder.Build();
#endregion

#region Home
app.MapGet("/", () =>
    Results.Json(new Home())
);
#endregion

#region Administradores
app.MapPost("/administradores/login", ([FromBody] LoginDTO loginDTO, IAdministradorServices administradorServices) =>
{
    if (administradorServices.login(loginDTO) != null)
        return Results.Ok("Login realizado com sucesso!");
    else
        return Results.BadRequest("Dados incorretos!");
});
#endregion

#region Veiculos
app.MapPost("/veiculos", ([FromBody] VeiculoDTO veiculoDTO, IVeiculoServices veiculoServices) =>
{
    var veiculo = new Veiculo
    {
        Nome = veiculoDTO.Nome,
        Marca = veiculoDTO.Marca,
        Ano = veiculoDTO.Ano,
    };
    veiculoServices.Incluir(veiculo);
    return Results.Created($"/veiculo/{veiculo.Id}", veiculo);
});
#endregion

#region app
app.UseSwagger();

app.UseSwaggerUI();

app.Run();
#endregion