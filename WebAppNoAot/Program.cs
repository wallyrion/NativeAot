using OpenTelemetry.Metrics;

var builder = WebApplication.CreateBuilder(args);

Console.WriteLine("OTEL_EXPORTER_OTLP_ENDPOINT = " + builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"]);

// Add services to the container.
builder.Services.AddHealthChecks();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddOpenTelemetry()
    .WithMetrics(metrics =>
    {
        metrics.AddAspNetCoreInstrumentation();
        //Our custom metrics
        metrics.AddMeter("System.Runtime");
        // Metrics provides by ASP.NET Core in .NET 8
        metrics.AddMeter("Microsoft.AspNetCore.Hosting");
        metrics.AddMeter("Microsoft.AspNetCore.Server.Kestrel");

        metrics.AddOtlpExporter();
        metrics.AddPrometheusExporter();
    });

var app = builder.Build();

app.MapPrometheusScrapingEndpoint();

app.MapHealthChecks("/healthz");


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

