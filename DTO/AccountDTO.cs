using GoodBankNS.AccountClasses;
using GoodBankNS.Interfaces_Actions;
using GoodBankNS.Interfaces_Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodBankNS.DTO
{
	/// <summary>
	/// Структура для показа данных о любом счете
	/// Либо для передачи данных при открытии счета
	/// Не меняется в процессе, поэтому все поля заполняются на этапе создания
	/// через конструктор
	/// </summary>
	public class AccountDTO
	{
		public string			ClientType		{ get; }
		public string			ClientName		{ get; }
		public string			AccType			{ get; }
		public uint				ID				{ get; }
		public string			AccountNumber	{ get; }
		public int				CurrentAmount	{ get; }
		public int				DepositAmount	{ get; }
		public int				DebtAmount		{ get; }
		public int				Interest		{ get; }
		public string			AccStatus		{ get; }
		public DateTime			Opened			{ get; }
		public DateTime?		Closed			{ get; }

		public AccountDTO(IAccount acc, IClient c)
		{
			ID				= acc.ID;    // Account ID
			AccountNumber	= acc.AccountNumber;
			Interest		= acc.Interest;
			AccStatus		= (acc.AccountStatus == AccountStatus.Opened) ? "Open" : "Closed";
			Opened			= acc.Opened;
			Closed			= acc.Closed;

			// Обнуляем, чтобы не ругался компилятор
			ClientType	  = ClientName	  = AccType	   = "";
			CurrentAmount = DepositAmount = DebtAmount = 0;

			if (c is IClientVIP)
			{
				ClientType = "VIP";
				ClientName =
					(c as IClientVIP).LastName +
					(c as IClientVIP).FirstName +
					(String.IsNullOrEmpty((c as IClientVIP).MiddleName) ? "" : " ") +
					(c as IClientVIP).MiddleName;
			}

			if (c is IClientSimple)
			{
				ClientType = "обычный";
				ClientName =
					(c as IClientSimple).LastName +
					(c as IClientSimple).FirstName +
					(String.IsNullOrEmpty((c as IClientSimple).MiddleName) ? "" : " ") +
					(c as IClientSimple).MiddleName;
			}

			if (c is IClientOrg)
			{
				ClientType = "ОРГ";
				ClientName = (c as IClientOrg).OrgName;
			}

			switch (acc.AccountType)
			{
				case AccountType.Current:
					AccType = "текущий";
					CurrentAmount = acc.Balance;
					break;
				case AccountType.Deposit:
					AccType = "вклад";
					DepositAmount = acc.Balance;
					break;
				case AccountType.Credit:
					AccType = "кредит";
					DebtAmount = acc.Balance;
					break;
			}
		}
	}
}
