using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

Console.WriteLine("OTEL_EXPORTER_OTLP_ENDPOINT = " + builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"]);

// Add services to the container.
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

       // metrics.AddOtlpExporter();
        metrics.AddPrometheusExporter();
    });

var app = builder.Build();
app.MapPrometheusScrapingEndpoint();


var sampleTodos = new Todo[]
{
    new(1, "Walk the dog"),
    new(2, "Do the dishes", DateOnly.FromDateTime(DateTime.Now)),
    new(3, "Do the laundry", DateOnly.FromDateTime(DateTime.Now.AddDays(1))),
    new(4, "Clean the bathroom"),
    new(5, "Clean the car", DateOnly.FromDateTime(DateTime.Now.AddDays(2)))
};

var todosApi = app.MapGroup("/todos");
todosApi.MapGet("/", () =>
{
    return sampleTodos;
});
todosApi.MapGet("/{id:int}", (int id) =>
    sampleTodos.FirstOrDefault(a => a.Id == id) is { } todo
        ? Results.Ok(todo)
        : Results.NotFound());
app.Run();

public record Todo(int Id, string? Title, DateOnly? DueBy = null, bool IsComplete = false);
