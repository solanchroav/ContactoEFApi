using Microsoft.EntityFrameworkCore;
using contactoEFAPI.Models;
using contactoEFAPI.Controllers;
using contactoEFAPI.Models.Dto;
using contactoEFAPI.Models.Request;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<RecursosContext>();
builder.Services.AddTransient<ContactosController>();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();


app.MapGet("/", () => "Api Contacto!");

app.MapGet("/contacts", async (RecursosContext context) =>
{
    await context.Contactos.ToListAsync();
});

app.MapGet("/contact/{id}", async (int id, RecursosContext context) =>
{
    var contact = await context.Contactos.FindAsync(id);

    return contact != null ? Results.Ok(contact) : Results.NotFound();
});

app.MapGet("/contact/{email}", async (string email, RecursosContext context) =>
{
    var contact = await context.Contactos.Where(t => t.Email == email).FirstOrDefaultAsync();

    return contact != null ? Results.Ok(context) : Results.NotFound();
});

app.MapGet("/contact/{telefono}", async (int telefono, RecursosContext context) =>
{
    var contact = await context.Contactos.Where(t => t.Telefono == telefono).FirstOrDefaultAsync();

    return contact != null ? Results.Ok(context) : Results.NotFound();
});

app.MapPost("/contact/buscar", async (ContactoRequest contacto, RecursosContext context) =>
{
    //Im using a post method cuz a minimal API doesnt allow bind complex objects from query string values
    var contact = await context.Contactos.Where(t => t.Direccion.Contains($"{contacto.Provincia + contacto.Ciudad}")).ToListAsync();
        return Results.Ok(context);
});

app.MapPost("/createContact", async (ContactoDto contacto, RecursosContext context) =>
{
    var contactEntity = new Contacto
    {
        Nombre = contacto.Nombre,
        Empresa = contacto.Empresa,
        Email = contacto.Email,
        FechaNacimiento = DateTime.TryParse(contacto.FechaNacimiento, out DateTime datetime)? datetime: null,
        Telefono = contacto.Telefono,
        Direccion = contacto.Direccion,
        ImagenPerfil = contacto.ImagenPerfil

    };
    
    context.Contactos.Add(contactEntity);
    await context.SaveChangesAsync();

    Response response = new Response
    {
        status = true,
        body = $"El Usuario con nombre {contacto.Nombre} ha sido creado satisfactoriamente"
    };

 
    return Results.Created(response.body, contacto);
  

});

app.MapDelete("/deleteContact", async (int id, RecursosContext context) =>
{
    var response = new Response();

    if (await context.Contactos.FindAsync(id) is Contacto contactById)
    {
        context.Contactos.Remove(contactById);
        await context.SaveChangesAsync();

        response = new Response
        {
            status = true,
            body = $"El Usuario con id {id} ha sido borrado satisfactoriamente"
        };

        return Results.Ok(response);
    }


    response = new Response
    {
        status = false,
        body = $"El Usuario con id {id} no se ha encontrado"
    };

    return Results.NotFound(response);
   
});

app.MapPut("/updateContact", async (int id, ContactoDto contacto, RecursosContext context) =>
{
    var response = new Response();
    var contactById = await context.Contactos.FindAsync(id);

    if (contactById is null)
    {
        response = new Response
        {
            status = false,
            body = $"El Usuario con id {id} no existe"
        };
        return Results.NotFound(response);
    }


    contactById.Nombre = string.IsNullOrEmpty(contacto.Nombre) ? contactById.Nombre : contacto.Nombre;
    contactById.Empresa = string.IsNullOrEmpty(contacto.Empresa) ? contactById.Empresa : contacto.Empresa;
    contactById.Email = string.IsNullOrEmpty(contacto.Email) ? contactById.Email : contacto.Email;
    contactById.FechaNacimiento = string.IsNullOrEmpty(contacto.FechaNacimiento) ? contactById.FechaNacimiento : DateTime.TryParse(contacto.FechaNacimiento, out DateTime datetime) ? datetime : contactById.FechaNacimiento;
    contactById.Telefono = contacto.Telefono == null ? contactById.Telefono : contacto.Telefono;
    contactById.Direccion = string.IsNullOrEmpty(contacto.Direccion) ? contactById.Direccion : contacto.Direccion;
    contactById.ImagenPerfil = string.IsNullOrEmpty(contacto.ImagenPerfil) ? contactById.ImagenPerfil : contacto.ImagenPerfil;

    context.Contactos.Update(contactById);
    await context.SaveChangesAsync();


    response = new Response
    {
        status = true,
        body = $"El Usuario con id {id} ha sido actualizado satisfactoriamente"
    };

    return Results.Created(response.body, contacto);

});


app.Run();
