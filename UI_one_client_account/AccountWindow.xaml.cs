using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using AccountClasses;
using Binding_UI_CondeBehind;
using ClientClasses;
using Interfaces_Data;
using UserControlsLists;

namespace UI_one_client_account
{
	/// <summary>
	/// Interaction logic for AccountWindow.xaml
	/// </summary>
	public partial class AccountWindow : Window, INotifyPropertyChanged
	{
		#region Account Fields in Window

		private int			AccountID;
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

		private double accumulatedInterest;
		public  double AccumulatedInterest
		{ 
			get => accumulatedInterest; 
			set {  accumulatedInterest = value; NotifyPropertyChanged(); }
		}

		private bool IsBlocked;

		#endregion

		BankActions BA;
		IClientDTO  client;

		public bool accountsNeedUpdate = false;
		public bool clientsNeedUpdate  = false;
		TransactionsLogUserControl transLogUC;

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
			ShowAccountTransactionsLog();
		}

		private void InitializeClassScopeVars(BankActions ba, IAccountDTO acc)
		{
			BA = ba;
			BankTodayDate.Text = $"Сегодня {BA.GBDateTime.Today():dd.MM.yyyy} г.";
			client	= BA.Clients.GetClientDTObyID(acc.ClientID);

			AccountID					= acc.AccountID;
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
			AccumulatedInterest			= acc.AccumulatedInterest;
			IsBlocked					= acc.IsBlocked;


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
				InterestAccumulationLine.Visibility = Visibility.Visible;
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
				PersonalInfo.Visibility		= Visibility.Visible;
			}
			ClientInfo.DataContext = client;
		}

		private void ShowAccountTransactionsLog()
		{
			transLogUC = new TransactionsLogUserControl();
			TransactionsGrid.Content = transLogUC;
			UpdateAccountTransactionsLog();
		}

		private void UpdateAccountTransactionsLog()
		{
			var accTransLog = BA.Log.GetAccountTransactionsLog(AccountID);
			transLogUC.TransactionsLog.ItemsSource = accTransLog;
		}
		private void TopUpButton_Click(object sender, RoutedEventArgs e)
		{
			if (AccClosed != null)
			{
				MessageBox.Show($"Счет {AccountNumber} закрыт.");
				return;
			}

			if (IsBlocked)
			{
				MessageBox.Show($"Счет {AccountNumber} заблокирован.");
				return;
			}

			if (!Topupable)
			{
				MessageBox.Show("Пополнение невозможно!");
				return;
			}
			EnterCashAmountWindow cashWin = new EnterCashAmountWindow();
			var result = cashWin.ShowDialog();
			if (result != true) return;

			IAccountDTO updatedAcc = BA.Accounts.TopUpCash(AccountID, cashWin.amount);
			Balance   = updatedAcc.Balance;
			IsBlocked = updatedAcc.IsBlocked;
			accountsNeedUpdate = true;
			UpdateAccountTransactionsLog();

			if (IsBlocked)
			{
				MessageBox.Show($"Счет {AccountNumber} заблокирован.");
			}
		}

		private void WithdrawCashButton_Click(object sender, RoutedEventArgs e)
		{
			if (AccClosed != null)
			{
				MessageBox.Show($"Счет {AccountNumber} закрыт.");
				return;
			}

			if (IsBlocked)
			{
				MessageBox.Show($"Счет {AccountNumber} заблокирован.");
				return;
			}

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
			IAccountDTO updatedAcc = BA.Accounts.WithdrawCash(AccountID, cashWin.amount);
			Balance = updatedAcc.Balance;
			accountsNeedUpdate = true;
			UpdateAccountTransactionsLog();
		}

		private void WireButton_Click(object sender, RoutedEventArgs e)
		{
			if (AccClosed != null)
			{
				MessageBox.Show($"Счет {AccountNumber} закрыт.");
				return;
			}

			if (IsBlocked)
			{
				MessageBox.Show($"Счет {AccountNumber} заблокирован.");
				return;
			}

			if (!WithdrawalAllowed)
			{
				MessageBox.Show("C данного счета нельзя снимать средства");
				return;
			}

			var topupableAccountsList = BA.Accounts.GetTopupableAccountsToWireTo(AccountID);
			EnterAmountAndAccountWindow eaawin = new EnterAmountAndAccountWindow(topupableAccountsList);
			var result = eaawin.ShowDialog();
			if (result != true) return;

			double wireAmount = eaawin.amount;
			if (wireAmount > Balance)
			{
				MessageBox.Show("Недостаточно средств для перевода");
				return;
			}
			int destAccID = eaawin.destinationAccount.AccountID;
			BA.Accounts.Wire(AccountID, destAccID, wireAmount);

			Balance -= wireAmount;
			MessageBox.Show($"Сумма {wireAmount:N2} руб. успешно переведена");
			accountsNeedUpdate = true;
			UpdateAccountTransactionsLog();
		}

		private void CloseAccountButton_Click(object sender, RoutedEventArgs e)
		{
			if (AccClosed != null)
			{
				MessageBox.Show($"Счет {AccountNumber} уже закрыт.");
				return;
			}

			if (IsBlocked)
			{
				MessageBox.Show($"Счет {AccountNumber} заблокирован.");
				return;
			}

			if (Balance < 0)
			{
				MessageBox.Show("Невозможно закрыть счет, на котором есть долг");
				return;
			}

			double accumulatedAmount;
			IAccountDTO closedAcc = BA.Accounts.CloseAccount(AccountID, out accumulatedAmount);

			if (accumulatedAmount > 0)
			{
				MessageBox.Show($"Получите ваши денюшки\n в размере {accumulatedAmount:N2} руб.");
			}

			// Обновляем суммы, даты, флажки в окошке
			Balance				= closedAcc.Balance;
			AccumulatedInterest = closedAcc.AccumulatedInterest;

			AccClosed			= closedAcc.Closed;
			Topupable			= closedAcc.Topupable;
			WithdrawalAllowed	= closedAcc.WithdrawalAllowed;

			MessageBox.Show($"Счет {AccountNumber} закрыт.\n"
				+ "Счет остается в системе, но его дальнейшее использование невозможно."
				);

			accountsNeedUpdate = true;
			clientsNeedUpdate  = true;
			UpdateAccountTransactionsLog();
		}
	}
}
