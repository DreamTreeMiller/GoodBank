using System;
using System.ComponentModel.DataAnnotations.Schema;
using Interfaces_Data;

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
		public AccountCurrent(IAccountDTO acc, DateTime opened)
			: base(acc.ClientID, acc.ClientType, AccountType.Current, acc.Compounding, acc.Interest,
				   opened,
				   true, true, RecalcPeriod.NoRecalc, 0)
		{
			Balance	= acc.Balance;
		}

		public override double RecalculateInterest()
		{
			NumberOfTopUpsInDay = 0;
			// Do nothing since no interest recalculation for current account
			return 0;
		}

		public override double CloseAccount(DateTime closingDate)
		{
			return base.CloseAccount(closingDate);
		}
	}
}
