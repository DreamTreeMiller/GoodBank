using GoodBankNS.Binding_UI_CondeBehind;
using GoodBankNS.ClientClasses;
using GoodBankNS.DTO;
using GoodBankNS.Interfaces_Data;
using GoodBankNS.UI_one_client_account;
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
	public partial class VIPclientsWindow : Window
	{
		private BankActions BA;
		public VIPclientsWindow(BankActions ba)
		{
			InitializeComponent();
			InitializeView(ba);
			ShowVIPClients();
			ShowVIPAccounts();
		}

		#region Инициализация обработчиков кнопок, вида, списков

		/// <summary>
		/// Привязываем 
		/// </summary>
		/// <param name="ui"></param>
		private void InitializeView(BankActions ba)
		{
			BA = ba;
			VIPclientsTelCB.IsChecked	= true;
			VIPclientsEmailCB.IsChecked = true;
		}
		private void ShowVIPClients()
		{
			var vipClients = BA.Clients.GetClientsList<IClientVIP>();
			VIPclientsDataGrid.ItemsSource = vipClients;
			VIPclientsTotalNumTB.Text = $"{vipClients.Count}";

		}

		private void ShowVIPAccounts()
		{
			VIPaccountsDataGrid.ItemsSource = BA.Accounts.GetAccountsList(ClientType.VIP);
		}

		#endregion

		private void VIPWinMenu_SelectClient_Click(object sender, RoutedEventArgs e)
		{
			ClientWindow clientWindow = new ClientWindow();
			clientWindow.ShowDialog();
		}

		private void VIPWinMenu_AddClient_Click(object sender, RoutedEventArgs e)
		{
			AddIndividualClientWindow addVIPclientWin = new AddIndividualClientWindow();
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

		#region Clients DataGrid CheckBoxes handlers

		private void VIPclientsBirthDateCB_Click(object sender, RoutedEventArgs e)
		{
			if(VIPclientsBirthDateCB.IsChecked == true)
				CreationDateColumn.Visibility = Visibility.Visible;
			else
				CreationDateColumn.Visibility = Visibility.Collapsed;
		}

		private void VIPclientsPassportCB_Click(object sender, RoutedEventArgs e)
		{
			if (VIPclientsPassportCB.IsChecked == true)
				PassportOrTINColumn.Visibility = Visibility.Visible;
			else
				PassportOrTINColumn.Visibility = Visibility.Collapsed;
		}

		private void VIPclientsTelCB_Click(object sender, RoutedEventArgs e)
		{
			if (VIPclientsTelCB.IsChecked == true)
				TelephoneColumn.Visibility = Visibility.Visible;
			else
				TelephoneColumn.Visibility = Visibility.Collapsed;
		}

		private void VIPclientsEmailCB_Click(object sender, RoutedEventArgs e)
		{
			if (VIPclientsEmailCB.IsChecked == true)
				EmailColumn.Visibility = Visibility.Visible;
			else
				EmailColumn.Visibility = Visibility.Collapsed;
		}

		private void VIPclientsAddressCB_Click(object sender, RoutedEventArgs e)
		{
			if (VIPclientsAddressCB.IsChecked == true)
				AddressColumn.Visibility = Visibility.Visible;
			else
				AddressColumn.Visibility = Visibility.Collapsed;
		}

		private void VIPclientsClosedAccountsCB_Click(object sender, RoutedEventArgs e)
		{
			if (VIPclientsClosedAccountsCB.IsChecked == true)
				NummberOfClosedAccountsColumn.Visibility = Visibility.Visible;
			else
				NummberOfClosedAccountsColumn.Visibility = Visibility.Collapsed;
		}

		#endregion

		#region Accounts DataGrid CheckBoxes handlers

		private void VIPCurrentAccountsCB_Click(object sender, RoutedEventArgs e)
		{
			// Если все другие галочки уже сняты, и эта тоже только что была снята
			if (VIPCurrentAccountsCB.IsChecked == false &&
					   VIPDepositsCB.IsChecked == false &&
						VIPCreditsCB.IsChecked == false &&
				 VIPClosedAccountsCB.IsChecked == false)
			{
				// То устанавливаем галочку обратно и выходим
				VIPCurrentAccountsCB.IsChecked = true;
				return;
			}

			if (VIPCurrentAccountsCB.IsChecked == true)
				VIPCurrAccountColumn.Visibility = Visibility.Visible;
			else
				VIPCurrAccountColumn.Visibility = Visibility.Collapsed;
		}

		private void VIPDepositsCB_Click(object sender, RoutedEventArgs e)
		{
			// Если все другие галочки уже сняты, и эта тоже только что была снята
			if (VIPCurrentAccountsCB.IsChecked == false &&
					   VIPDepositsCB.IsChecked == false &&
						VIPCreditsCB.IsChecked == false &&
				 VIPClosedAccountsCB.IsChecked == false)
			{
				// То устанавливаем галочку обратно и выходим
				VIPDepositsCB.IsChecked = true;
				return;
			}

			if (VIPDepositsCB.IsChecked == true)
				VIPDepositColumn.Visibility = Visibility.Visible;
			else
				VIPDepositColumn.Visibility = Visibility.Collapsed;
		}

		private void VIPCreditsCB_Click(object sender, RoutedEventArgs e)
		{
			// Если все другие галочки уже сняты, и эта тоже только что была снята
			if (VIPCurrentAccountsCB.IsChecked == false &&
					   VIPDepositsCB.IsChecked == false &&
						VIPCreditsCB.IsChecked == false &&
				 VIPClosedAccountsCB.IsChecked == false)
			{
				// То устанавливаем галочку обратно и выходим
				VIPCreditsCB.IsChecked = true;
				return;
			}

			if (VIPCreditsCB.IsChecked == true)
				VIPCreditColumn.Visibility = Visibility.Visible;
			else
				VIPCreditColumn.Visibility = Visibility.Collapsed;
		}

		private void VIPClosedAccountsCB_Click(object sender, RoutedEventArgs e)
		{
			// Если все другие галочки уже сняты, и эта тоже только что была снята
			if (VIPCurrentAccountsCB.IsChecked == false &&
					   VIPDepositsCB.IsChecked == false &&
						VIPCreditsCB.IsChecked == false &&
				 VIPClosedAccountsCB.IsChecked == false)
			{
				// То устанавливаем галочку обратно и выходим
				VIPClosedAccountsCB.IsChecked = true;
				return;
			}

			if (VIPClosedAccountsCB.IsChecked == true)
				VIPClosedDateColumn.Visibility = Visibility.Visible;
			else
				VIPClosedDateColumn.Visibility = Visibility.Collapsed;
		}

		#endregion
	}
}
