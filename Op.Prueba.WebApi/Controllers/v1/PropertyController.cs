using Microsoft.AspNetCore.Mvc;
using Op.Prueba.Application.Features.Property.Commands.CreatePropertyCommand;
using Op.Prueba.Application.Features.Property.Commands.DeletePropertyCommand;
using Op.Prueba.Application.Features.Property.Commands.UpdatePropertyCommand;
using Op.Prueba.Application.Features.Property.Queries.GetAllPropertiesQuery;
using Op.Prueba.Application.Features.Property.Queries.GetPropertyByIdQuery;
using Swashbuckle.AspNetCore.Annotations;

namespace OP.Prueba.WebAPI.Controllers;

/// <summary>
/// API para gestionar propiedades.
/// Permite realizar operaciones CRUD con soporte para CQRS + MediatR.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class PropertyController : BaseApiController
{
    /// <summary>
    /// Obtiene una lista de propiedades.
    /// </summary>
    /// <param name="query">Parámetros de consulta (paginación, filtros, etc).</param>
    /// <returns>Lista de propiedades con paginación.</returns>
    [HttpGet]
    [SwaggerOperation(
        Summary = "Obtener lista de propiedades",
        Description = "Devuelve una lista de propiedades con soporte de paginación y filtros."
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Lista obtenida correctamente.")]
    public async Task<IActionResult> Get([FromQuery] GetAllPropertiesQuery query)
    {
        var response = await Mediator.Send(query);
        return StatusCode((int)response.HttpCode, response);
    }

    /// <summary>
    /// Obtiene una propiedad por su ID.
    /// </summary>
    /// <param name="id">ID de la propiedad.</param>
    /// <returns>Propiedad encontrada o 404 si no existe.</returns>
    [HttpGet("{id}")]
    [SwaggerOperation(
        Summary = "Obtener propiedad por ID",
        Description = "Devuelve los datos de una propiedad específica mediante su ID."
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Propiedad encontrada.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Propiedad no encontrada.")]
    public async Task<IActionResult> GetById(string id)
    {
        var response = await Mediator.Send(new GetPropertyByIdQuery(id));
        return StatusCode((int)response.HttpCode, response);
    }

    /// <summary>
    /// Crea una nueva propiedad.
    /// </summary>
    /// <param name="command">Datos de la propiedad a crear.</param>
    /// <returns>ID de la propiedad creada.</returns>
    [HttpPost]
    [SwaggerOperation(
        Summary = "Crear propiedad",
        Description = "Permite registrar una nueva propiedad en el sistema."
    )]
    [SwaggerResponse(StatusCodes.Status201Created, "Propiedad creada correctamente.")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Error en los datos enviados.")]
    public async Task<IActionResult> Post([FromBody] CreatePropertyCommand command)
    {
        var response = await Mediator.Send(command);
        return StatusCode((int)response.HttpCode, response);
    }

    /// <summary>
    /// Actualiza una propiedad existente.
    /// </summary>
    /// <param name="id">ID de la propiedad a actualizar.</param>
    /// <param name="command">Datos actualizados de la propiedad.</param>
    /// <returns>Estado de la operación.</returns>
    [HttpPut("{id}")]
    [SwaggerOperation(
        Summary = "Actualizar propiedad",
        Description = "Permite modificar los datos de una propiedad existente."
    )]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Propiedad actualizada correctamente.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Propiedad no encontrada.")]
    public async Task<IActionResult> Put(string id, [FromBody] UpdatePropertyCommand command)
    {
        if (id != command.Id) return BadRequest();

        var response = await Mediator.Send(command);
        return StatusCode((int)response.HttpCode, response);
    }

    /// <summary>
    /// Elimina una propiedad por su ID.
    /// </summary>
    /// <param name="id">ID de la propiedad a eliminar.</param>
    /// <returns>Estado de la operación.</returns>
    [HttpDelete("{id}")]
    [SwaggerOperation(
        Summary = "Eliminar propiedad",
        Description = "Elimina una propiedad de forma permanente mediante su ID."
    )]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Propiedad eliminada correctamente.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Propiedad no encontrada.")]
    public async Task<IActionResult> Delete(string id)
    {
        var response = await Mediator.Send(new DeletePropertyCommand(id));
        return StatusCode((int)response.HttpCode, response);
    }
}