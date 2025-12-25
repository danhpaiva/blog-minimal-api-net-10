using Blog.Data;
using Blog.Extensions;
using Blog.Services;
using Blog.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureIdentity.Password;

namespace Blog.Controllers;

[ApiController]
public class AccountController : ControllerBase
{
    [HttpPost("v1/accounts/")]
    public async Task<IActionResult> PostAsync(
        [FromBody] RegisterViewModel registerViewModel,
        [FromServices] EmailService emailService,
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

        var password = PasswordGenerator.Generate(25);
        user.PasswordHash = PasswordHasher.Hash(password);

        try
        {
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            emailService.Send(
                user.Name,
                user.Email,
                "Bem vindo ao Blog!",
                $"Sua senha é {password}"
            );

            return Ok(new ResultViewModel<dynamic>(new
            {
                user = user.Email,
                //Somente para testar mandando o password no retorno da api.
                //Nao fazer isso em producao.
                password
            }));
        }
        catch (DbUpdateException)
        {
            return StatusCode(400, new ResultViewModel<string>("06XE01 - Nao foi possivel criar o usuario com este email."));
        }
        catch (Exception)
        {
            return StatusCode(500, new ResultViewModel<string>("06XE02 - Falha interna no servidor"));
        }
    }

    [HttpPost("v1/accounts/login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginViewModel loginViewModel,
        [FromServices] AppDbContext context,
        [FromServices] TokenService tokenService)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));

        var user = await context
            .Users
            .AsNoTracking()
            .Include(u => u.Roles)
            .FirstOrDefaultAsync(u => u.Email == loginViewModel.Email);

        if (user == null)
            return StatusCode(401, new ResultViewModel<string>("06XE03 - Usuario ou senha invalidos"));

        if (!PasswordHasher.Verify(user.PasswordHash, loginViewModel.Password))
            return StatusCode(401, new ResultViewModel<string>("06XE04 - Usuario ou senha invalidos"));

        try
        {
            var token = tokenService.GenerateToken(user);
            return Ok(new ResultViewModel<string>(token, null));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<string>("06XE05 - Falha interna no servidor"));
        }
    }
}
