using BankAccountManagement.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace BankAccountManagement.UI.ViewModels;

public partial class MainWindowViewModel
{
    private string _name;
    private ICommand _saveCommand;
    private ICommand _saveCommandAsync;
    private ICommand _transferCommand;
    private ICommand _refreshCommand;

    public string StatusMessage { get; set; }

    public ObservableCollection<Account> UserAccounts { get; set; }

    public string Name
    {
        get
        {
            return _name;
        }
        set
        {
            _name = value;
            OnPropertyChanged(nameof(Name));
        }
    }
}
