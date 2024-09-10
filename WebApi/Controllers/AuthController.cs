using Application.Dtos;
using Application.handlers.auth.commands.Login;
using Application.handlers.auth.commands.Register;
using Application.mappers;
using Application.mediator.interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/auth/")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public AuthController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
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
    public async Task<ActionResult<bool>> Register([FromBody] RegisterDto dto)
    {
        var registerUserCommand = _mapper.Map<RegisterUserCommand>(dto);
        var result = await _mediator.HandleRequest(registerUserCommand);

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
        var loginUserCommand = _mapper.Map<LoginCommand>(dto);
        var result = await _mediator.HandleRequest(loginUserCommand);

        return Ok(result);
    }
}
