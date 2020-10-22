using GoodBankNS.AccountClasses;
using GoodBankNS.Binding_UI_CondeBehind;
using GoodBankNS.ClientClasses;
using GoodBankNS.DTO;
using GoodBankNS.Interfaces_Data;
using GoodBankNS.UserControlsLists;
using System;
using System.Windows;

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

		public bool needUpdate = false;

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
				InterestAccumulationAccLabel.Visibility = Visibility.Visible;
				InterestAccumulationAccValue.Visibility = Visibility.Visible;
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
			if (!Acc.Topupable)
			{
				MessageBox.Show("Пополнение невозможно!");
				return;
			}
			EnterCashAmountWindow cashWin = new EnterCashAmountWindow();
			var result = cashWin.ShowDialog();
			if (result != true) return;

			IAccount updatedAcc = BA.Accounts.TopUp(Acc.ID, cashWin.amount);
			Balance.Text = $"{updatedAcc.Balance:C2}";
			needUpdate = true;
		}

		private void WithdrawCashButton_Click(object sender, RoutedEventArgs e)
		{
			if (!Acc.WithdrawalAllowed)
			{
				MessageBox.Show("Снятие невозможно!");
				return;
			}
			EnterCashAmountWindow cashWin = new EnterCashAmountWindow();
			var result = cashWin.ShowDialog();
			if (result != true) return;

			if(Acc.Balance < cashWin.amount)
			{
				MessageBox.Show("Недостаточно средств для снятия!");
				return;
			}
			IAccount updatedAcc = BA.Accounts.Withdraw(Acc.ID, cashWin.amount);
			Balance.Text = $"{updatedAcc.Balance:C2}";
			needUpdate = true;
		}

		private void WireButton_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("Wire to another account");
		}

		private void CloseAccountButton_Click(object sender, RoutedEventArgs e)
		{
			IAccount closedAcc = null;
			if (Acc.Balance < 0)
			{
				MessageBox.Show("Невозможно закрыть счет, на котором есть долг");
				return;
			}

			if (Acc.Balance > 0)
			{
				MessageBox.Show($"Получите ваши денюшки\n в размере {Acc.Balance:N2} руб.");
				BA.Accounts.Withdraw(Acc.ID, Acc.Balance);
			}

			closedAcc = BA.Accounts.CloseAccount(Acc.ID);

			Balance.Text			 = $"{closedAcc.Balance:C2}";
			ClosedDate.Text			 = $"{(DateTime)closedAcc.Closed:dd.MM.yyyy}";
			TopupableFlag.Text		 = "нет";
			WithdrawAllowedFlag.Text = "нет";

			needUpdate = true;


		}
	}
}
