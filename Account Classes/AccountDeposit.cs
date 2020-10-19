using GoodBankNS.Interfaces_Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodBankNS.AccountClasses
{
	public class AccountDeposit : Account
	{
		public override AccountType AccType { get => AccountType.Deposit; }
		public override double Balance { get; set; }

		/// <summary>
		/// Создание счета на основе введенных данных
		/// </summary>
		/// <param name="acc">Данные для открытия счета</param>
		/// Напоминалка, что инициализируется в базовом классе
		/// ClientID	  = clientID;				--> потом из IAccountDTO acc
		/// ClientType	  = clientType;				--> потом из IAccountDTO acc
		/// ID			  = NextID();
		/// AccountNumber = $"{ID:000000000000}";	--> потом добавляется CUR
		/// Compounding	  = compounding;			--> потом из IAccountDTO acc
		/// CompoundAccID = compAccID;				--> потом из IAccountDTO acc
		/// Balance		  = 0;
		/// Interest	  = interest;				--> потом из IAccountDTO acc
		/// AccountStatus = AccountStatus.Opened;
		/// Opened		  = DateTime.Now;
		/// Topupable	  =							--> из IAccountDTO acc
		/// WithdrawalAllowed	=					--> из IAccountDTO acc
		/// RecalcPeriod  =							--> из IAccountDTO acc
		/// EndDate		  =							--> из IAccountDTO acc 
		public AccountDeposit(IAccountDTO acc)
			: base(acc.ClientID, acc.ClientType, acc.Compounding, acc.CompoundAccID, acc.Interest,
				  acc.Topupable, acc.WithdrawalAllowed, acc.RecalcPeriod, acc.EndDate)
		{
			AccountNumber	= "DEP" + AccountNumber;
			Balance			= acc.Balance;
		}

		/// <summary>
		/// Констркуктор для искусственной генерации счета. 
		/// Включает в себя поле даты открытия счета
		/// </summary>
		/// <param name="acc"></param>
		/// <param name="opened"></param>
		public AccountDeposit(IAccountDTO acc, DateTime opened)
			: base(acc.ClientID, acc.ClientType, acc.Compounding, acc.CompoundAccID, acc.Interest,
				  opened,
				  acc.Topupable, acc.WithdrawalAllowed, acc.RecalcPeriod, acc.EndDate)
		{
			AccountNumber = "DEP" + AccountNumber;
			Balance = acc.Balance;
		}
	}
}
