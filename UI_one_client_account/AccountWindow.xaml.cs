using GoodBankNS.AccountClasses;
using GoodBankNS.Binding_UI_CondeBehind;
using GoodBankNS.ClientClasses;
using GoodBankNS.DTO;
using GoodBankNS.Interfaces_Data;
using GoodBankNS.UserControlsLists;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace GoodBankNS.UI_one_client_account
{
	/// <summary>
	/// Interaction logic for AccountWindow.xaml
	/// </summary>
	public partial class AccountWindow : Window, INotifyPropertyChanged
	{
		#region Account Fields in Window

		private uint		accID;
		private AccountType	accountType;
		private string		accountNumber;
		public	string		AccountNumber
		{
			get => accountNumber;
			set {  accountNumber = value; NotifyPropertyChanged(); }
		}

		private	double		balance;
		public	double		Balance
		{
			get => balance;
			set {  balance = value; NotifyPropertyChanged(); }
		}

		public	double		Interest	{ get; set; }

		public	DateTime	Opened		{ get; set; }

		public	DateTime?	EndDate		{ get; set; }

		private DateTime?	accClosed;
		public	DateTime?	AccClosed 
		{ 
			get => accClosed;
			set {  accClosed = value; NotifyPropertyChanged(); } 
		}

		private bool		topupable;
		public  bool		Topupable 
		{ 
			get => topupable; 
			set {  topupable = value; NotifyPropertyChanged(); }
		}

		private bool		withdrawalAllowed;
		public	bool		WithdrawalAllowed
		{ 
			get => withdrawalAllowed; 
			set {  withdrawalAllowed = value; NotifyPropertyChanged(); }
		}

		public RecalcPeriod RecalcPeriod { get; set; }

		public bool Compounding { get; set; }

		public string InterestAccumulationAccNum { get; set; }

		#endregion

		BankActions BA;
		IClientDTO  client;

		public bool accountsNeedUpdate = false;
		public bool clientsNeedUpdate  = false;

		public event PropertyChangedEventHandler PropertyChanged;

		private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public AccountWindow(BankActions ba, IAccountDTO acc)
		{
			InitializeComponent();
			InitializeClassScopeVars(ba, acc);
			InitializeAccountFieldLabelsAndVisibility();
			InitializeClientDetails();
		}

		private void InitializeClassScopeVars(BankActions ba, IAccountDTO acc)
		{
			BA		= ba;
			client	= new ClientDTO(BA.Clients.GetClientByID(acc.ClientID));

			accID						= acc.ID;
			accountType					= acc.AccType;
			AccountNumber				= acc.AccountNumber;
			Balance						= acc.Balance;
			Interest					= acc.Interest;
			Opened						= acc.Opened;
			EndDate						= acc.EndDate;
			AccClosed					= acc.Closed;
			Topupable					= acc.Topupable;
			WithdrawalAllowed			= acc.WithdrawalAllowed;
			RecalcPeriod				= acc.RecalcPeriod;
			Compounding					= acc.Compounding;
			InterestAccumulationAccNum	= acc.InterestAccumulationAccNum;

			DataContext = this;
		}

		private void InitializeAccountFieldLabelsAndVisibility()
		{
			AccountWindowNameTags tags = new AccountWindowNameTags(accountType);
			Title							= tags.SystemWindowTitle;
			MainTitle.Text					= tags.WindowHeader;
			WithdrawCashButton.Visibility	= tags.WithdrawCashButtonVisibility;
			WireButton.Visibility			= tags.WireButtonVisibility;
			DepositPart.Visibility			= tags.DepositPartVisibility;

			// Без капитализации указываем счет для накопления процентов
			if(Compounding == false)
			{
				InterestAccumulationAccLabel.Visibility = Visibility.Visible;
				InterestAccumulationAccValue.Visibility = Visibility.Visible;
			}

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
			if (!Topupable)
			{
				MessageBox.Show("Пополнение невозможно!");
				return;
			}
			EnterCashAmountWindow cashWin = new EnterCashAmountWindow();
			var result = cashWin.ShowDialog();
			if (result != true) return;

			IAccount updatedAcc = BA.Accounts.TopUp(accID, cashWin.amount);
			Balance = updatedAcc.Balance;
			accountsNeedUpdate = true;
		}

		private void WithdrawCashButton_Click(object sender, RoutedEventArgs e)
		{
			if (!WithdrawalAllowed)
			{
				MessageBox.Show("Снятие невозможно!");
				return;
			}
			EnterCashAmountWindow cashWin = new EnterCashAmountWindow();
			var result = cashWin.ShowDialog();
			if (result != true) return;

			if(Balance < cashWin.amount)
			{
				MessageBox.Show("Недостаточно средств для снятия!");
				return;
			}
			IAccount updatedAcc = BA.Accounts.Withdraw(accID, cashWin.amount);
			Balance = updatedAcc.Balance;
			accountsNeedUpdate = true;
		}

		private void WireButton_Click(object sender, RoutedEventArgs e)
		{
			var topupableAccountsList = BA.Accounts.GetAllTopupableAccounts();
		}

		private void CloseAccountButton_Click(object sender, RoutedEventArgs e)
		{
			IAccount closedAcc = null;
			if (Balance < 0)
			{
				MessageBox.Show("Невозможно закрыть счет, на котором есть долг");
				return;
			}

			if (Balance > 0)
			{
				MessageBox.Show($"Получите ваши денюшки\n в размере {Balance:N2} руб.");
				BA.Accounts.Withdraw(accID, Balance);
			}

			closedAcc = BA.Accounts.CloseAccount(accID);

			Balance				= closedAcc.Balance;
			AccClosed			= closedAcc.Closed;
			Topupable			= false;
			WithdrawalAllowed	= false;

			accountsNeedUpdate = true;
			clientsNeedUpdate  = true;
		}
	}
}
