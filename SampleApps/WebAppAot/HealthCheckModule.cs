using System.Runtime.CompilerServices;
using HealthCheck.ModuleInitializer;

namespace WebAotSample;

public static class HealthCheckModule
{
    [ModuleInitializer]
    public static void Initialize()
    {
        HealthCheckInitializer.Initialize();
    }
}