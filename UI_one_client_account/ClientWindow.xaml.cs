using GoodBankNS.DTO;
using GoodBankNS.UserControlsLists;
using GoodBankNS.ClientClasses;
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
using GoodBankNS.Binding_UI_CondeBehind;

namespace GoodBankNS.UI_one_client_account
{
	/// <summary>
	/// Interaction logic for ClientWindow.xaml
	/// </summary>
	public partial class ClientWindow : Window
	{
		private BankActions BA;
		private AccountsList		 accountsListView;
		private AccountsViewNameTags alntag;
		private WindowID			 wid = WindowID.EditClientVIP;
		private ClientDTO			 client = new ClientDTO();

		public ClientWindow(BankActions ba, ClientDTO client)
		{
			InitializeComponent();
			InitializeAccountsView(ba, client);
		}

		private void InitializeAccountsView(BankActions ba, ClientDTO client)
		{
			BA = ba;
			OrganizationInfo.Visibility = Visibility.Collapsed;
			PersonalInfo.Visibility		= Visibility.Visible;
			this.client					= client;

			switch (this.client.ClientType)
			{
				case ClientType.VIP:
					Title			= "ВИП";
					MainTitle.Text	= "ОЧЕНЬ ВАЖНАЯ ПЕРСОНА";
					break;
				case ClientType.Simple:
					Title			= "Физик";
					MainTitle.Text	= "ФИЗИК";
					wid				= WindowID.EditClientSIM;
					break;
				case ClientType.Organization:
					Title						= "Юрик";
					MainTitle.Text				= "ЮРИК";
					OrganizationInfo.Visibility = Visibility.Visible;
					PersonalInfo.Visibility		= Visibility.Collapsed;
					wid							= WindowID.EditClientORG;
					break;
			}

			ClientInfo.DataContext = client;
			accountsListView = new AccountsList();
			AccountsList.Content = accountsListView;

			// Убираем словов "сундучки"
			accountsListView.WordAccountsTag.Visibility = Visibility.Collapsed;
		}

		private void ClientWindow_EditClient_Click(object sender, RoutedEventArgs e)
		{
			var tags				= new AddEditClientNameTags(wid);
			var editClientWindow	= new AddEditClientWindow(tags, client);
			var result = editClientWindow.ShowDialog();
			if (result != true) return;
			this.client.UpdateMyself(editClientWindow.tmpClient);
			BA.Clients.UpdateClient(editClientWindow.tmpClient);
		}
	}
}
