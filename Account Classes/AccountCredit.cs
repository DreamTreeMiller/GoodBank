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
		/// Констркуктор создания счета. 
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
		public override double RecalculateInterest(DateTime currentBankTime)
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
				currentBankTime,
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

		public override double CloseAccount(DateTime currentBankTime)
		{
			return base.CloseAccount(currentBankTime);
		}
	}
}
