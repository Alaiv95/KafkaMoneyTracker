using Application.Dtos;
using Application.handlers.category.command.AddCategory;
using Application.handlers.category.command.DeleteCategory;
using Application.handlers.category.queries;
using Application.mediator.interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using WebApi.Extentions;

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
    public async Task<ActionResult<List<CategoryLookupDto>>> GetAll([FromQuery] CategoryGetAllRequestDto dto)
    {
        var getCategoriesQuery = new GetCategoriesQuery
        {
            IncludeCustom = dto.IncludeCustom
        };

        var categories = await _mediator.HandleRequest(getCategoriesQuery);

        return Ok(categories);
    }

    /// <summary>
    /// Add custom category
    /// </summary>
    /// <returns>Returns created category</returns>
    /// <response code="200">Success</response>
    /// <response code="400">Bad Request</response>
    [Authorize]
    [HttpPost("add")]
    [ProducesResponseType(typeof(CategoryLookupDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CategoryLookupDto>> Add([FromBody] AddCategoryRequestDto dto)
    {
        var addCategoryCommand = new AddCategoryCommand
        {
            Name = dto.Name,
            UserId = HttpContext.GetUserIdFromToken()
        };
        var category = await _mediator.HandleRequest(addCategoryCommand);

        return Ok(category);
    }
    
    /// <summary>
    /// Delete custom category
    /// </summary>
    /// <returns>Returns deleted category</returns>
    /// <response code="200">Success</response>
    /// <response code="400">Bad Request</response>
    /// <response code="404">Not found</response>
    [Authorize]
    [HttpDelete("delete/{id:guid}")]
    [ProducesResponseType(typeof(CategoryLookupDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CategoryLookupDto>> Add([FromRoute] Guid id)
    {
        var deleteCategoryCommand = new DeleteCategoryCommand()
        {
            CategoryId = id,
            UserId = HttpContext.GetUserIdFromToken()
        };
        var category = await _mediator.HandleRequest(deleteCategoryCommand);

        return Ok(category);
    }
}