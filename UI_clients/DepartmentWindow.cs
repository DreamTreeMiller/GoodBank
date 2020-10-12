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
		private WindowID wid;

		private WindowNameTags deptwinnametags;
		private ClientsList clientsListView;
		private ClientsViewNameTags clntag;

		private ClientType ClientTypeForAccountsList;
		private AccountsList accountsListView;
		private AccountsViewNameTags alntag;

		public DepartmentWindow(WindowID wid, BankActions ba)
		{
			InitializeComponent();
			InitializeView(wid, ba);
			ShowClients();
			SetClientTypeForAccountsList();
			ShowAccounts();
		}

		#region Инициализация обработчиков кнопок, вида, списков

		/// <summary>
		/// Привязываем 
		/// </summary>
		/// <param name="ui"></param>
		private void InitializeView(WindowID wid, BankActions ba)
		{
			// Прикручиваем банк с обработчиками всех действий над счетами
			BA = ba;
			this.wid = wid;

			// Вставляем нужные надписи в окошко департаментов
						 deptwinnametags = new WindowNameTags(wid);
								   Title = deptwinnametags.SystemWindowTitle;
						  MainTitle.Text = deptwinnametags.WindowHeader;
			WinMenu_SelectClient.Content = deptwinnametags.SelectClientTag;
			   WinMenu_AddClient.Content = deptwinnametags.AddClientTag;

			// Создаем область для списка клиентов. Вставляем нужные надписи
			clntag				= new ClientsViewNameTags(wid);
			clientsListView		= new ClientsList(clntag);
			ClientsList.Content = clientsListView;

			// Создаем область для списка счетов. Вставляем нужные надписи
			accountsListView	= new AccountsList();
			AccountsList.Content = accountsListView;
		}

		private void ShowClients()
		{
			ObservableCollection<ClientDTO> clientsList = new ObservableCollection<ClientDTO>();

			switch (wid)
			{
				case WindowID.DepartmentVIP:
					clientsList = BA.Clients.GetClientsList<IClientVIP>();
					break;
				case WindowID.DepartmentSIM:
					clientsList = BA.Clients.GetClientsList<IClientSimple>();
					break;
				case WindowID.DepartmentORG:
					clientsList = BA.Clients.GetClientsList<IClientOrg>();
					break;
				case WindowID.DepartmentALL:
					clientsList = BA.Clients.GetClientsList<IClient>();
					break;
			}
			clientsListView.ClientsDataGrid.ItemsSource = clientsList;
			clientsListView.ClientsTotalNumberValue.Text = $"{clientsList.Count}";
		}

		private void SetClientTypeForAccountsList()
		{
			switch (wid)
			{
				case WindowID.DepartmentVIP:
					ClientTypeForAccountsList = ClientType.VIP;
					return;
				case WindowID.DepartmentSIM:
					ClientTypeForAccountsList = ClientType.Simple;
					return;
				case WindowID.DepartmentORG:
					ClientTypeForAccountsList = ClientType.Organization;
					return;
				case WindowID.DepartmentALL:
					ClientTypeForAccountsList = ClientType.All;
					return;
			}
		}

		private void ShowAccounts()
		{
			accountsListView.AccountsDataGrid.ItemsSource = 
				BA.Accounts.GetAccountsList(ClientTypeForAccountsList);
		}

		#endregion

		private void WinMenu_SelectClient_Click(object sender, RoutedEventArgs e)
		{
			var client = clientsListView.ClientsDataGrid.SelectedItem as ClientDTO;
			if (client == null)
			{
				MessageBox.Show("Выберите клиента для показа");
				return;
			}
			ClientWindow clientWindow = new ClientWindow(client);
			clientWindow.ShowDialog();
		}

		private void WinMenu_AddClient_Click(object sender, RoutedEventArgs e)
		{
			AddEditClientNameTags nameTags  = new AddEditClientNameTags(wid);
			AddClientWindow addVIPclientWin = new AddClientWindow(nameTags);
			bool? result = addVIPclientWin.ShowDialog();
			
			if (result != true) return;
			IClientDTO newClient = addVIPclientWin.newClientData;
			BA.Clients.AddClient(newClient);
			ShowClients();
		}

		private void WinMenu_SelectAccount_Click(object sender, RoutedEventArgs e)
		{

		}

		private void WinMenu_Search_Click(object sender, RoutedEventArgs e)
		{

		}

	}
}
