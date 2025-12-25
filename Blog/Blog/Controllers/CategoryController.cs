using Blog.Data;
using Blog.Extensions;
using Blog.Models;
using Blog.ViewModels;
using Blog.ViewModels.Categories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Blog.Controllers;

[ApiController]
public class CategoryController : ControllerBase
{
    [HttpGet("v1/categories")]
    public async Task<IActionResult> GetAsync(
        [FromServices] AppDbContext context,
        [FromServices] IMemoryCache cache)
    {
        try
        {
            var categories = await cache.GetOrCreateAsync("CategoriesCache", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);
                return await GetCategoriesAsync(context);
            });

            return Ok(new ResultViewModel<List<Category>>(categories));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<List<Category>>("05XE01 - Falha interna no servidor"));
        }
    }

    private async Task<List<Category>> GetCategoriesAsync(AppDbContext context)
    {
        return await context.Categories.ToListAsync();
    }

    [HttpGet("v1/categories/{id:int}")]
    public async Task<IActionResult> GetByIdAsync(
        [FromRoute] int id,
        [FromServices] AppDbContext context)
    {
        try
        {
            var category = await context
            .Categories
            .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
                return NotFound(new ResultViewModel<Category>("Conteudo nao encontrado."));

            return Ok(new ResultViewModel<Category>(category));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<Category>("05XE02 - Falha interna no servidor"));
        }
    }

    [HttpPost("v1/categories")]
    public async Task<IActionResult> PostAsync(
    [FromBody] EditorCategoryViewModel model,
    [FromServices] AppDbContext context)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ResultViewModel<Category>(ModelState.GetErrors()));

        try
        {
            var category = new Category
            {
                Id = 0,
                Name = model.Name,
                Slug = model.Slug.ToLower(),
            };

            await context.Categories.AddAsync(category);
            await context.SaveChangesAsync();

            return Created($"v1/categories/{category.Id}", new ResultViewModel<Category>(category));
        }
        catch (DbUpdateException dbEx)
        {
            return StatusCode(500, new ResultViewModel<Category>("05XE3 - Nao foi possivel incluir a Categoria"));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<Category>("05XE4 - Falha interna no servidor"));
        }
    }

    [HttpPut("v1/categories/{id:int}")]
    public async Task<IActionResult> PutAsync(
        [FromRoute] int id,
        [FromBody] EditorCategoryViewModel model,
        [FromServices] AppDbContext context)
    {
        try
        {
            var category = await context
             .Categories
             .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
                return NotFound(new ResultViewModel<Category>("Conteudo nao encontrado."));

            category.Name = model.Name;
            category.Slug = model.Slug;

            context.Categories.Update(category);

            await context.SaveChangesAsync();

            return Ok(new ResultViewModel<Category>(category));
        }
        catch (DbUpdateException dbEx)
        {
            return StatusCode(500, new ResultViewModel<Category>("05XE5 - Nao foi possivel alterar a Categoria"));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<Category>("05XE6 - Falha interna no servidor"));
        }
    }

    [HttpDelete("v1/categories/{id:int}")]
    public async Task<IActionResult> DeleteAsync(
       [FromRoute] int id,
       [FromServices] AppDbContext context)
    {
        try
        {
            var category = await context
            .Categories
            .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
                return NotFound(new ResultViewModel<Category>("Conteudo nao encontrado."));

            context.Categories.Remove(category);

            await context.SaveChangesAsync();

            return Ok(new ResultViewModel<Category>(category));
        }
        catch (DbUpdateException dbEx)
        {
            return StatusCode(500, new ResultViewModel<Category>("05XE7 - Nao foi possivel excluir a Categoria"));
        }
        catch (Exception e)
        {
            return StatusCode(500, new ResultViewModel<Category>("05XE8 - Falha interna no servidor"));
        }
    }
}
