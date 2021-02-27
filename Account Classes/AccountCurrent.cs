using BankInside;
using Interfaces_Data;
using LoggingNS;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountClasses
{
	[Table("AccountsCurrent")]
	public class AccountCurrent : Account
	{
	/// <summary>
	/// Конструктор для работы Entity Framework
	/// </summary>
	public AccountCurrent() { }

		/// <summary>
		/// Констркуктор для создания счета. 
		/// Включает в себя поле даты открытия счета
		/// </summary>
		/// <param name="acc"></param>
		/// <param name="opened"></param>
		public AccountCurrent(IAccountDTO acc, DateTime opened, Action<Transaction> writeloghandler)
			: base(acc.ClientID, acc.ClientType, AccountType.Current, acc.Compounding, acc.Interest,
				   opened,
				   true, true, RecalcPeriod.NoRecalc, 0,
				   writeloghandler)
		{
			AccountNumber = "CUR" + AccountNumber;
			Balance		  = acc.Balance;

			Transaction openAccountTransaction = new Transaction(
				AccountID,
				Opened,
				"",
				"",
				OperationType.OpenAccount,
				Balance,
				"Текущий счет " + AccountNumber
				+ " с начальной суммой " + Balance + " руб."
				+ " открыт."
				);
			OnWriteLog(openAccountTransaction);
		}

		public override double RecalculateInterest(DateTime currentBankTime)
		{
			NumberOfTopUpsInDay = 0;
			// Do nothing since no interest recalculation for current account
			return 0;
		}

		public override double CloseAccount(DateTime currentBankTime)
		{
			return base.CloseAccount(currentBankTime);
		}
	}
}
