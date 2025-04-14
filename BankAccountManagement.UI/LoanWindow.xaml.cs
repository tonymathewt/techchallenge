using BankAccountManagement.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BankAccountManagement.UI
{
    /// <summary>
    /// Interaction logic for LoanWindow.xaml
    /// </summary>
    public partial class LoanWindow : Window
    {
        public LoanWindow()
        {
            InitializeComponent();
            Loaded += LoanWindow_Loaded;
        }

        private async void LoanWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Access the ViewModel from DataContext
            if (DataContext is LoanViewModel viewModel)
            {
                await viewModel.PopulateModels();
            }
        }
    }
}
