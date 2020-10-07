using GoodBankNS.UserControlsLists;
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

namespace GoodBankNS.UI_one_client_account
{
	/// <summary>
	/// Interaction logic for ClientWindow.xaml
	/// </summary>
	public partial class ClientWindow : Window
	{
		private AccountsList		 accountsListView;
		private AccountsViewNameTags alntag;

		public ClientWindow()
		{
			InitializeComponent();
			InitializeAccountsView();
		}

		private void InitializeAccountsView()
		{
			accountsListView = new AccountsList();
			AccountsList.Content = accountsListView;
			accountsListView.WordAccountsTag.Visibility = Visibility.Collapsed;
		}

		private void ClientWindow_EditClient_Click(object sender, RoutedEventArgs e)
		{

		}
	}
}
