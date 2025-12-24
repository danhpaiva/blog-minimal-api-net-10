using Blog.Data;
using Blog.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers;

[ApiController]
public class CategoryController : ControllerBase
{
    [HttpGet("v1/categories")]
    public async Task<IActionResult> GetAsync([FromServices] AppDbContext context)
    {
        var categories = await context.Categories.ToListAsync();
        return Ok(categories);
    }

    [HttpGet("v1/categories/{id:int}")]
    public async Task<IActionResult> GetByIdAsync(
        [FromRoute] int id,
        [FromServices] AppDbContext context)
    {
        var category = await context
            .Categories
            .FirstOrDefaultAsync(c => c.Id == id);

        if (category == null)
            return NotFound();
        return Ok(category);
    }

    [HttpPost("v1/categories")]
    public async Task<IActionResult> PostAsync(
    [FromBody] Category category,
    [FromServices] AppDbContext context)
    {
        try
        {
            await context.Categories.AddAsync(category);
            await context.SaveChangesAsync();

            return Created($"v1/categories/{category.Id}", category);
        }
        catch (DbUpdateException dbEx)
        {
            return StatusCode(500, "05XE9 - Nao foi possivel incluir a Categoria");
        }
        catch (Exception e)
        {
            return StatusCode(500, "05XE10 - Falha interna no servidor");
        }
    }

    [HttpPut("v1/categories/{id:int}")]
    public async Task<IActionResult> PutAsync(
        [FromRoute] int id,
        [FromBody] Category model,
        [FromServices] AppDbContext context)
    {
        var category = await context
              .Categories
              .FirstOrDefaultAsync(c => c.Id == id);

        if (category == null)
            return NotFound();
        
        category.Name = model.Name;
        category.Slug = model.Slug;

        context.Categories.Update(category);

        await context.SaveChangesAsync();

        return Ok(category);
    }

    [HttpDelete("v1/categories/{id:int}")]
    public async Task<IActionResult> DeleteAsync(
       [FromRoute] int id,
       [FromServices] AppDbContext context)
    {
        var category = await context
              .Categories
              .FirstOrDefaultAsync(c => c.Id == id);

        if (category == null)
            return NotFound();

        context.Categories.Remove(category);

        await context.SaveChangesAsync();

        return Ok(category);
    }
}
