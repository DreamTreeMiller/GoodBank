using Binding_UI_CondeBehind;
using ClientClasses;
using DTO;
using Interfaces_Data;
using UI_one_client_account;
using UserControlsLists;
using System.Collections.ObjectModel;
using System.Windows;

namespace UI_clients
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
		ObservableCollection<IClientDTO> clientsList = new ObservableCollection<IClientDTO>();

		private ClientType				ClientTypeForAccountsList;
		private AccountsList			accountsListView;
		ObservableCollection<IAccountDTO> accountsList = new ObservableCollection<IAccountDTO>();

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

			// Помещаем текущую дату банка в правый верхний угол окна
			BankTodayDate.Text = $"Сегодня {BA.GBDateTime.Today():dd.MM.yyyy}";

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
					clientsList = BA.Clients.GetClientsList(ClientType.VIP);
					ClientTypeForAccountsList	= ClientType.VIP;
					addClientWID				= WindowID.AddClientVIP;
					break;
				case WindowID.DepartmentSIM:
					clientsList = BA.Clients.GetClientsList(ClientType.Simple);
					ClientTypeForAccountsList	= ClientType.Simple;
					addClientWID				= WindowID.AddClientSIM;
					break;
				case WindowID.DepartmentORG:
					clientsList = BA.Clients.GetClientsList(ClientType.Organization);
					ClientTypeForAccountsList	= ClientType.Organization;
					addClientWID				= WindowID.AddClientORG;
					break;
				case WindowID.DepartmentALL:
					clientsList = BA.Clients.GetClientsList(ClientType.All);
					ClientTypeForAccountsList	= ClientType.All;
					addClientWID				= WindowID.AddClientALL;
					break;
			}
			clientsListView.ClientsDataGrid.ItemsSource = clientsList;
			clientsListView.ClientsTotalNumberValue.Text = $"{clientsList.Count:N0}";
		}

		private void ShowAccounts()
		{
			var accList = BA.Accounts.GetAccountsList(ClientTypeForAccountsList);
			accountsList = accList.accList;
			accountsListView.AccountsDataGrid.ItemsSource   = accountsList;
			accountsListView.AccountsTotalNumberValue.Text	= $"{accList.accList.Count:N0}";
			accountsListView.CurrentTotalAmount.Text		= $"{accList.totalCurr:N2}";
			accountsListView.DepositsTotalAmount.Text		= $"{accList.totalDeposit:N2}";
			accountsListView.CreditsTotalAmount.Text		= $"{accList.totalCredit:N2}";
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

			if (clientWindow.clientsNeedUpdate)  InitializeClientsAndWindowTypes();
			if (clientWindow.accountsNeedUpdate) ShowAccounts();
		}

		private void WinMenu_AddClient_Click(object sender, RoutedEventArgs e)
		{
			AddEditClientNameTags nameTags  = new AddEditClientNameTags(addClientWID);
			IClientDTO newClient = null;
			AddEditClientWindow addСlientWin = new AddEditClientWindow(BA, nameTags, newClient);
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
			//IClientDTO client = BA.Clients.GetClientDTObyID(account.ClientID);
			AccountWindow accountWindow = new AccountWindow(BA, account);
			accountWindow.ShowDialog();
			if (accountWindow.clientsNeedUpdate)  InitializeClientsAndWindowTypes();
			if (accountWindow.accountsNeedUpdate) ShowAccounts();
		}

		private void WinMenu_Search_Click(object sender, RoutedEventArgs e)
		{

		}

	}


}
