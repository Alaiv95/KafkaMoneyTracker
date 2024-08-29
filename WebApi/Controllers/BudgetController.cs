
using Application.Dtos;
using Application.handlers.budget.queries.models;
using Application.mappers;
using Application.mediator.interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Extentions;

namespace WebApi.Controllers;

[Route("api/budget/")]
[ApiController]
public class BudgetController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly BudgetMapper _budgetMapper;

    public BudgetController(IMediator mediator, BudgetMapper budgetMapper)
    {
        _mediator = mediator;
        _budgetMapper = budgetMapper;
    }

    /// <summary>
    /// Create the Budget for category during chosen period
    /// </summary>
    /// <returns>Returns Guid of created budget</returns>
    /// <response code="200">Success</response>
    /// <response code="400">Bad Request</response>
    [Authorize]
    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Guid>> AddBudget([FromBody] BudgetCreateDto dto)
    {
        var userId = HttpContext.GetUserIdFromToken();

        var createBudgetCommand = _budgetMapper.DtoToCommand(dto, userId);
        var createdBudgetId = await _mediator.HandleRequest(createBudgetCommand);

        return Ok(createdBudgetId);
    }

    /// <summary>
    /// Create the Budget for category during chosen period
    /// </summary>
    /// <returns>Returns GetBudgetListDto</returns>
    /// <response code="200">Success</response>
    /// <response code="204">No Content</response>
    /// <response code="400">Bad Request</response>
    [Authorize]
    [HttpGet("search")]
    [ProducesResponseType(typeof(BudgetListVm), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BudgetListVm>> SearchBudgets([FromQuery] BudgetGetListDto dto)
    {
        var userId = HttpContext.GetUserIdFromToken();

        var getBudgetQuery = _budgetMapper.DtoToQuery(dto, userId);
        var budgetList = await _mediator.HandleRequest(getBudgetQuery);

        return (budgetList == null || !budgetList.Budgets.Any()) ? NoContent() : Ok(budgetList);
    }
}
