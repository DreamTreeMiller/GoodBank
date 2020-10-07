using GoodBankNS.Binding_UI_CondeBehind;
using GoodBankNS.ClientClasses;
using GoodBankNS.DTO;
using GoodBankNS.Interfaces_Data;
using GoodBankNS.UI_one_client_account;
using GoodBankNS.UserControlsLists;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
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

namespace GoodBankNS.UI_clients
{
	/// <summary>
	/// Interaction logic for VIPclientsWindow.xaml
	/// </summary>
	public partial class DepartmentWindow : Window
	{
		private BankActions BA;
		private ClientsList clientsListView;
		private ClientsViewNameTags clntag;
		private AccountsList accountsListView;
		private AccountsViewNameTags alntag;
		public DepartmentWindow(BankActions ba, ClientsViewNameTags clntag)
		{
			InitializeComponent();
			InitializeView(ba, clntag);
			ShowVIPClients();
			ShowVIPAccounts();
		}

		#region Инициализация обработчиков кнопок, вида, списков

		/// <summary>
		/// Привязываем 
		/// </summary>
		/// <param name="ui"></param>
		private void InitializeView(BankActions ba, ClientsViewNameTags clntag)
		{
			BA = ba;
			this.clntag = clntag;
			MainTitle.Text = "ОЧЕНЬ ВАЖНЫЕ ПЕРСОНЫ";
			clientsListView = new ClientsList(clntag);
			ClientsList.Content = clientsListView;

			accountsListView = new AccountsList();
			AccountsList.Content = accountsListView;
		}
		private void ShowVIPClients()
		{
			var vipClients = BA.Clients.GetClientsList<IClientVIP>();
			clientsListView.ClientsDataGrid.ItemsSource = vipClients;
			clientsListView.ClientsTotalNumberValue.Text = $"{vipClients.Count}";
		}

		private void ShowVIPAccounts()
		{
			accountsListView.AccountsDataGrid.ItemsSource = BA.Accounts.GetAccountsList(ClientType.VIP);
		}

		#endregion

		private void VIPWinMenu_SelectClient_Click(object sender, RoutedEventArgs e)
		{
			ClientWindow clientWindow = new ClientWindow();
			clientWindow.ShowDialog();
		}

		private void VIPWinMenu_AddClient_Click(object sender, RoutedEventArgs e)
		{
			AddClientWindow addVIPclientWin = new AddClientWindow();
			bool? result = addVIPclientWin.ShowDialog();
			
			if (result != true) return;
			IClientDTO newClient = addVIPclientWin.newClientData;
			BA.Clients.AddClient(newClient);
			ShowVIPClients();
		}

		private void VIPWinMenu_SelectAccount_Click(object sender, RoutedEventArgs e)
		{

		}

		private void VIPWinMenu_Search_Click(object sender, RoutedEventArgs e)
		{

		}

	}
}
