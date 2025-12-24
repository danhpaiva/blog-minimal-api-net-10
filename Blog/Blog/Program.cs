using Blog.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions( options =>
    {
        options.SuppressModelStateInvalidFilter = true;
    }
    );

builder.Services.AddDbContext<AppDbContext>();

builder.Services.AddOpenApi();

var app = builder.Build();
app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();
