using BankAccountManagement.Models;
using BankAccountManagement.Models.Payload;
using BankAccountManagement.UI.Commands;
using BankAccountManagement.UI.Services.Api.Response;
using BankAccountManagement.UI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BankAccountManagement.UI.ViewModels
{
    public class LoanViewModel
    {
        private readonly IBankAccountManagementService _tcService;
        private readonly IMessageBoxService _messageBoxService;
        private readonly Account _selectedLinkedAccount;
        private ICommand _saveCommandAsync;

        public ObservableCollection<Account> ExistingAccounts { get; set; }

        public Account SelectedLinkedAccount { get; set; }

        public int Duration { get; set; }

        public int Value { get; set; }

        public ICommand SubmitLoanCommandAsync
        {
            get
            {
                if (_saveCommandAsync == null)
                {
                    _saveCommandAsync = new RelayCommand(async param => await SubmitLoanAsync());
                }

                return _saveCommandAsync;
            }
        }

        public LoanViewModel(IBankAccountManagementService tcService,
            IMessageBoxService messageBoxService)
        {
            _tcService = tcService ?? throw new ArgumentNullException(nameof(tcService));
            _messageBoxService = messageBoxService ?? throw new ArgumentNullException(nameof(messageBoxService));
            ExistingAccounts = new ObservableCollection<Account>();
        }

        public async Task PopulateModels()
        {
            var accounts = await _tcService.Accounts.GetUserAccounts(UserSession.UserId);

            foreach (var account in accounts.Value.Where(a => a.AccountType != BankAccountManagement.Enums.AccountType.Loan))
            {
                ExistingAccounts.Add(account);
            }
        }

        private async Task SubmitLoanAsync()
        {
            var user = await _tcService.Users.GetUser(UserSession.UserName);
            var newLoan = new Loan
            {
                Duration = Duration,
                LinkedAccountId = SelectedLinkedAccount.AccountId,
                Value = Value,
            };

            var validateResponse = await _tcService.Loans.ValidateLoanRequest(newLoan);
            if(validateResponse.Value.Count > 0)
            {
                var errorMessage = new StringBuilder();
                foreach (var validateError in validateResponse.Value)
                {
                    errorMessage.AppendLine(validateError);
                }
                _messageBoxService.ShowWarningBox(errorMessage.ToString(), "Validate Request failed!");
                return;
            }

            var response = await _tcService.Loans.CreateLoan(new LoanRequest
            {
                Loan = newLoan,
                User = user.Value
            });

            if (response is IErrorResponse<bool> error)
            {
                _messageBoxService.ShowWarningBox(error.Error, "Loan Create Failed!");
                return;
            }
            else
            {
                _messageBoxService.ShowMessageBox("Loan A/c created.", "Success");

                // Go to Main window
                var thisWindow = Application.Current.Windows
                    .Cast<Window>()
                    .FirstOrDefault(w => w.Title == "LoanWindow");
                thisWindow.Close();
            }
        }
    }
}
