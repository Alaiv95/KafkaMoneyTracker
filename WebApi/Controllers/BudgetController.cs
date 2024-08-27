namespace WebApi.Controllers;

using Application.Dtos;
using Application.mappers;
using Application.mediator.interfaces;
using Application.mediatorHandlers.budget.commands;
using Application.mediatorHandlers.budget.queries;
using Microsoft.AspNetCore.Mvc;

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
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Guid>> AddBudget([FromBody] BudgetCreateDto dto)
    {
        var createBudgetCommand = _budgetMapper.DtoToCommand(dto);
        var createdBudgetId = await _mediator.HandleRequest<CreateBudgetCommand, Guid>(createBudgetCommand);

        return Ok(createdBudgetId);
    }

    /// <summary>
    /// Create the Budget for category during chosen period
    /// </summary>
    /// <returns>Returns GetBudgetListDto</returns>
    /// <response code="200">Success</response>
    /// <response code="204">No Content</response>
    /// <response code="400">Bad Request</response>
    [HttpGet("search")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<GetBudgetListDto>> SearchBudgets([FromQuery] GetBudgetListQuery query)
    {
        var budgetList = await _mediator.HandleRequest<GetBudgetListQuery, GetBudgetListDto>(query);

        return (budgetList == null || budgetList.Budgets.Any()) ? NoContent() : Ok(budgetList);
    }
}
