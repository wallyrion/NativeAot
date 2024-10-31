using System.Text.Json.Serialization;
using OpenTelemetry;
using OpenTelemetry.Metrics;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.AddOpenTelemetry()
    .WithMetrics(metrics =>
    {
        metrics.AddAspNetCoreInstrumentation();

        metrics.AddMeter("System.Runtime");
        // Metrics provides by ASP.NET Core in .NET 8
        metrics.AddMeter("Microsoft.AspNetCore.Hosting");
        metrics.AddMeter("Microsoft.AspNetCore.Server.Kestrel");
        
        metrics.AddPrometheusExporter();
        metrics.AddOtlpExporter();
    });


builder.Services.ConfigureHttpJsonOptions(options => { options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default); });

var app = builder.Build();
app.MapPrometheusScrapingEndpoint();

var todosApi = app.MapGroup("/todos");
todosApi.MapGet("/", TodoGenerator.GenerateRandomTodo);

app.Run();


public record Todo(Guid Id, string? Title, string Description, DateOnly? DueBy = null, bool IsComplete = false);


public static class TodoGenerator
{
    private static readonly Random Random = new Random(200);
    private static readonly string[] Titles = { "Buy groceries", "Complete project report", "Call a friend", "Read a book", "Exercise", "Plan the weekend" };

    public static Todo GenerateRandomTodo()
    {
        var id = Guid.NewGuid();
        var title = Titles[Random.Next(Titles.Length)];
        var dueBy = DateOnly.FromDateTime(DateTime.Today.AddDays(Random.Next(1, 30))); // Random due date within the next 30 days
        var isComplete = Random.Next(2) == 0; // Randomly assigns true or false
        var descriptionLength = Random.Next(100, 1000);
        var description = GenerateRandomString(descriptionLength);
        
        return new Todo(id, title, description, dueBy, isComplete);
    }
    
    static string GenerateRandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        char[] result = new char[length];
    
        for (int i = 0; i < length; i++)
        {
            result[i] = chars[Random.Shared.Next(chars.Length)];
        }

        return new string(result);
    }
}



[JsonSerializable(typeof(Todo[]))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{
}