﻿using BankInside;
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
			: base(acc.ClientID, acc.ClientType, AccountType.Current, acc.Compounding, acc.Interest,
				   true, true, RecalcPeriod.NoRecalc, 0, writeloghandler)
		{
			AccountNumber	= "CUR" + AccountNumber;
			Balance			= acc.Balance;

			Transaction openAccountTransaction = new Transaction(
				AccountID,
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
