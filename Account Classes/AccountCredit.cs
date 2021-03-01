using System;
using System.ComponentModel.DataAnnotations.Schema;
using Interfaces_Data;

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
		public AccountCredit(IAccountDTO acc, DateTime opened)
			: base(acc.ClientID, acc.ClientType, 
				   AccountType.Credit, acc.Compounding, acc.Interest,
				   opened,
				   true, false, RecalcPeriod.Monthly, acc.Duration)
		{
			Balance			= acc.Balance;
			MonthsElapsed	= acc.MonthsElapsed;
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
			return calculatedInterest;
		}

		public override double CloseAccount(DateTime closingDate)
		{
			return base.CloseAccount(closingDate);
		}
	}
}
