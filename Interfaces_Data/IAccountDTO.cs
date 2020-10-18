using GoodBankNS.AccountClasses;
using GoodBankNS.ClientClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodBankNS.Interfaces_Data
{
	public interface IAccountDTO 
	{
		ClientType	ClientType		{ get; set; }
		string		ClientName		{ get; set; }
		uint		ClientID		{ get; set; }
		AccountType	AccType			{ get; set; }
		uint		ID				{ get; }
		string		AccountNumber	{ get; set; }
		double		CurrentAmount	{ get; set; }
		double		DepositAmount	{ get; set; }
		double		DebtAmount		{ get; set; }
		double		Interest		{ get; set; }

		/// <summary>
		/// С капитализацией или без
		/// </summary>
		bool		Compounding		{ get; set; }

		/// <summary>
		/// ID счета, куда перечислять проценты.
		/// При капитализации, совпадает с ИД счета депозита
		/// </summary>
		uint		CompoundAccID	{ get; set; }

		DateTime	Opened				{ get; set; }
		DateTime?	Closed			{ get; set; }

		/// <summary>
		/// Пополняемый счет или нет
		/// </summary>
		bool		Topupable		{ get; set; }

		/// <summary>
		/// С правом частичного снятия или нет
		/// </summary>
		bool	WithdrawalAllowed	{ get; set; }

		/// <summary>
		/// Период пересчета процентов - ежедневно, ежемесячно, ежегодно, один раз в конце
		/// </summary>
		RecalcPeriod RecalcPeriod	{ get; set; }

		/// <summary>
		/// Дата окончания вклада/кредита
		/// null - бессрочный вклад
		/// </summary>
		DateTime?	EndDate			{ get; set; }
	}
}
