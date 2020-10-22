using GoodBankNS.AccountClasses;
using GoodBankNS.ClientClasses;
using GoodBankNS.Interfaces_Actions;
using GoodBankNS.Interfaces_Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
		public uint			ClientID		{ get; set; }
		public string		ClientName		{ get; set; }
		public AccountType	AccType			{ get; set; }
		public uint			ID				{ get; } = 0;
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

		#region поля только для депозитов
		/// <summary>
		/// ID счета, куда перечислять проценты.
		/// При капитализации, совпадает с ИД счета депозита
		/// 0 - если внутренний счет
		/// </summary>
		public uint			InterestAccumulationAccID	{ get; set; }


		public string		InterestAccumulationAccNum	{ get; set; }
		public double		AccumulatedInterest			{ get; set; } = 0;

		#endregion

		public DateTime		Opened			{ get; set; }

		/// <summary>
		/// Количество месяцев, на который открыт вклад, выдан кредит.
		/// 0 - бессрочно
		/// </summary>
		public int Duration { get; set; }

		/// <summary>
		/// Дата окончания вклада/кредита. 
		/// null - бессрочно
		/// </summary>
		public DateTime? EndDate =>
			Duration == 0 ? null : (DateTime?)Opened.AddMonths(Duration);

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
						  bool compounding, uint interestAccumAccID, string interestAccumAccNum, DateTime opened, 
						  bool topup, bool withdraw, RecalcPeriod recalc, int duration)

		{
			ClientType			= ct;
			ClientID			= clientID;
			AccType				= accType;
			Balance				= balance;
			Interest			= interest;
			Compounding			= compounding;
			InterestAccumulationAccID  = interestAccumAccID;
			InterestAccumulationAccNum = interestAccumAccNum;
			Opened				= opened;
			Topupable			= topup;
			WithdrawalAllowed	= withdraw;
			RecalcPeriod		= recalc;
			Duration			= duration;
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
			Opened				= acc.Opened;
			Duration			= acc.Duration;
			Closed				= acc.Closed;
			Topupable			= acc.Topupable;
			WithdrawalAllowed	= acc.WithdrawalAllowed;
			RecalcPeriod		= acc.RecalcPeriod;

			if (acc is IAccountDeposit)
			{
				InterestAccumulationAccID  = (acc as IAccountDeposit).InterestAccumulationAccID;
				InterestAccumulationAccNum = (acc as IAccountDeposit).InterestAccumulationAccNum;
			}

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
