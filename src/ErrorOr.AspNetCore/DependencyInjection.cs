using ErrorOr.AspNetCore;
using Microsoft.Extensions.Options;

// Intentionally placed in Microsoft.Extensions.DependencyInjection so that AddErrorOrAspNetCore()
// is discoverable via IntelliSense without an extra using directive — consistent with AddHealthChecks(),
// AddAuthentication(), and other ASP.NET Core service registration conventions.
namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extensions for registering ErrorOr ASP.NET Core services.
/// </summary>
public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        /// <summary>
        /// Registers ErrorOr ASP.NET Core services and options.
        /// </summary>
        /// <param name="configure">An optional action to configure <see cref="ErrorOrAspNetCoreOptions"/>.</param>
        /// <returns>An <see cref="OptionsBuilder{ErrorOrAspNetCoreOptions}"/> for further configuration.</returns>
        public OptionsBuilder<ErrorOrAspNetCoreOptions> AddErrorOrAspNetCore(
            Action<ErrorOrAspNetCoreOptions>? configure = null)
        {
            var optionsBuilder = services.AddOptions<ErrorOrAspNetCoreOptions>();

            if (configure is not null)
            {
                optionsBuilder.Configure(configure);
            }

            return optionsBuilder;
        }
    }
}
