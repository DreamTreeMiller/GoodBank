using GoodBankNS.AccountClasses;
using GoodBankNS.ClientClasses;
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
	public class AccountDTO : IAccountDTO
	{
		public ClientType	ClientType		{ get; set; }
		public string		ClientTypeTag
		{ 
			get
			{
				string ct = "";
				switch(ClientType)
				{
					case ClientType.VIP:
						ct = "ВИП";
						break;
					case ClientType.Simple:
						ct = "Физик";
						break;
					case ClientType.Organization:
						ct = "Юрик";
						break;
					case ClientType.All:
					default:
						ct = "";
						break;
				}
				return ct;
			}
		}
		public uint			ClientID		{ get; set; }
		public string		ClientName		{ get; set; }
		public AccountType	AccType			{ get; set; }
		public string		AccTypeTag							// only {get; }
		{ 
			get
			{
				string att = "";
				switch (AccType)
				{
					case AccountType.Current:
						att = "текущий";
						break;
					case AccountType.Deposit:
						att = "вклад";
						break;
					case AccountType.Credit:
						att = "кредит";
						break;
					case AccountType.Total:
					default:
						att = "";
						break;
				}
				return att;
			}
		}
		public uint			ID				{ get; }
		public string		AccountNumber	{ get; set; }
		public double		Balance			{ get; set; }

		public string		CurrentAmount	
		{
			get => AccType == AccountType.Current ? $"{Balance:N2}" : "";
		}

		public string		DepositAmount
		{
			get => AccType == AccountType.Deposit ? $"{Balance:N2}" : ""; 
		}

		public string		DebtAmount
		{
			get => AccType == AccountType.Credit ? $"{Balance:N2}" : ""; 
		}

		public double		Interest		{ get; set; }

		/// <summary>
		/// С капитализацией или без
		/// </summary>
		public bool			Compounding	{ get; set; } = true;

		/// <summary>
		/// ID счета, куда перечислять проценты.
		/// При капитализации, совпадает с ИД счета депозита
		/// </summary>
		public uint			CompoundAccID	{ get; set; }
		public string		CompoundAccNum	{ get; set; } = "Fake-0000-0000-0000";

		public DateTime		Opened			{ get; set; }

		/// <summary>
		/// Дата окончания вклада/кредита
		/// null - бессрочный вклад
		/// </summary>
		public DateTime?	EndDate			{ get; set; }

		/// <summary>
		/// Дата фактического закрытия вклада
		/// </summary>
		public DateTime?	Closed			{ get; set; }

		/// <summary>
		/// Пополняемый счет или нет
		/// </summary>
		public bool			Topupable		{ get; set; }

		/// <summary>
		/// С правом частичного снятия или нет
		/// </summary>
		public bool		WithdrawalAllowed	{ get; set; }

		/// <summary>
		/// Период пересчета процентов - ежедневно, ежемесячно, ежегодно, один раз в конце
		/// </summary>
		public RecalcPeriod	RecalcPeriod	{ get; set; }


		/// <summary>
		/// Конструктор для создания счета и записи счета в базу
		/// Данные получены от ручного ввода
		/// 14 полей!!! ужас!!!
		/// </summary>
		public AccountDTO(ClientType ct, uint clientID, AccountType accType,
						  double balance, double interest, 
						  bool compounding, uint compAccID, DateTime opened, 
						  bool topup, bool withdraw, RecalcPeriod recalc, DateTime? endDate)

		{
			ClientType			= ct;
			ClientID			= clientID;
			AccType				= accType;
			Balance				= balance;
			Interest			= interest;
			Compounding			= compounding;
			CompoundAccID		= compAccID;
			Opened				= opened;
			Topupable			= topup;
			WithdrawalAllowed	= withdraw;
			RecalcPeriod		= recalc;
			EndDate				= endDate;
		}

	/// <summary>
	/// Конструктор для формирования ДТО для ПОКАЗА счетов
	/// </summary>
	/// <param name="c">Клиент</param>
	/// <param name="acc">Счет</param>
	public AccountDTO(IClient c, IAccount acc)
		{
			ClientID			= c.ID;
			AccType				= acc.AccType;
			ID					= acc.ID;				// Account ID
			AccountNumber		= acc.AccountNumber;
			Balance				= acc.Balance;
			Interest			= acc.Interest;
			Compounding			= acc.Compounding;
			CompoundAccID		= acc.CompoundAccID;
			Opened				= acc.Opened;
			EndDate				= acc.EndDate;
			Closed				= acc.Closed;
			Topupable			= acc.Topupable;
			WithdrawalAllowed	= acc.WithdrawalAllowed;
			RecalcPeriod		= acc.RecalcPeriod;


			if (c is IClientVIP)
			{
				ClientType = ClientType.VIP;
				ClientName =
					(c as IClientVIP).LastName + " " +
					(c as IClientVIP).FirstName +
					(String.IsNullOrEmpty((c as IClientVIP).MiddleName) ? "" : " ") +
					(c as IClientVIP).MiddleName;
			}

			if (c is IClientSimple)
			{
				ClientType = ClientType.Simple;
				ClientName =
					(c as IClientSimple).LastName + " " +
					(c as IClientSimple).FirstName +
					(String.IsNullOrEmpty((c as IClientSimple).MiddleName) ? "" : " ") +
					(c as IClientSimple).MiddleName;
			}

			if (c is IClientOrg)
			{
				ClientType = ClientType.Organization;
				ClientName = (c as IClientOrg).OrgName;
			}

		}
	}
}
