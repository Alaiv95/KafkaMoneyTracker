using Application.Dtos;
using Application.handlers.budget.commands.CreateBudget;
using Application.handlers.budget.commands.UpdateBudget;
using Application.handlers.budget.queries.GetBudgetList;
using Application.mappers;
using Application.mediator.interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Extentions;

namespace WebApi.Controllers;

[Route("api/budget/")]
[ApiController]
public class BudgetController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _budgetMapper;

    public BudgetController(IMediator mediator, IMapper budgetMapper)
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
        var createBudgetCommand = _budgetMapper.Map<CreateBudgetCommand>(dto);
        createBudgetCommand.UserId = HttpContext.GetUserIdFromToken();
        
        var createdBudgetId = await _mediator.HandleRequest(createBudgetCommand);

        return Ok(createdBudgetId);
    }
    
    /// <summary>
    /// Update active Budget for chosen category
    /// </summary>
    /// <returns>Returns changed fields of budget</returns>
    /// <response code="200">Success</response>
    /// <response code="400">Bad Request</response>
    [Authorize]
    [HttpPut("update")]
    [ProducesResponseType(typeof(BudgetUpdateResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BudgetUpdateResponseDto>> UpdateBudget([FromBody] BudgetUpdateRequestDto dto)
    {
        var createBudgetCommand = _budgetMapper.Map<UpdateBudgetCommand>(dto);
        createBudgetCommand.UserId = HttpContext.GetUserIdFromToken();
        
        var updatedBudget = await _mediator.HandleRequest(createBudgetCommand);

        return Ok(updatedBudget);
    }

    /// <summary>
    /// Get list of filtered budgets
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
    public async Task<ActionResult<BudgetListVm>> SearchBudgets([FromQuery] BaseFilterSearchDto dto)
    {
        var getBudgetQuery = _budgetMapper.Map<GetBudgetListQuery>(dto);
        getBudgetQuery.UserId = HttpContext.GetUserIdFromToken();
        
        var budgetList = await _mediator.HandleRequest(getBudgetQuery);

        return (!budgetList.Budgets.Any()) ? NoContent() : Ok(budgetList);
    }
}
