using GoodBank.DTO;
using GoodBank.UI_one_client_account;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace GoodBank.UI_clients
{
	/// <summary>
	/// Interaction logic for VIPclientsWindow.xaml
	/// </summary>
	public partial class VIPclientsWindow : Window
	{
		private ObservableCollection<ClientDTO>  vipClients  { get; set; }
		private ObservableCollection<AccountDTO> vipAccoutns { get; set; }
		public VIPclientsWindow()
		{
			InitializeComponent();

			ShowVIPClients();
			ShowVIPAccounts();
		}

		private void ShowVIPClients()
		{
			VIPclientsDataGrid.ItemsSource = vipClients;
		}

		private void ShowVIPAccounts()
		{
			VIPaccountsDataGrid.ItemsSource = vipAccoutns;
		}
		private void VIPWinMenu_SelectClient_Click(object sender, RoutedEventArgs e)
		{

		}

		private void VIPWinMenu_AddClient_Click(object sender, RoutedEventArgs e)
		{
			AddVIPClientWindow addVIPclientWin = new AddVIPClientWindow();
			bool? result = addVIPclientWin.ShowDialog();
			ClientDTO ccc = addVIPclientWin.newClientData;
		}

		private void VIPWinMenu_SelectAccount_Click(object sender, RoutedEventArgs e)
		{

		}

		private void VIPWinMenu_Search_Click(object sender, RoutedEventArgs e)
		{

		}
	}
}
