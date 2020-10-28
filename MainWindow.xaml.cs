using GoodBankNS.Binding_UI_CondeBehind;
using GoodBankNS.Imitation;
using GoodBankNS.Interfaces_Data;
using GoodBankNS.UI_clients;
using GoodBankNS.BankInside;
using System.Windows;
using GoodBankNS.UserControlsLists;
using GoodBankNS.Search;
using System.Windows.Documents;
using GoodBankNS.DTO;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace GoodBankNS
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private IGoodBank GoodBank;
		private BankActions BA;
		public string BankFoundationDay = "Основан " 
			+ $"{GoodBankNS.BankInside.GoodBank.BankFoundationDay:D}"
			;

		public MainWindow()
		{
			InitializeComponent();
			InitializeBank();
			InitializeWelcomeScreenMessages();
			
		}

		private void InitializeBank()
		{
			GoodBank = new GoodBank();
			BA		 = new BankActions(GoodBank);
		}

		private void InitializeWelcomeScreenMessages()
		{
			BankFoundationDayMessage.Text = BankFoundationDay;
			BankTodayDate.Text = $"Сегодня {GoodBankNS.BankInside.GoodBank.Today:dd MMMM yyyy} г.";
		}

		private void VipClientsDeptButton_Click(object sender, RoutedEventArgs e)
		{
			DepartmentWindow vipClientsWin = new DepartmentWindow(WindowID.DepartmentVIP, BA);
			vipClientsWin.ShowDialog();
		}

		private void SimpleClientsDeptButton_Click(object sender, RoutedEventArgs e)
		{
			DepartmentWindow simpleClientsWin = new DepartmentWindow(WindowID.DepartmentSIM, BA);
			simpleClientsWin.ShowDialog();
		}

		private void OrgClientsDeptButton_Click(object sender, RoutedEventArgs e)
		{
			DepartmentWindow orgClientsWin = new DepartmentWindow(WindowID.DepartmentORG, BA);
			orgClientsWin.ShowDialog();
		}

		private void BankManagerButton_Click(object sender, RoutedEventArgs e)
		{
			DepartmentWindow allClientsWin = new DepartmentWindow(WindowID.DepartmentALL, BA);
			allClientsWin.ShowDialog();
		}

		private void SearchButton_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("Функция поиска в разработке");
		}

		private void TimeMachineButton_Click(object sender, RoutedEventArgs e)
		{
			BA.Accounts.AddOneMonth();
			BankTodayDate.Text = $"Сегодня {GoodBankNS.BankInside.GoodBank.Today:dd MMMM yyyy} г.";
			MessageBox.Show("Время в мире, где существует банк, ушло на месяц вперёд.\n"
						  + "Пересчитаны проценты на всех счетах.");

		}

		private void GenerateButton_Click(object sender, RoutedEventArgs e)
		{
			var gw = new GenerateWindow();
			var result = gw.ShowDialog();
			if (result != true) return;
			Generate.Bank(BA, gw.vipClients, gw.simClients, gw.orgClients);
			MessageBox.Show("Клиенты и счета созданы!");
		}

		private void SearchPeopleButton_Click(object sender, RoutedEventArgs e)
		{
			EnterSearchRequestForIndividualWindow esriw = new EnterSearchRequestForIndividualWindow();
			var result = esriw.ShowDialog();

			if (result != true) return;

			ObservableCollection<IClientDTO> searchResult = BA.Clients.GetClientsList(esriw.CheckAllFields);
			PersonsSearchResultWindow psrw = new PersonsSearchResultWindow();

			ClientsViewNameTags tags = new ClientsViewNameTags(WindowID.DepartmentALL);
			ClientsList cluc = new ClientsList(tags);
			cluc.ClientsDataGrid.ItemsSource = searchResult;

			psrw.ClientsList.Content = cluc;

			psrw.ShowDialog();
		}

		private void SearchOrgButton_Click(object sender, RoutedEventArgs e)
		{

		}

		private void SearchAccountsButton_Click(object sender, RoutedEventArgs e)
		{

		}
	}
}
