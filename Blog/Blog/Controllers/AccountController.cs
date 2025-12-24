using Blog.Data;
using Blog.Extensions;
using Blog.Services;
using Blog.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers;

[ApiController]
public class AccountController : ControllerBase
{
    [HttpPost("v1/accounts/")]
    public async IActionResult Post(
        [FromBody] RegisterViewModel registerViewModel,
        [FromServices] AppDbContext context)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));

        var user = new Models.User
        {
            Name = registerViewModel.Name,
            Email = registerViewModel.Email,
            Slug = registerViewModel.Email.Replace("@", "-").Replace(".", "-")
        };

        return null;
    }

    [HttpPost("v1/accounts/login")]
    public IActionResult Login([FromServices] TokenService tokenService)
    {
        var token = tokenService.GenerateToken(null);
        return Ok(token);
    }
}
