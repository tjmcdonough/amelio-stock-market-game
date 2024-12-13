using System.Reflection;
using StockMarketGame.Common;

namespace StockMarketGame.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddSingletonServices(this IServiceCollection serviceCollection)
    {
        Type baseInterface = typeof(ISingletonService);

        _registerServicesBasedOnInterface(serviceCollection, typeof(Program), baseInterface);
        
        return serviceCollection;
    }
    
    private static void _registerServicesBasedOnInterface(IServiceCollection serviceCollection, Type sampleTypeInAssembly, Type baseInterface)
    {
        var services = Assembly.GetAssembly(sampleTypeInAssembly)?.GetExportedTypes()
            .Where(type => type is { IsClass: true, IsAbstract: false })
            .SelectMany(type => type.GetInterfaces(), (type, interfaceType) => new { Type = type, Interface = interfaceType })
            .Where(t => t.Interface != baseInterface && baseInterface.IsAssignableFrom(t.Interface))
            .ToList();

        services?.ForEach(service =>
        {
            serviceCollection.AddSingleton(service.Interface, service.Type);
        });
    }
}