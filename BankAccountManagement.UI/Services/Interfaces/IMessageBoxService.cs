namespace BankAccountManagement.UI.Services.Interfaces
{
    public interface IMessageBoxService
    {
        bool ShowMessageBox(string message, string caption);

        bool ShowWarningBox(string message, string caption);

        bool ShowErrorBox(string message, string caption);
    }
}
