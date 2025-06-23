using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Fiap.Hackatoon.Product.Application.Interfaces;
using DTO = Fiap.Hackatoon.Product.Application.DataTransferObjects;
using Fiap.Hackatoon.Product.Api.Filters;

namespace Fiap.Hackatoon.Product.Api.Controllers;

/// <summary>
/// Product controller
/// </summary>
[Route("products")]
public class ProductController(ILogger<ProductController> logger, IProductApplicationService productApplicationService) : BaseController(logger)
{
    private readonly IProductApplicationService _productApplicationService = productApplicationService;

    /// <summary>
    /// Criar um novo produto
    /// </summary>
    /// <param name="model">Objeto com as propriedades para criar um novo produto</param>
    /// <returns>Um objeto do produto criado</returns>
    [HttpPost]
    // [Authorize(Policy = Policies.SuperOrModerator)]
    [SkipUserFilter]
    [Produces("application/json")]
    [ProducesResponseType(typeof(DTO.Product), StatusCodes.Status200OK)]
    public async Task<object> Create([FromBody] DTO.Product model)
    {
        try
        {
            var product = await _productApplicationService.Add(model);
            return Ok(product);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Editar um produto
    /// </summary>
    /// <param name="model">Objeto com as propriedades para editar um produto</param>
    /// <returns>Um objeto do produto criado</returns>
    [HttpPut]
    // [Authorize(Policy = Policies.SuperOrModerator)]
    [SkipUserFilter]
    [Produces("application/json")]
    [ProducesResponseType(typeof(DTO.Product), StatusCodes.Status200OK)]
    public async Task<object> Update([FromBody] DTO.Product model)
    {
        try
        {
            var product = await _productApplicationService.Update(model);
            return Ok(product);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Obter um produto pelo Id
    /// </summary>
    /// <param name="id">Id do produto</param>
    /// <returns>Um objeto do produto</returns>
    [HttpGet("{id}")]
    // [Authorize(Policy = Policies.SuperOrModerator)]
    [SkipUserFilter]
    [Produces("application/json")]
    [ProducesResponseType(typeof(DTO.Product), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            var product = await _productApplicationService.GetById(id);

            return Ok(product);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Remover um produto pelo Id
    /// </summary>
    /// <param name="id">Id do produto</param>
    /// <returns>Status da operação</returns>
    [HttpDelete("{id}")]
    // [Authorize(Policy = Policies.SuperOrModerator)]
    [SkipUserFilter]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await _productApplicationService.Delete(id);

            return Ok("produto removido com sucesso.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}