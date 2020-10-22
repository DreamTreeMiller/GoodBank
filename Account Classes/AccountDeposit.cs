using GoodBankNS.Interfaces_Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodBankNS.AccountClasses
{
	public class AccountDeposit : Account, IAccountDeposit
	{
		public override AccountType AccType { get => AccountType.Deposit; }

		public override double Balance { get; set; }

		/// <summary>
		/// ID счета, куда перечислять проценты.
		/// При капитализации, совпадает с ИД счета депозита
		/// Без капитализации равен 0
		/// </summary>
		public uint InterestAccumulationAccID { get; } = 0;

		/// <summary>
		/// Номер счета, куда перечислять проценты.
		/// При капитализации, совпадает с номером счета депозита
		/// Без капитализации имеет значение "внутренний счет"
		/// </summary>
		public string InterestAccumulationAccNum { get; } 

		/// <summary>
		/// Накомпленные проценты 
		/// </summary>
		public double AccumulatedInterest { get; set; } = 0;

		public override DateTime? EndDate => 
			Duration == 0 ? null : (DateTime?)Opened.AddMonths(Duration);

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
			: base(acc.ClientID, acc.ClientType, acc.Compounding, acc.Interest,
				  acc.Topupable, acc.WithdrawalAllowed, acc.RecalcPeriod, acc.Duration)
		{
			AccountNumber	= "DEP" + AccountNumber;
			Balance			= acc.Balance;
			InterestAccumulationAccID  = acc.InterestAccumulationAccID;
			InterestAccumulationAccNum = acc.InterestAccumulationAccNum;
		}

		/// <summary>
		/// Констркуктор для искусственной генерации счета. 
		/// Включает в себя поле даты открытия счета
		/// </summary>
		/// <param name="acc"></param>
		/// <param name="opened"></param>
		public AccountDeposit(IAccountDTO acc, DateTime opened)
			: base(acc.ClientID, acc.ClientType, acc.Compounding, acc.Interest,
				  opened,
				  acc.Topupable, acc.WithdrawalAllowed, acc.RecalcPeriod, acc.Duration)
		{
			AccountNumber = "DEP" + AccountNumber;
			Balance = acc.Balance;
			InterestAccumulationAccID  = acc.InterestAccumulationAccID;
			InterestAccumulationAccNum = acc.InterestAccumulationAccNum;
		}
	}
}
