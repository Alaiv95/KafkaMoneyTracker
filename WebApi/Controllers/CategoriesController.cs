using Application.Dtos;
using Application.handlers.category.queries;
using Application.mediator.interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/categories/")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoriesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get list of categories
    /// </summary>
    /// <returns>Returns list of CategoryLookupDto</returns>
    /// <response code="200">Success</response>
    /// <response code="400">Bad Request</response>
    [HttpGet("get-all")]
    [ProducesResponseType(typeof(List<CategoryLookupDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Guid>> GetAll([FromQuery] CategoryGetAllRequestDto dto)
    {
        var getCategoriesQuery = new GetCategoriesQuery
        {
            IncludeCustom = dto.IncludeCustom
        };
        var categories = await _mediator.HandleRequest(getCategoriesQuery);

        return Ok(categories);
    }
}