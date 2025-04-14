using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using BankAccountManagement.UI.Services;
using BankAccountManagement.UI.Services.Interfaces;
using BankAccountManagement.UI.ViewModels;
using System.Runtime.CompilerServices;

namespace BankAccountManagement.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IServiceProvider ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);            

            var serviceCollection = new ServiceCollection();

            ConfigureServices(serviceCollection);

            ServiceProvider = serviceCollection.BuildServiceProvider();
            var loginWindow = new Login();
            var loginWindowVM = ServiceProvider.GetRequiredService<LoginViewModel>();
            loginWindow.DataContext = loginWindowVM;
            loginWindow.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false, true)
                .Build();

            services.AddSingleton<IConfiguration>(configuration);

            AddViewModels(services);

            services.AddHttpClient<IBankAccountManagementService, BankAccountManagementService>();

            services.AddSingleton<IMessageBoxService, MessageBoxService>();
        }

        private void AddViewModels(IServiceCollection services)
        {
            services.AddTransient<LoginViewModel>();
            services.AddTransient<MainWindowViewModel>();
            services.AddTransient<LoanViewModel>();
            services.AddTransient<TransferMoneyViewModel>();
        }
    }
}
