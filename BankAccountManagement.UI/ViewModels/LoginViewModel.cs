using BankAccountManagement.UI.Commands;
using BankAccountManagement.UI.Services.Api.Response;
using BankAccountManagement.UI.Services.Interfaces;
using System.Windows;
using System.Windows.Input;

namespace BankAccountManagement.UI.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private MainWindowViewModel _viewModel;
        private readonly IBankAccountManagementService _tcService;
        private readonly IMessageBoxService _messageBoxService;

        public LoginViewModel(MainWindowViewModel mainWindowViewModel,
            IBankAccountManagementService tcService,
            IMessageBoxService messageBoxService)
        {
            _viewModel = mainWindowViewModel;
            _tcService = tcService ?? throw new ArgumentNullException(nameof(tcService));
            _messageBoxService = messageBoxService ?? throw new ArgumentNullException(nameof(messageBoxService));
        }

        private ICommand _loginCommandAsync;

        public string UserName { get; set; }

        public ICommand LoginCommandAsync
        {
            get
            {
                if (_loginCommandAsync == null)
                {
                    _loginCommandAsync = new RelayCommand(async param => await LoginAsync());
                }

                return _loginCommandAsync;
            }
        }

        public async Task LoginAsync()
        {
            var response = await _tcService.Users.GetUser(UserName);

            if (response is IErrorResponse<bool> error)
            {
                _messageBoxService.ShowMessageBox(error.Error, "Request failed!");
            }

            if (response.Value != null)
            {
                UserSession.UserName = response.Value.Name;
                UserSession.UserId = response.Value.UserId;
                UserSession.Email = response.Value.Email;

                var mainWindow = new MainWindow();
                mainWindow.DataContext = _viewModel;
                mainWindow.Show();
                Application.Current.Windows[0]?.Close();
            }
            else
            {
                _messageBoxService.ShowMessageBox("User not found", "Not foud");
            }
        }
    }
}
