using BankInside;
using Interfaces_Data;
using LoggingNS;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountClasses
{
	[Table("AccountsCredit")]
	public class AccountCredit : Account
	{
		public double AccumulatedInterest { get; set; }

		/// <summary>
		/// Конструктор для работы Entity Framework
		/// </summary>
		public AccountCredit() { }

		/// <summary>
		/// Создание счета на основе введенных данных
		/// </summary>
		/// <param name="acc">Данные для открытия счета</param>
		/// Напоминалка, что инициализируется в базовом классе
		/// ClientID	  = clientID;				--> из IAccountDTO acc
		/// ClientType	  = clientType;				--> из IAccountDTO acc
		/// ID			  = NextID();
		/// AccountNumber = $"{ID:000000000000}";	--> добавляется CRE
		/// Compounding	  = compounding;			--> из IAccountDTO acc
		/// CompoundAccID = compAccID;				--> из IAccountDTO acc
		/// Balance		  = 0;
		/// Interest	  = interest;				--> из IAccountDTO acc
		/// AccountStatus = AccountStatus.Opened;
		/// Opened		  = GoodBank.Today;
		/// Topupable	  =							--> false
		/// WithdrawalAllowed	=					--> false
		/// RecalcPeriod  =							--> monthly
		/// EndDate		  =							--> из IAccountDTO acc 
		public AccountCredit(IAccountDTO acc, Action<Transaction> writeloghandler)
			: base(acc.ClientID, acc.ClientType, AccountType.Credit, acc.Compounding, acc.Interest,
				   true, false, RecalcPeriod.Monthly, acc.Duration, writeloghandler)
		{
			AccountNumber	= "CRE" + AccountNumber;
			Balance			= acc.Balance;

			Transaction openAccountTransaction = new Transaction(
				AccountID,
				GoodBank.GetBanksTodayWithCurrentTime(),
				"",
				"",
				OperationType.OpenAccount,
				Balance,
				"Кредитный счет " + AccountNumber
				+ " на сумму " + Balance + " руб."
				+ " открыт."
				);
			OnWriteLog(openAccountTransaction);
		}

		/// <summary>
		/// Констркуктор для искусственной генерации счета. 
		/// Включает в себя поле даты открытия счета
		/// </summary>
		/// <param name="acc"></param>
		/// <param name="opened"></param>
		public AccountCredit(IAccountDTO acc, DateTime opened, Action<Transaction> writeloghandler)
			: base(acc.ClientID, acc.ClientType, 
				   AccountType.Credit, acc.Compounding, acc.Interest,
				   opened,
				   true, false, RecalcPeriod.Monthly, acc.Duration, writeloghandler)
		{
			AccountNumber	= "CRE" + AccountNumber;
			Balance			= acc.Balance;
			MonthsElapsed	= acc.MonthsElapsed;

			Transaction openAccountTransaction = new Transaction(
				AccountID,
				Opened,
				"",
				"",
				OperationType.OpenAccount,
				Balance,
				"Кредитный счет " + AccountNumber
				+ " на сумму " + Balance + " руб."
				+ " открыт."
				);
			OnWriteLog(openAccountTransaction);
		}

		/// <summary>
		/// Пересчет процентов для кредитов. Происходит только раз в месяц,
		/// т.е. не раз в год, и не один раз в конце
		/// </summary>
		/// <param name="date"></param>
		public override double RecalculateInterest()
		{
			if (Closed != null) return 0;

			NumberOfTopUpsInDay = 0;

			// Пересчёт не нужен. 
			// Клиент должен пополнить счет до 0 и закрыть
			if (Duration == MonthsElapsed) return 0;
			MonthsElapsed++;

			double calculatedInterest = Balance * Interest / 12;
			AccumulatedInterest		 += calculatedInterest;
			Balance					 += calculatedInterest;

			Transaction interestAccrualTransaction = new Transaction(
				AccountID,
				GoodBank.GetBanksTodayWithCurrentTime(),
				"",
				AccountNumber,
				OperationType.InterestAccrual,
				calculatedInterest,
				"Начисление процентов на счет " + AccountNumber
				+ $" на сумму {calculatedInterest:N2} руб."
				);
			OnWriteLog(interestAccrualTransaction);
			return calculatedInterest;
		}

		public override double CloseAccount()
		{
			return base.CloseAccount();
		}
	}
}
