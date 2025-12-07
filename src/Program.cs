using MinimalApi.Dtos;
using MinimalApi.Services;
using MinimalApi.Validators;

var builder = WebApplication.CreateBuilder(args);

// Register services
builder.Services.AddScoped<TaskTrackerClient>();
builder.Services.AddScoped<TaskRequestDtoValidator>();

var app = builder.Build();

app.MapPost("/tasks", async (HttpContext context, TaskTrackerClient client, TaskRequestDtoValidator validator) =>
{
    var taskItem = await context.Request.ReadFromJsonAsync<TaskRequestDto>();
    if (taskItem == null)
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        return;
    }

    var validationResult = await validator.ValidateAsync(taskItem);
    if (!validationResult.IsValid)
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        await context.Response.WriteAsJsonAsync(validationResult.Errors);
        return;
    }

    var createdTaskItem = await client.CreateTaskAsync(taskItem);
    await context.Response.WriteAsJsonAsync(createdTaskItem);
});
app.Run();
