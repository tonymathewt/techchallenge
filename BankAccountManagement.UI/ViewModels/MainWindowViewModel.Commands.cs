using System.Windows.Input;
using BankAccountManagement.UI.Commands;

namespace BankAccountManagement.UI.ViewModels;

public partial class MainWindowViewModel
{
    public ICommand ApplyLoanCommandAsync
    {
        get
        {
            if (_saveCommandAsync == null)
            {
                _saveCommandAsync = new RelayCommand(async param => await RequestLoanAsync());
            }

            return _saveCommandAsync;
        }
    }

    public ICommand TransferMoneyCommand
    {
        get
        {
            if (_transferCommand == null)
            {
                _transferCommand = new RelayCommand(param =>  TransferMoney());
            }

            return _transferCommand;
        }
    }

    public ICommand RefreshCommandAsync
    {
        get
        {
            if (_refreshCommand == null)
            {
                _refreshCommand = new RelayCommand(param => FetchUserAccounts());
            }

            return _refreshCommand;
        }
    }    
}
