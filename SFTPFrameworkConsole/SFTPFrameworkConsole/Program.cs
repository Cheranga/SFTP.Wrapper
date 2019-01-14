using System;
using Microsoft.Extensions.DependencyInjection;
using SFTP.Wrapper;
using SFTP.Wrapper.Configs;
using SFTP.Wrapper.Requests;

namespace SFTPFrameworkConsole
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var serviceProvider = GetServiceProvider();
            var sftpManager = serviceProvider.GetService<ISftpFileManager>();
            var getFilesRequest = new GetAllFilesRequest(@"/directory");

            var getFilesOperation = sftpManager.GetAllFilesAsync(getFilesRequest).Result;
            if (getFilesOperation.Status)
            {
                //
                // Do your thing
                //
                foreach (var sftpFileInformation in getFilesOperation.Data.Files)
                {
                    Console.WriteLine($"{sftpFileInformation.Name} :: {sftpFileInformation.FullName} :: {sftpFileInformation.Length}");
                }
            }
            else
            {
                Console.WriteLine(getFilesOperation.Message);
            }

            Console.ReadLine();
        }

        private static IServiceProvider GetServiceProvider()
        {
            var services = new ServiceCollection();
            //
            // Register the services here
            //
            services.UseSftp(new SftpConfig
            {
                Host = "SFTP Host",
                UserName = "User Name",
                Password = "Password",
                Port = 22
            });

            var serviceProvider = services.BuildServiceProvider();
            return serviceProvider;
        }
    }
}