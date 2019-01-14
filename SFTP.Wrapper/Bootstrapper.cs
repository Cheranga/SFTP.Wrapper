using System;
using Microsoft.Extensions.DependencyInjection;
using SFTP.Wrapper.Configs;

namespace SFTP.Wrapper
{
    public static class Bootstrapper
    {
        public static void UseSftp(this IServiceCollection services, SftpConfig config)
        {
            var isValidConfig = config?.IsValid() ?? false;
            if (!isValidConfig)
            {
                throw new Exception($"{nameof(SftpConfig)} is invalid");
            }

            RegisterDependencies(services, config);
        }

        private static void RegisterDependencies(IServiceCollection services, SftpConfig config)
        {
            services.AddSingleton(config);
            services.AddSingleton<ISftpManager, SftpManager>();
        }
    }
}