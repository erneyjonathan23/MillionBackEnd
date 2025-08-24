using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Op.Prueba.Application.Features.PropertyTrace.Commands.CreatePropertyTraceCommand;
using Op.Prueba.Application.Features.PropertyTrace.Commands.DeletePropertyTraceCommand;
using Op.Prueba.Application.Features.PropertyTrace.Commands.UpdatePropertyTraceCommand;
using Op.Prueba.Application.Features.PropertyTrace.Queries.GetAllPropertyTracesQuery;
using Op.Prueba.Application.Features.PropertyTrace.Queries.GetPropertyTraceByIdQuery;
using OP.Prueba.WebAPI.Controllers;

namespace OP.Prueba.WebAPI.Controllers;

/// <summary>
/// API para gestionar trazas de propiedades (PropertyTrace).
/// Permite realizar operaciones CRUD con soporte para CQRS + MediatR.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class PropertyTracesController : BaseApiController
{
    /// <summary>
    /// Obtiene una lista de trazas de propiedades.
    /// </summary>
    /// <param name="query">Parámetros de consulta (paginación, filtros, etc).</param>
    /// <returns>Lista de trazas de propiedades con paginación.</returns>
    [HttpGet]
    [SwaggerOperation(
        Summary = "Obtener lista de trazas de propiedades",
        Description = "Devuelve una lista de trazas de propiedades con soporte de paginación y filtros."
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Lista obtenida correctamente.")]
    public async Task<IActionResult> Get([FromQuery] GetAllPropertyTracesQuery query)
    {
        var response = await Mediator.Send(query);
        return StatusCode((int)response.HttpCode, response);
    }

    /// <summary>
    /// Obtiene una traza de propiedad por su ID.
    /// </summary>
    /// <param name="id">ID de la traza de propiedad.</param>
    /// <returns>Traza encontrada o 404 si no existe.</returns>
    [HttpGet("{id}")]
    [SwaggerOperation(
        Summary = "Obtener traza de propiedad por ID",
        Description = "Devuelve los datos de una traza de propiedad específica mediante su ID."
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Traza encontrada.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Traza no encontrada.")]
    public async Task<IActionResult> GetById(string id)
    {
        var response = await Mediator.Send(new GetPropertyTraceByIdQuery(id));
        return StatusCode((int)response.HttpCode, response);
    }

    /// <summary>
    /// Crea una nueva traza de propiedad.
    /// </summary>
    /// <param name="command">Datos de la traza de propiedad a crear.</param>
    /// <returns>ID de la traza creada.</returns>
    [HttpPost]
    [SwaggerOperation(
        Summary = "Crear traza de propiedad",
        Description = "Permite registrar una nueva traza de propiedad en el sistema."
    )]
    [SwaggerResponse(StatusCodes.Status201Created, "Traza creada correctamente.")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Error en los datos enviados.")]
    public async Task<IActionResult> Post([FromBody] CreatePropertyTraceCommand command)
    {
        var response = await Mediator.Send(command);
        return StatusCode((int)response.HttpCode, response);
    }

    /// <summary>
    /// Actualiza una traza de propiedad existente.
    /// </summary>
    /// <param name="id">ID de la traza a actualizar.</param>
    /// <param name="command">Datos actualizados de la traza.</param>
    /// <returns>Estado de la operación.</returns>
    [HttpPut("{id}")]
    [SwaggerOperation(
        Summary = "Actualizar traza de propiedad",
        Description = "Permite modificar los datos de una traza de propiedad existente."
    )]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Traza actualizada correctamente.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Traza no encontrada.")]
    public async Task<IActionResult> Put(string id, [FromBody] UpdatePropertyTraceCommand command)
    {
        if (id != command.Id) return BadRequest();

        var response = await Mediator.Send(command);
        return StatusCode((int)response.HttpCode, response);
    }

    /// <summary>
    /// Elimina una traza de propiedad por su ID.
    /// </summary>
    /// <param name="id">ID de la traza a eliminar.</param>
    /// <returns>Estado de la operación.</returns>
    [HttpDelete("{id}")]
    [SwaggerOperation(
        Summary = "Eliminar traza de propiedad",
        Description = "Elimina una traza de propiedad de forma permanente mediante su ID."
    )]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Traza eliminada correctamente.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Traza no encontrada.")]
    public async Task<IActionResult> Delete(string id)
    {
        var response = await Mediator.Send(new DeletePropertyTraceCommand(id));
        return StatusCode((int)response.HttpCode, response);
    }
}