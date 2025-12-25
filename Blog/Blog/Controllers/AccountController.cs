using Blog.Data;
using Blog.Extensions;
using Blog.Services;
using Blog.ViewModels;
using Blog.ViewModels.Accounts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureIdentity.Password;
using System.Text.RegularExpressions;

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

    [Authorize]
    [HttpPost("v1/accounts/upload-image")]
    public async Task<IActionResult> UploadImageAsync(
        [FromBody] UploadImageViewModel uploadImageViewModel,
        [FromServices] AppDbContext context)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));

        var fileName = $"{Guid.NewGuid().ToString()}.jpg";

        var data = new Regex(@"^data:image\/[a-z]+;base64,")
            .Replace(uploadImageViewModel.Base64Image, "");

        var bytes = Convert.FromBase64String(data);

        try
        {
            await System.IO.File.WriteAllBytesAsync($"wwwroot/images/{fileName}", bytes);
        }
        catch (Exception)
        {
            return StatusCode(500, new ResultViewModel<string>("06XE08 - Nao foi possivel salvar a imagem"));
        }

        var user = await context
            .Users
            .FirstOrDefaultAsync(u => u.Email == User.Identity.Name);

        if (user == null)
            return NotFound(new ResultViewModel<string>("06XE06 - Usuario nao encontrado"));
        
        user.Image = $"images/{fileName}";

        try
        {
            context.Users.Update(user);
            await context.SaveChangesAsync();
            return Ok(new ResultViewModel<string>("images/" + fileName, null));
        }
        catch (Exception)
        {
            return StatusCode(500, new ResultViewModel<string>("06XE07 - Falha interna no servidor"));
        }
    }
}
