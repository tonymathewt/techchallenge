using BankAccountManagement.Models.Payload;
using BankAccountManagement.UI.Services.Api.Response;
using BankAccountManagement.UI.Services.Interfaces;
using System.Text;
using System.Windows;

namespace BankAccountManagement.UI.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {

        private readonly IBankAccountManagementService _tcService;
        private readonly IMessageBoxService _messageBoxService;
        private LoanViewModel _loanViewModel;
        private TransferMoneyViewModel _transferViewModel;

        public MainWindowViewModel(IBankAccountManagementService tcService, 
            IMessageBoxService messageBoxService,
            LoanViewModel loanViewModel,
            TransferMoneyViewModel transferViewModel)
        {
            _tcService = tcService ?? throw new ArgumentNullException(nameof(tcService));
            _messageBoxService = messageBoxService ?? throw new ArgumentNullException(nameof(messageBoxService));
            _loanViewModel = loanViewModel ?? throw new ArgumentNullException(nameof(loanViewModel));
            _transferViewModel = transferViewModel ?? throw new ArgumentNullException(nameof(transferViewModel));
            UserAccounts = new System.Collections.ObjectModel.ObservableCollection<Models.Account>();
        }

        public async Task FetchUserAccounts()
        {
            Name = UserSession.UserName;
            UserAccounts = new System.Collections.ObjectModel.ObservableCollection<Models.Account>();
            var response = await _tcService.Accounts.GetUserAccounts(UserSession.UserId);

            if (response is IErrorResponse<bool> error)
            {
                _messageBoxService.ShowErrorBox(error.Error, "Request failed!");
            }

            if (response.Value != null)
            {
                foreach (var account in response.Value)
                {
                    UserAccounts.Add(account);
                }
            }

            OnPropertyChanged(nameof(UserAccounts));
        }

        private async Task RequestLoanAsync()
        {
            var users = await _tcService.Users.GetAll();

            var validateResponse = await _tcService.Loans.ValidateLoanEligibility(UserSession.UserName);
            if (validateResponse is IErrorResponse<bool> error)
            {
                _messageBoxService.ShowWarningBox(error.Error, "Validate Request failed!");
                return;
            }

            if (validateResponse.Value != null && validateResponse.Value.Count > 0)
            {
                var errors = new StringBuilder();
                foreach (var validateError in validateResponse.Value)
                {
                    errors.AppendLine(validateError);
                }

                _messageBoxService.ShowWarningBox(errors.ToString(), "Not eligible!");
            }
            else
            {
                // Go to Loan window
                var loanWindow = new LoanWindow();
                loanWindow.DataContext = _loanViewModel;
                loanWindow.Show();
            }
        }        

        private void TransferMoney()
        {
            var transferWindow = new TransferWindow();
            transferWindow.DataContext = _transferViewModel;
            transferWindow.Show();
        }
    }
}