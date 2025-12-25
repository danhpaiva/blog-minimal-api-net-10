using Blog.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers;

[ApiController]
public class PostController : ControllerBase
{
    [HttpGet("v1/posts/")]
    public async Task<IActionResult> GetAsync(
        [FromServices] AppDbContext context)
    {
        return Ok(await context.Posts.ToListAsync());
    }
}
