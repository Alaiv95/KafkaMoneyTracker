using Application.Dtos;
using Application.mappers;
using Application.mediator.interfaces;
using Application.mediatorHandlers.auth;
using Application.mediatorHandlers.budget.commands;
using Microsoft.AspNetCore.Mvc;
using WebApi.Extentions;

namespace WebApi.Controllers;

[Route("api/auth/")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly AuthMapper _authMapper;

    public AuthController(IMediator mediator, AuthMapper authMapper)
    {
        _mediator = mediator;
        _authMapper = authMapper;
    }

    /// <summary>
    /// Register user
    /// </summary>
    /// <returns>boolean</returns>
    /// <response code="200">Success</response>
    /// <response code="400">Bad Request</response>
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<bool>> Register([FromBody] AuthDto dto)
    {
        var registerUserCommand = _authMapper.DtoToRegisterUserCommand(dto);
        var result = await _mediator.HandleRequest<RegisterUserCommand, bool>(registerUserCommand);

        return Ok(result);
    }

    /// <summary>
    /// Login user
    /// </summary>
    /// <returns>bearer token</returns>
    /// <response code="200">Success</response>
    /// <response code="400">Bad Request</response>
    /// <response code="403">Forbidden</response>
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<string>> Login([FromBody] AuthDto dto)
    { 
        var loginUserCommand = _authMapper.DtoToLoginCommand(dto);
        var result = await _mediator.HandleRequest<LoginCommand, string>(loginUserCommand);

        return Ok(result);
    }
}
