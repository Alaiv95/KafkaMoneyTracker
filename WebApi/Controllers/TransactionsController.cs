
using Application.Dtos;
using Application.MailClient;
using Application.mappers;
using Application.mediator.interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Extentions;

namespace WebApi.Controllers;

[Route("api/transactions/")]
[ApiController]
public class TransactionsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly TransactionMapper _transactionMapper;
    private readonly IMailClient _mailClient;

    public TransactionsController(IMediator mediator, TransactionMapper transactionMapper, IMailClient mailClient)
    {
        _mediator = mediator;
        _transactionMapper = transactionMapper;
        _mailClient = mailClient;
    }

    /// <summary>
    /// Initiate transaction for chosen category
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
        var userId = HttpContext.GetUserIdFromToken();

        var createTransactionCommand = _transactionMapper.DtoToCommand(dto, userId);
        var result = await _mediator.HandleRequest(createTransactionCommand);

        return Ok(result);
    }
}
