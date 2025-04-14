using BankAccountManagement.Models;
using BankAccountManagement.UI.Commands;
using BankAccountManagement.UI.Services.Api.Response;
using BankAccountManagement.UI.Services.Interfaces;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace BankAccountManagement.UI.ViewModels
{
    public class TransferMoneyViewModel : ViewModelBase
    {
        private readonly IBankAccountManagementService _tcService;
        private readonly IMessageBoxService _messageBoxService;
        private ICommand _selectSourceAccountCommand;
        private ICommand _transferCommandAsync;

        public Account SelectedTargetAccount { get; set; }

        public Account SelectedSourceAccount { get; set; }

        public int Amount { get; set; }

        public List<Account> UserAccounts { get; set; }

        public ObservableCollection<Account> TargetAccounts { get; set; }

        public ObservableCollection<Account> SourceAccounts { get; set; }

        public TransferMoneyViewModel(IBankAccountManagementService tcService,
            IMessageBoxService messageBoxService)
        {
            _tcService = tcService ?? throw new ArgumentNullException(nameof(tcService));
            _messageBoxService = messageBoxService ?? throw new ArgumentNullException(nameof(messageBoxService));
        }

        public ICommand SelectSourceAccountCommand
        {
            get
            {
                if (_selectSourceAccountCommand == null)
                {
                    _selectSourceAccountCommand = new RelayCommand(param => BindTargetAccount());
                }

                return _selectSourceAccountCommand;
            }
        }

        public ICommand TransferCommandAsync
        {
            get
            {
                if (_transferCommandAsync == null)
                {
                    _transferCommandAsync = new RelayCommand(async param => await TransferMoney());
                }

                return _transferCommandAsync;
            }
        }

        public async Task FetchUserAccounts()
        {
            UserAccounts = new List<Account>();
            SourceAccounts = new ObservableCollection<Account>();
            var response = await _tcService.Accounts.GetUserAccounts(UserSession.UserId);

            if (response is IErrorResponse<bool> error)
            {
                _messageBoxService.ShowMessageBox(error.Error, "Request failed!");
            }

            if (response.Value != null)
            {
                foreach (var account in response.Value)
                {
                    UserAccounts.Add(account);
                    SourceAccounts.Add(account);
                }
            }

            BindSourceAccount();
        }

        private void BindSourceAccount()
        {
            SourceAccounts = new ObservableCollection<Account>();
            var sources = UserAccounts.Where(a => a.AccountType != BankAccountManagement.Enums.AccountType.Loan);
            foreach (var source in sources)
            {
                SourceAccounts.Add(source);
            }

            OnPropertyChanged(nameof(SourceAccounts));
        }

        private void BindTargetAccount()
        {
            TargetAccounts = new ObservableCollection<Account>();
            var targets = UserAccounts.Where(a => a.AccountId != SelectedSourceAccount.AccountId);
            foreach (var source in targets)
            {
                TargetAccounts.Add(source);
            }

            OnPropertyChanged(nameof(TargetAccounts));
        }

        private async Task TransferMoney()
        {
            var userAccounts = await _tcService.Accounts.GetUserAccounts(UserSession.UserId);
            var fromAccount = userAccounts.Value.Where(ua=>ua.AccountId == SelectedSourceAccount.AccountId).FirstOrDefault();
            if(fromAccount.Balance < Amount)
            {
                _messageBoxService.ShowWarningBox("Available funds not enough.", "Insufficient funds!");
                return;
            }

            var response = await _tcService.Accounts.TransferMoney(new Models.Payload.TransferMoney()
            {
                UserId = UserSession.UserId,
                Amount = Amount,
                FromAccountId = SelectedSourceAccount.AccountId,
                ToAccountId = SelectedTargetAccount.AccountId
            });

            if (response is IErrorResponse<bool> error)
            {
                _messageBoxService.ShowErrorBox(error.Error, "Transfer failed!");
            }
            else
            {
                _messageBoxService.ShowMessageBox("Transfer Complete", "Success");
            }

            var thisWindow = Application.Current.Windows
                .Cast<Window>()
                .FirstOrDefault(w => w.Title == "TransferWindow");
            thisWindow.Visibility = Visibility.Hidden;
        }
    }
}
