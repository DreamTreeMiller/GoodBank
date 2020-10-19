using GoodBankNS.AccountClasses;
using GoodBankNS.ClientClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodBankNS.Interfaces_Data
{
	public interface IAccount
	{
		/// <summary>
		/// Тип клиента, которому принадлежит счет.
		/// Это избыточное поле, но благодаря ему делается всего один проход по базе 
		/// при показе счетов одного типа клиентов
		/// </summary>
		ClientType		ClientType		{ get; set; }

		/// <summary>
		/// ID владельца счета. 
		/// Это избыточное поле, но так быстрее найти данные владельца 
		/// при показе счетов одного типа клиентов
		/// </summary>
		uint			ClientID		{ get; set; }

		/// <summary>
		/// Тип счета текущий, вклад или кредит
		/// </summary>
		AccountType		AccType			{ get; }

		/// <summary>
		/// Уникальный ID счёта - используем для базы
		/// </summary>
		uint			ID				{ get; }

		/// <summary>
		/// Уникальный номер счёта. 
		/// Числовая часть совпадает с ID. 
		/// Есть префикс, указывающий тип счета
		/// </summary>
		string			AccountNumber	{ get; set; }

		/// <summary>
		/// Баланс счёта. Для разных типов разный
		/// Текущий - остаток
		/// Вклад	- сумма вклада
		/// Кредит	- сумма долга
		/// </summary>
		double			 Balance			{ get; set; }

		/// <summary>
		/// Процент. 0 для текущего, прирорст для вклада, минус для долга
		/// </summary>
		double			 Interest		{ get; set; }

		/// <summary>
		/// С капитализацией или без
		/// </summary>
		bool			Compounding		{ get; set; }

		/// <summary>
		/// ID счета, куда перечислять проценты.
		/// При капитализации, совпадает с ИД счета депозита
		/// </summary>
		uint			CompoundAccID	{ get; set; }

		string			CompoundAccNum	{ get; set; }

		/// <summary>
		/// Дата открытия счета
		/// </summary>
		DateTime		Opened			{ get; set; }

		DateTime? EndDate { get; set; }

		/// <summary>
		/// Дата закрытия счета. Только для закрытых
		/// </summary>
		DateTime?		Closed			{ get; set; }

		/// <summary>
		/// Пополняемый счет или нет
		/// </summary>
		bool			Topupable		{ get; set; }

		/// <summary>
		/// С правом частичного снятия или нет
		/// </summary>
		bool	WithdrawalAllowed		{ get; set; }

		/// <summary>
		/// Период пересчета процентов - ежедневно, ежемесячно, ежегодно, один раз в конце
		/// </summary>
		RecalcPeriod	RecalcPeriod	{ get; set; }
	}
}
