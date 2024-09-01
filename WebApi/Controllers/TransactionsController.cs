
using Application.Dtos;
using Application.handlers.transactions.commands.CancelTransactions;
using Application.handlers.transactions.commands.CreateTransaction;
using Application.handlers.transactions.queries.GetUserTransactions;
using Application.MailClient;
using Application.mappers;
using Application.mediator.interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Extentions;

namespace WebApi.Controllers;

[Route("api/transactions/")]
[ApiController]
public class TransactionsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _transactionMapper;

    public TransactionsController(IMediator mediator, IMapper transactionMapper)
    {
        _mediator = mediator;
        _transactionMapper = transactionMapper;
    }

    /// <summary>
    /// Initiate transaction for chosen category. If limit of budget exceeded, email message is sent to user
    /// </summary>
    /// <returns>Return boolean true if transaction initiated</returns>
    /// <response code="200">Success</response>
    /// <response code="400">Bad Request</response>
    [Authorize]
    [HttpPost("initiate")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<bool>> InitTransaction([FromBody] TransactionCreateDto dto)
    {
        var createTransactionCommand = _transactionMapper.Map<CreateTransactionCommand>(dto);
        createTransactionCommand.UserId = HttpContext.GetUserIdFromToken();
        
        var result = await _mediator.HandleRequest(createTransactionCommand);

        return Ok(result);
    }
    
    /// <summary>
    /// Get list of transactions
    /// </summary>
    /// <returns>Returns list of TransactionLookupExtendedDto</returns>
    /// <response code="200">Success</response>
    /// <response code="400">Bad Request</response>
    [Authorize]
    [HttpGet("search")]
    [ProducesResponseType(typeof(List<TransactionLookupExtendedDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TransactionLookupDto>> SearchTransactions([FromQuery] BaseFilterSearchDto dto)
    {
        var searchQuery = _transactionMapper.Map<GetUserTransactionsQuery>(dto);
        searchQuery.UserId = HttpContext.GetUserIdFromToken();

        var result = await _mediator.HandleRequest(searchQuery);
    
        return Ok(result);
    }
    
    /// <summary>
    /// Cancel transactions of current user, transaction ids of other users are ignored
    /// </summary>
    /// <returns>Return canceled transactions of current user</returns>
    /// <response code="200">Success</response>
    /// <response code="400">Bad Request</response>
    [Authorize]
    [HttpPut("cancel_by-ids")]
    [ProducesResponseType(typeof(List<TransactionLookupDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TransactionLookupDto>> CancelTransactions([FromBody] BaseItemListDto dto)
    {
        var cancelTransactionCommand = _transactionMapper.Map<CancelTransactionsCommand>(dto);
        cancelTransactionCommand.UserId = HttpContext.GetUserIdFromToken();
        
        var result = await _mediator.HandleRequest(cancelTransactionCommand);
    
        return Ok(result);
    }
}
