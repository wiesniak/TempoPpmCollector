using System;
using System.IO;
using System.Reflection;
using System.Windows;

using Microsoft.Extensions.Configuration;

using Microsoft.Extensions.DependencyInjection;

using TimeCollector.Models;

namespace TimeCollector
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IServiceProvider ServiceProvider { get; private set; }

        public IConfiguration Configuration { get; private set; }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var builder = new ConfigurationBuilder()
             .SetBasePath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? Directory.GetCurrentDirectory())
             .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            Configuration = builder.Build();

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            ServiceProvider = serviceCollection.BuildServiceProvider();

            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(Configuration.GetSection("GroupingRules").Get<GroupingRule[]>() ?? Array.Empty<GroupingRule>());

            services.AddTransient(typeof(MainWindow));
        }
    }
}