using BankAccountManagement.UI.ViewModels;
using System.Windows;

namespace BankAccountManagement.UI
{
    /// <summary>
    /// Interaction logic for TransferWindow.xaml
    /// </summary>
    public partial class TransferWindow : Window
    {
        public TransferWindow()
        {
            InitializeComponent();
            Loaded += TransferWindow_Loaded;
        }

        private async void TransferWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Access the ViewModel from DataContext
            if (DataContext is TransferMoneyViewModel viewModel)
            {
                await viewModel.FetchUserAccounts();
            }
        }
    }
}
