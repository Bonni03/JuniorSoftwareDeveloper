using Microsoft.AspNetCore.Http.Json;
using System.Text.Json.Serialization;
using System.Text.Json;

// path for swagger after running dotnet: http://localhost:5134/swagger/index.html

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

const string fPath = "tasks.json";

// Load values which I didnt hardcode for security 
var userId = builder.Configuration.GetValue<int>("AppSettings:UserId");
var passwordWS = builder.Configuration.GetValue<string>("AppSettings:PasswordWS");
var cabinetId = builder.Configuration.GetValue<string>("AppSettings:CabinetId");


async Task<List<TaskItem>> LoadTasks()
{
    if (!File.Exists(fPath))
        return new List<TaskItem>();

    var json = await File.ReadAllTextAsync(fPath);
    return JsonSerializer.Deserialize<List<TaskItem>>(json) ?? new List<TaskItem>();
}

async Task SaveTasks(List<TaskItem> tasks)
{
    var json = JsonSerializer.Serialize(tasks, new JsonSerializerOptions { WriteIndented = true });
    await File.WriteAllTextAsync(fPath, json);
}

app.MapPost("/tasks", async (TaskItem newTask) =>
{
    var tasks = await LoadTasks();

    if (tasks.Any(t => t.TASK_ID == newTask.TASK_ID))
        return Results.Conflict($"Task with ID {newTask.TASK_ID} already exists.");


    newTask.CREATION_DATE = DateTime.UtcNow;
    tasks.Add(newTask);
    await SaveTasks(tasks);

    /*
        Here its the Extra.
    */

    try
    {
        using var httpClient = new HttpClient();

        var pload = new
        {
            userId = userId,
            passwordWS = passwordWS,
            cabinetId = cabinetId,
            indexFields = new[]
            {
            new { fieldName = "TASK_ID", fieldValue = newTask.TASK_ID.ToString() },
            new { fieldName = "TASK_DESCRIPTION", fieldValue = newTask.TASK_DESCRIPTION ?? "" },
            new { fieldName = "CREATION_DATE", fieldValue = DateTime.UtcNow.ToString("o") }
        }
        };

        var response = await httpClient.PostAsJsonAsync(
            "https://services.paloalto.swiss:10443/api2/Docuware/add-record",
            pload
        );

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Error in sending: {response.StatusCode} - {error}");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Exception while sending: {ex.Message}");
    }

    return Results.Created($"/tasks/{newTask.TASK_ID}", newTask);
});

app.MapGet("/tasks", async () =>
{
    var tasks = await LoadTasks();
    var orderedTasks = tasks.OrderBy(t => t.TASK_ID).ToList();
    return Results.Ok(orderedTasks);
})
.WithName("GetTasks");

app.MapPut("/tasks/{id}", async (int id, TaskItem updatedTask) =>
{
    var tasks = await LoadTasks();
    var existingTask = tasks.FirstOrDefault(t => t.TASK_ID == id);

    if (existingTask == null)
        return Results.NotFound($"Task with ID {id} not found.");

    existingTask.TASK_DESCRIPTION = updatedTask.TASK_DESCRIPTION;
    await SaveTasks(tasks);

    return Results.Ok(existingTask);
})
.WithName("UpdateTask");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();
