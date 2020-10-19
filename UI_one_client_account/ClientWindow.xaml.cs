﻿using GoodBankNS.DTO;
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
using GoodBankNS.Interfaces_Data;

namespace GoodBankNS.UI_one_client_account
{
	/// <summary>
	/// Interaction logic for ClientWindow.xaml
	/// </summary>
	public partial class ClientWindow : Window
	{
		private BankActions			 BA;
		private AccountsList		 accountsListView;
		private WindowID			 wid	= WindowID.EditClientVIP;
		private IClientDTO			 client = new ClientDTO();

		public ClientWindow(BankActions ba, IClientDTO client)
		{
			InitializeComponent();
			InitializeAccountsView(ba, client);
			ShowAccounts();
		}

		private void InitializeAccountsView(BankActions ba, IClientDTO client)
		{
			BA = ba;
			OrganizationInfo.Visibility = Visibility.Collapsed;
			PersonalInfo.Visibility		= Visibility.Visible;
			this.client					= client;

			switch (this.client.ClientType)
			{
				case ClientType.VIP:
					Title						= "ВИП";
					MainTitle.Text				= "ОЧЕНЬ ВАЖНАЯ ПЕРСОНА";
					break;
				case ClientType.Simple:
					Title						= "Физик";
					MainTitle.Text				= "ФИЗИК";
					wid							= WindowID.EditClientSIM;
					break;
				case ClientType.Organization:
					Title						= "Юрик";
					MainTitle.Text				= "ЮРИК";
					OrganizationInfo.Visibility = Visibility.Visible;
					PersonalInfo.Visibility		= Visibility.Collapsed;
					wid							= WindowID.EditClientORG;
					break;
			}

			ClientInfo.DataContext	= client;
			accountsListView		= new AccountsList();
			AccountsList.Content	= accountsListView;

			// Убираем словов "сундучки"
			accountsListView.WordAccountsTag.Visibility  = Visibility.Collapsed;
			accountsListView.ClientNameColumn.Visibility = Visibility.Collapsed;
		}

		private void ShowAccounts()
		{
			var accountsList = BA.Accounts.GetClientAccounts(client.ID);
			accountsListView.AccountsDataGrid.ItemsSource = accountsList.accList;
			accountsListView.AccountsTotalNumberValue.Text = $"{accountsList.accList.Count:N0}";
			accountsListView.CurrentTotalAmount.Text = $"{accountsList.totalCurr:N2}";
			accountsListView.DepositsTotalAmount.Text = $"{accountsList.totalDeposit:N2}";
			accountsListView.CreditsTotalAmount.Text = $"{accountsList.totalCredit:N2}";
		}
		private void ClientWindow_EditClient_Click(object sender, RoutedEventArgs e)
		{
			var tags				= new AddEditClientNameTags(wid);
			var editClientWindow	= new AddEditClientWindow(tags, client);
			var result = editClientWindow.ShowDialog();
			if (result != true) return;

			// Обновляем визуально редактируемый элемент
			// Обновляем базу клиентов.
			// Эти два действия должны всегда быть вместе!
			(this.client as ClientDTO).UpdateMyself(editClientWindow.tmpClient as ClientDTO);
			BA.Clients.UpdateClient(editClientWindow.tmpClient);
		}

		private void ClientWindow_AccountDetails_Click(object sender, RoutedEventArgs e)
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
	}
}
