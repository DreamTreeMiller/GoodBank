using GoodBankNS.Interfaces_Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodBankNS.AccountClasses
{
	public class AccountCredit : Account
	{
		public override AccountType AccType { get => AccountType.Credit; }
		public override double		Balance { get; set; }
		public double				AccumulatedInterest { get; set; }

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
		public AccountCredit(IAccountDTO acc)
			: base(acc.ClientID, acc.ClientType, acc.Compounding, acc.Interest,
				   true, false, RecalcPeriod.Monthly, acc.Duration)
		{
			AccountNumber	= "CRE" + AccountNumber;
			Balance			= acc.Balance;
		}

		/// <summary>
		/// Констркуктор для искусственной генерации счета. 
		/// Включает в себя поле даты открытия счета
		/// </summary>
		/// <param name="acc"></param>
		/// <param name="opened"></param>
		public AccountCredit(IAccountDTO acc, DateTime opened)
			: base(acc.ClientID, acc.ClientType, acc.Compounding, acc.Interest,
				   opened,
				   true, false, RecalcPeriod.Monthly, acc.Duration)
		{
			AccountNumber	= "CRE" + AccountNumber;
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

			// Пересчёт не нужен. 
			// Клиент должен пополнить счет до 0 и закрыть
			if (Duration == MonthsElapsed) return 0;
			MonthsElapsed++;

			double calculatedInterest = Balance * Interest / 12;
			AccumulatedInterest		 += calculatedInterest;
			Balance					 += calculatedInterest;

			return calculatedInterest;
		}

		public override double CloseAccount()
		{
			AccumulatedInterest = 0;
			return base.CloseAccount();
		}
	}
}
