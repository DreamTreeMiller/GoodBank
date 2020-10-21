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
		public override double Balance { get; set; }

		public override DateTime? EndDate => 
			Duration == 0 ? null : (DateTime?)Opened.AddMonths(Duration);

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
		/// Opened		  = DateTime.Now;
		/// Topupable	  =							--> false
		/// WithdrawalAllowed	=					--> false
		/// RecalcPeriod  =							--> monthly
		/// EndDate		  =							--> из IAccountDTO acc 
		public AccountCredit(IAccountDTO acc)
			: base(acc.ClientID, acc.ClientType, acc.Compounding, acc.CompoundAccID, acc.Interest,
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
			: base(acc.ClientID, acc.ClientType, acc.Compounding, acc.CompoundAccID, acc.Interest,
				   opened,
				   true, false, RecalcPeriod.Monthly, acc.Duration)
		{
			AccountNumber	= "CRE" + AccountNumber;
			Balance			= acc.Balance;
		}
	}
}
