using Blog.Data;
using Blog.ViewModels;
using Blog.ViewModels.Posts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers;

[ApiController]
public class PostController : ControllerBase
{
    [HttpGet("v1/posts/")]
    public async Task<IActionResult> GetAsync(
        [FromServices] AppDbContext context,
        [FromQuery] int page = 0,
        [FromQuery] int pageSize = 25)
    {
        try
        {
            var posts = await context
          .Posts
          .AsNoTracking()
          .Include(p => p.Category)
          .Include(p => p.Author)
          .Select(
          p => new ListPostsViewModel
          {
              Id = p.Id,
              Title = p.Title,
              Slug = p.Slug,
              LastUpdateDate = p.LastUpdateDate,
              Category = p.Category.Name,
              Author = p.Author.Name
          })
          .Skip(page * pageSize)
          .Take(pageSize)
          .OrderBy(p => p.LastUpdateDate)
          .ToListAsync();
            return Ok(new ResultViewModel<dynamic>(new
            {
                page,
                pageSize,
                posts
            }));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<string>("09XE01 - Falha interna no servidor"));
        }
    }
}
