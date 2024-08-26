namespace WebApi.Controllers;

using Application.budget.commands;
using Application.mediator;
using Microsoft.AspNetCore.Mvc;
using WebApi.Dtos;

[Route("api/[controller]")]
[ApiController]
public class BudgetController : ControllerBase
{
    private readonly IMediator _mediator;

    public BudgetController(IMediator mediator) => _mediator = mediator;

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
        var createBudgetCommand = dto.ToCommand();
        var createdBudgetId = await _mediator.HandleCommand<CreateBudgetCommand, Guid>(createBudgetCommand);

        return Ok(createdBudgetId);
    }
}
