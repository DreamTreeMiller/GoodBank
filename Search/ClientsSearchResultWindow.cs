using GoodBankNS.Binding_UI_CondeBehind;
using GoodBankNS.DTO;
using GoodBankNS.Interfaces_Data;
using GoodBankNS.UI_one_client_account;
using GoodBankNS.UserControlsLists;
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

namespace GoodBankNS.Search
{
	/// <summary>
	/// Interaction logic for PersonsSearchResultWindow.xaml
	/// </summary>
	public partial class ClientsSearchResultWindow : Window
	{
		private BankActions BA;
		private ClientsList clientsListUserControl;

		public ClientsSearchResultWindow(
			BankActions ba, 
			ObservableCollection<IClientDTO> searchResult, 
			WindowID searchType)
		{
			InitializeComponent();
			InitializeBankActionsAndClientsListUserControl(ba, searchResult, searchType);
		}

		private void InitializeBankActionsAndClientsListUserControl(
			BankActions ba, 
			ObservableCollection<IClientDTO> searchResult,
			WindowID searchType)
		{
			BA = ba;
			ClientsViewNameTags tags = new ClientsViewNameTags(searchType);
			clientsListUserControl = new ClientsList(tags);
			clientsListUserControl.ClientsDataGrid.ItemsSource = searchResult;
			clientsListUserControl.ClientsTotalNumberValue.Text = $"{searchResult.Count:N0}";
			ClientsList.Content = clientsListUserControl;
		}
		private void btn_SelectClient_Click(object sender, RoutedEventArgs e)
		{
			var client = clientsListUserControl.ClientsDataGrid.SelectedItem as ClientDTO;
			if (client == null)
			{
				MessageBox.Show("Выберите клиента для показа");
				return;
			}
			ClientWindow clientWindow = new ClientWindow(BA, client);
			clientWindow.ShowDialog();
		}
	}
}
