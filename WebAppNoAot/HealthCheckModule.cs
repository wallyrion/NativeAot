using System.Runtime.CompilerServices;
using HealthCheck.ModuleInitializer;

namespace WebAppNoAot;

public static class HealthCheckModule
{
    [ModuleInitializer]
    public static void Initialize()
    {
        HealthCheckInitializer.Initialize();
    }
}