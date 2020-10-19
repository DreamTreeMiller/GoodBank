using GoodBankNS.AccountClasses;
using GoodBankNS.BankInside;
using GoodBankNS.Binding_UI_CondeBehind;
using GoodBankNS.ClientClasses;
using GoodBankNS.DTO;
using GoodBankNS.Interfaces_Data;
using GoodBankNS.UserControlsLists;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
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
	/// Interaction logic for AccountWindow.xaml
	/// </summary>
	public partial class AccountWindow : Window
	{
		BankActions BA;
		IClientDTO  client;
		IAccountDTO Acc;


		public AccountWindow(BankActions ba, IAccountDTO acc)
		{
			InitializeComponent();
			InitializeClassScopeVars(ba, acc);
			InitializeAccountDetails();
			InitializeClientDetails();
		}

		private void InitializeClassScopeVars(BankActions ba, IAccountDTO acc)
		{
			BA		= ba;
			client	= new ClientDTO(BA.Clients.GetClientByID(acc.ClientID));
			Acc		= acc;
		}

		private void InitializeAccountDetails()
		{
			AccountWindowNameTags tags = new AccountWindowNameTags(Acc.AccType);
			Title							= tags.SystemWindowTitle;
			MainTitle.Text					= tags.WindowHeader;
			WithdrawCashButton.Visibility	= tags.WithdrawCashButtonVisibility;
			WireButton.Visibility			= tags.WireButtonVisibility;
			DepositPart.Visibility			= tags.DepositPartVisibility;

			// Без капитализации указываем счет для накопления процентов
			if(Acc.Compounding == false)
			{
				CompoundAccLabel.Visibility = Visibility.Visible;
				CompoundAccValue.Visibility = Visibility.Visible;
			}

			AccountInfo.DataContext			= Acc;
		}

		private void InitializeClientDetails()
		{
			if (client.ClientType == ClientType.Organization)
			{
				OrganizationInfo.Visibility = Visibility.Visible;
				PersonalInfo.Visibility		= Visibility.Collapsed;
			}
			else
			{
				OrganizationInfo.Visibility = Visibility.Collapsed;
				PersonalInfo.Visibility = Visibility.Visible;
			}
			ClientInfo.DataContext = client;
		}

		private void TopUpButton_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("Top Up Account");
		}

		private void WithdrawCashButton_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("Withdraw cash");
		}

		private void WireButton_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("Wire to another account");
		}

		private void CloseAccountButton_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("Close account");
		}
	}
}
