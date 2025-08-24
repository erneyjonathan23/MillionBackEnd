using Microsoft.AspNetCore.Mvc;
using Op.Prueba.Application.Features.Owner.Commands.CreateOwnerCommand;
using Op.Prueba.Application.Features.Owner.Commands.DeleteOwnerCommand;
using Op.Prueba.Application.Features.Owner.Commands.UpdateOwnerCommand;
using Op.Prueba.Application.Features.Owner.Queries.GetAllOwnersQuery;
using Op.Prueba.Application.Features.Owner.Queries.GetOwnerByIdQuery;
using Swashbuckle.AspNetCore.Annotations;

namespace OP.Prueba.WebAPI.Controllers;

/// <summary>
/// API para gestionar propietarios (Owner).
/// Permite realizar operaciones CRUD con soporte para CQRS + MediatR.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class OwnerController : BaseApiController
{
    /// <summary>
    /// Obtiene una lista de propietarios.
    /// </summary>
    /// <param name="query">Parámetros de consulta (paginación, filtros, etc).</param>
    /// <returns>Lista de propietarios con paginación.</returns>
    [HttpGet]
    [SwaggerOperation(
        Summary = "Obtener lista de propietarios",
        Description = "Devuelve una lista de propietarios con soporte de paginación y filtros."
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Lista obtenida correctamente.")]
    public async Task<IActionResult> Get([FromQuery] GetAllOwnersQuery query)
    {
        var response = await Mediator.Send(query);
        return StatusCode((int)response.HttpCode, response);
    }

    /// <summary>
    /// Obtiene un propietario por su ID.
    /// </summary>
    /// <param name="id">ID del propietario.</param>
    /// <returns>Propietario encontrado o 404 si no existe.</returns>
    [HttpGet("{id}")]
    [SwaggerOperation(
        Summary = "Obtener propietario por ID",
        Description = "Devuelve los datos de un propietario específico mediante su ID."
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Propietario encontrado.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Propietario no encontrado.")]
    public async Task<IActionResult> GetById(string id)
    {
        var response = await Mediator.Send(new GetOwnerByIdQuery(id));
        return StatusCode((int)response.HttpCode, response);
    }

    /// <summary>
    /// Crea un nuevo propietario.
    /// </summary>
    /// <param name="command">Datos del propietario a crear.</param>
    /// <returns>ID del propietario creado.</returns>
    [HttpPost]
    [SwaggerOperation(
        Summary = "Crear propietario",
        Description = "Permite registrar un nuevo propietario."
    )]
    [SwaggerResponse(StatusCodes.Status201Created, "Propietario creado correctamente.")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Error en los datos enviados.")]
    public async Task<IActionResult> Post([FromBody] CreateOwnerCommand command)
    {
        var response = await Mediator.Send(command);
        return StatusCode((int)response.HttpCode, response);
    }

    /// <summary>
    /// Actualiza un propietario existente.
    /// </summary>
    /// <param name="id">ID del propietario a actualizar.</param>
    /// <param name="command">Datos actualizados del propietario.</param>
    /// <returns>Estado de la operación.</returns>
    [HttpPut("{id}")]
    [SwaggerOperation(
        Summary = "Actualizar propietario",
        Description = "Permite modificar los datos de un propietario existente."
    )]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Propietario actualizado correctamente.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Propietario no encontrado.")]
    public async Task<IActionResult> Put(string id, [FromBody] UpdateOwnerCommand command)
    {
        if (id != command.Id) return BadRequest();

        var response = await Mediator.Send(command);
        return StatusCode((int)response.HttpCode, response);
    }

    /// <summary>
    /// Elimina un propietario por su ID.
    /// </summary>
    /// <param name="id">ID del propietario a eliminar.</param>
    /// <returns>Estado de la operación.</returns>
    [HttpDelete("{id}")]
    [SwaggerOperation(
        Summary = "Eliminar propietario",
        Description = "Elimina un propietario de forma permanente mediante su ID."
    )]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Propietario eliminado correctamente.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Propietario no encontrado.")]
    public async Task<IActionResult> Delete(string id)
    {
        var response = await Mediator.Send(new DeleteOwnerCommand(id));
        return StatusCode((int)response.HttpCode, response);
    }
}