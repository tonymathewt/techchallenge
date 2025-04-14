using System.Windows;
using BankAccountManagement.UI.Services.Interfaces;

namespace BankAccountManagement.UI.Services
{
    internal class MessageBoxService : IMessageBoxService
    {
        public bool ShowMessageBox(string message, string caption)
        {
            var result = MessageBox.Show(message, caption, MessageBoxButton.YesNo);
            return result == MessageBoxResult.Yes;
        }

        public bool ShowWarningBox(string message, string caption)
        {
            var result = MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Warning);
            return result == MessageBoxResult.OK;
        }

        public bool ShowErrorBox(string message, string caption)
        {
            var result = MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Error);
            return result == MessageBoxResult.OK;
        }
    }
}
