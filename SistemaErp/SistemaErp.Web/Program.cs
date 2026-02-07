using Microsoft.EntityFrameworkCore;
using SistemaErp.Aplicacion.Servicio;
using SistemaErp.Dominio.Interfaces;
using SistemaErp.Infraestructura.Data;
using SistemaErp.Infraestructura.Repositorio;
using System;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();


builder.Services.AddScoped<StoredProcedureHelper>(sp =>
    new StoredProcedureHelper(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

builder.Services.AddScoped<IMovimientoRepositorio, MovimientoRepositorio>();
builder.Services.AddScoped<MovimientoServicio>();




var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Movimiento}/{action=Index}/{id?}");

app.Run();
