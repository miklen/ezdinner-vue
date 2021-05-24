using EzDinner.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace EzDinner.IntegrationTests
{
    public class StartupFixture : IDisposable
    {
        private bool _disposedValue;

        public ServiceProvider Provider { get; }

        public StartupFixture()
        {
            var services = new ServiceCollection();
            var configuration = new ConfigurationBuilder()
            .SetBasePath(AssemblyDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile("appsettings.development.json", optional: true, reloadOnChange: true) // add local development variables if present - file is not committed to git so won't be present for prod
            .Build();

            services.AddSingleton(configuration);
            services.RegisterMsGraph(configuration.GetSection("AzureAdB2C"));
            services.RegisterCosmosDb(configuration.GetSection("CosmosDb"));
            services.RegisterCasbin(configuration.GetSection("CosmosDb"));
            services.RegisterRepositories();

            Provider = services.BuildServiceProvider();
        }

        public static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    Provider.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                _disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~Startup()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
