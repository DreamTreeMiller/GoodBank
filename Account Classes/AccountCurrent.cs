﻿using GoodBankNS.BankInside;
using GoodBankNS.DTO;
using GoodBankNS.Interfaces_Data;
using GoodBankNS.Transaction_Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodBankNS.AccountClasses
{
	public class AccountCurrent : Account
	{
		public override AccountType AccType { get => AccountType.Current; }
		public override double Balance { get; set; }

		/// <summary>
		/// Создание счета на основе введенных данных
		/// </summary>
		/// <param name="acc">Данные для открытия счета</param>
		/// Напоминалка, что инициализируется в базовом классе
		/// ClientID	  = clientID;				--> из IAccountDTO acc
		/// ClientType	  = clientType;				--> из IAccountDTO acc
		/// ID			  = NextID();
		/// AccountNumber = $"{ID:000000000000}";	--> добавляется CUR
		/// Compounding	  = compounding;			--> из IAccountDTO acc
		/// CompoundAccID = compAccID;				--> из IAccountDTO acc
		/// Balance		  = 0;
		/// Interest	  = interest;				--> из IAccountDTO acc
		/// AccountStatus = AccountStatus.Opened;
		/// Opened		  = GoodBank.Today;
		/// Topupable	  =							--> true
		/// WithdrawalAllowed	=					--> ture
		/// RecalcPeriod  =							--> No recalc period
		/// EndDate		  =							--> null 
		public AccountCurrent(IAccountDTO acc, Action<Transaction> writeloghandler)
			: base(acc.ClientID, acc.ClientType, acc.Compounding, acc.Interest,
				   true, true, RecalcPeriod.NoRecalc, 0, writeloghandler)
		{
			AccountNumber	= "CUR" + AccountNumber;
			Balance			= acc.Balance;

			Transaction openAccountTransaction = new Transaction(
				AccID,
				GoodBank.GetBanksTodayWithCurrentTime(),
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

		/// <summary>
		/// Констркуктор для искусственной генерации счета. 
		/// Включает в себя поле даты открытия счета
		/// </summary>
		/// <param name="acc"></param>
		/// <param name="opened"></param>
		public AccountCurrent(IAccountDTO acc, DateTime opened, Action<Transaction> writeloghandler)
			: base(acc.ClientID, acc.ClientType, acc.Compounding, acc.Interest,
				   opened,
				   true, true, RecalcPeriod.NoRecalc, 0,
				   writeloghandler)
		{
			AccountNumber = "CUR" + AccountNumber;
			Balance		  = acc.Balance;

			Transaction openAccountTransaction = new Transaction(
				AccID,
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

		public override double RecalculateInterest()
		{
			NumberOfTopUpsInDay = 0;
			// Do nothing since no interest recalculation for current account
			return 0;
		}

		public override double CloseAccount()
		{
			return base.CloseAccount();
		}
	}
}
