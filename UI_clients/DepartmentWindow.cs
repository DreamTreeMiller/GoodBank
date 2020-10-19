using GoodBankNS.Binding_UI_CondeBehind;
using GoodBankNS.ClientClasses;
using GoodBankNS.DTO;
using GoodBankNS.Interfaces_Data;
using GoodBankNS.UI_one_client_account;
using GoodBankNS.UserControlsLists;
using System.Collections.ObjectModel;
using System.Windows;
using System.Data;
using System.Windows.Controls;
using System.Xml;
using System.Windows.Media;

namespace GoodBankNS.UI_clients
{
	/// <summary>
	/// Interaction logic for VIPclientsWindow.xaml
	/// </summary>
	public partial class DepartmentWindow : Window
	{
		private BankActions				BA;
		private WindowID				wid;

		private WindowNameTags			deptwinnametags;
		private ClientsList				clientsListView;
		private ClientsViewNameTags		clntag;
		private WindowID				addClientWID;
		ObservableCollection<ClientDTO> clientsList = new ObservableCollection<ClientDTO>();

		private ClientType				ClientTypeForAccountsList;
		private AccountsList			accountsListView;

		public DepartmentWindow(WindowID wid, BankActions ba)
		{
			InitializeComponent();
			InitializeView(wid, ba);
			InitializeClientsAndWindowTypes();
			ShowAccounts();
		}

		#region Инициализация обработчиков кнопок, вида, списков

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

		private void InitializeClientsAndWindowTypes()
		{
			switch (wid)
			{
				case WindowID.DepartmentVIP:
					clientsList = BA.Clients.GetClientsList<IClientVIP>();
					ClientTypeForAccountsList	= ClientType.VIP;
					addClientWID				= WindowID.AddClientVIP;
					break;
				case WindowID.DepartmentSIM:
					clientsList = BA.Clients.GetClientsList<IClientSimple>();
					ClientTypeForAccountsList	= ClientType.Simple;
					addClientWID				= WindowID.AddClientSIM;
					break;
				case WindowID.DepartmentORG:
					clientsList = BA.Clients.GetClientsList<IClientOrg>();
					ClientTypeForAccountsList	= ClientType.Organization;
					addClientWID				= WindowID.AddClientORG;
					break;
				case WindowID.DepartmentALL:
					clientsList = BA.Clients.GetClientsList<IClient>();
					ClientTypeForAccountsList	= ClientType.All;
					addClientWID				= WindowID.AddClientALL;
					break;
			}
			clientsListView.ClientsDataGrid.ItemsSource = clientsList;
			clientsListView.ClientsTotalNumberValue.Text = $"{clientsList.Count:N0}";
		}

		private void ShowAccounts()
		{
			var accountsList = BA.Accounts.GetAccountsList(ClientTypeForAccountsList);
			accountsListView.AccountsDataGrid.ItemsSource = accountsList.accList;
			accountsListView.AccountsTotalNumberValue.Text = $"{accountsList.accList.Count:N0}";
			accountsListView.CurrentTotalAmount.Text = $"{accountsList.totalCurr:N2}";
			accountsListView.DepositsTotalAmount.Text = $"{accountsList.totalDeposit:N2}";
			accountsListView.CreditsTotalAmount.Text = $"{accountsList.totalCredit:N2}";
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
			ClientWindow clientWindow = new ClientWindow(BA, client);
			clientWindow.ShowDialog();
		}

		private void WinMenu_AddClient_Click(object sender, RoutedEventArgs e)
		{
			AddEditClientNameTags nameTags  = new AddEditClientNameTags(addClientWID);
			IClientDTO newClient = null;
			AddEditClientWindow addСlientWin = new AddEditClientWindow(nameTags, newClient);
			bool? result = addСlientWin.ShowDialog();
			
			if (result != true) return;
			// Добавляем нового клиента в базу в бэкэнде
			newClient = addСlientWin.tmpClient;
			IClientDTO addedClient = BA.Clients.AddClient(newClient);

			// Добавляем нового клиента в список на экране
			AddNewClientToDataGrid(addedClient);
		}

		/// <summary>
		/// Добавляет только что созданного клиента к списку на экране
		/// Это делается вместо получения заново всего списка клиентов одного типа
		/// </summary>
		/// <param name="addedClient"></param>
		private void AddNewClientToDataGrid(IClientDTO addedClient)
		{
			clientsList.Add(addedClient as ClientDTO);
			clientsListView.ClientsTotalNumberValue.Text = $"{clientsList.Count}";
		}

		private void WinMenu_SelectAccount_Click(object sender, RoutedEventArgs e)
		{
			var account = accountsListView.AccountsDataGrid.SelectedItem as AccountDTO;
			if (account == null)
			{
				MessageBox.Show("Выберите счет для показа");
				return;
			}
			IClient client = BA.Clients.GetClientByID(account.ClientID);
			AccountWindow accountWindow = new AccountWindow(BA, account);
			accountWindow.ShowDialog();

		}

		private void WinMenu_Search_Click(object sender, RoutedEventArgs e)
		{

		}

	}


}
