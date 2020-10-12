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

namespace GoodBankNS.UI_one_client_account
{
	/// <summary>
	/// Interaction logic for ClientWindow.xaml
	/// </summary>
	public partial class ClientWindow : Window
	{
		private AccountsList		 accountsListView;
		private AccountsViewNameTags alntag;

		public ClientWindow(ClientDTO client)
		{
			InitializeComponent();
			InitializeAccountsView(client);
		}

		private void InitializeAccountsView(ClientDTO client)
		{
			OrganizationInfo.Visibility = Visibility.Collapsed;
			PersonalInfo.Visibility = Visibility.Visible;

			switch (client.ClientType)
			{
				case ClientType.VIP:
					Title			= "ВИП";
					MainTitle.Text	= "ОЧЕНЬ ВАЖНАЯ ПЕРСОНА";
					break;
				case ClientType.Simple:
					Title			= "Физик";
					MainTitle.Text	= "ФИЗИК";
					break;
				case ClientType.Organization:
					Title						= "Юрик";
					MainTitle.Text				= "ЮРИК";
					OrganizationInfo.Visibility = Visibility.Visible;
					PersonalInfo.Visibility		= Visibility.Collapsed;
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

		}
	}
}
