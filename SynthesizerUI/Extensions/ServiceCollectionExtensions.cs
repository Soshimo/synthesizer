using Microsoft.Extensions.DependencyInjection;

namespace SynthesizerUI.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddViewModels<TViewModelBase>(this IServiceCollection services)
    {
        var vmType = typeof(TViewModelBase);

        var viewModels =
            vmType.Assembly.ExportedTypes.Where(x => x.IsAssignableTo(vmType) && x is { IsAbstract: false, IsInterface: false });

        foreach (var viewModel in viewModels)
        {
            services.AddSingleton(viewModel);
        }

        return services;
    }
}