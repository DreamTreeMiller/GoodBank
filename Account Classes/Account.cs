using GoodBankNS.ClientClasses;
using GoodBankNS.Interfaces_Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodBankNS.AccountClasses
{
	public abstract class Account : IAccount
	{
		#region Статическая часть для генерации уникального ID

		/// <summary>
		/// Текущий ID счета
		/// </summary>
		private static uint		staticID;

		/// <summary>
		/// Статический конструктор. Обнуляет счетчик ID
		/// </summary>
		static Account()
		{
			staticID = 0;
		}

		/// <summary>
		/// Герерирует следующий ID
		/// </summary>
		/// <returns>New unique ID</returns>
		private static uint		NextID()
		{
			staticID++;
			return staticID;
		}

		#endregion

		#region Общие поля для всех счетов
		
		/// <summary>
		/// Тип клиента, которому принадлежит счет.
		/// Это избыточное поле, но благодаря ему делается всего один проход по базе 
		/// при показе счетов одного типа клиентов
		/// </summary>
		public ClientType		ClientType		{ get; set; }

		/// <summary>
		/// ID владельца счета. 
		/// </summary>
		public uint				ClientID		{ get; set; }

		/// <summary>
		/// Тип счета текущий, вклад или кредит
		/// Дублирование, т.к. тип счета определяется его классом
		/// </summary>
		public abstract AccountType		AccType		{ get; }

		/// <summary>
		/// Уникальный ID счёта - используем для базы
		/// </summary>
		public uint				ID				{ get; }

		/// <summary>
		/// Уникальный номер счёта. 
		/// Числовая часть совпадает с ID. 
		/// Есть префикс, указывающий тип счета
		/// </summary>
		public string			AccountNumber	{ get; set; }

		/// <summary>
		/// Баланс счёта. Для разных типов разный
		/// Текущий - остаток
		/// Вклад	- сумма вклада
		/// Кредит	- сумма долга
		/// </summary>
		public abstract double	Balance			{ get; set; }

		/// <summary>
		/// Процент. 0 для текущего, прирорст для вклада, минус для долга
		/// </summary>
		public double			Interest		{ get; set; }

		/// <summary>
		/// С капитализацией или без
		/// </summary>
		public bool				Compounding		{ get; set; }

		/// <summary>
		/// ID счета, куда перечислять проценты.
		/// При капитализации, совпадает с ИД счета депозита
		/// </summary>
		public uint				CompoundAccID	{ get; set; }

		public string			CompoundAccNum	{ get; set; }

		/// <summary>
		/// Дата открытия счета
		/// </summary>
		public DateTime			Opened			{ get; set; }

		/// <summary>
		/// Количество месяцев, на который открыт вклад, выдан кредит.
		/// 0 - бессрочно
		/// </summary>
		public int				Duration		{ get; set; }

		/// <summary>
		/// Дата окончания вклада/кредита. 
		/// null - бессрочно
		/// </summary>
		public abstract DateTime? EndDate		{ get; }

		/// <summary>
		/// Дата закрытия счета. Только для закрытых
		/// Если счет открыт, то равен null
		/// </summary>
		public DateTime?		Closed			{ get; set; } = null;

		/// <summary>
		/// Пополняемый счет или нет
		/// </summary>
		public bool				Topupable		{ get; set; }

		/// <summary>
		/// С правом частичного снятия или нет
		/// </summary>
		public bool			WithdrawalAllowed	{ get; set; }

		/// <summary>
		/// Период пересчета процентов - ежедневно, ежемесячно, ежегодно, один раз в конце
		/// </summary>
		public RecalcPeriod		RecalcPeriod	{ get; set; }

		#endregion

		#region Конструктор

		/// <summary>
		/// Создание счета
		/// </summary>
		/// <param name="clientID"></param>
		/// <param name="clientType"></param>
		/// <param name="compounding"></param>
		/// <param name="compAccID"></param>
		/// <param name="interest"></param>
		public Account( uint clientID, ClientType clientType, bool compounding, uint compAccID, double interest,
						bool topup, bool withdrawal, RecalcPeriod recalc, int duration)
		{
			ClientID			= clientID;
			ClientType			= clientType;
			ID					= NextID();
			AccountNumber		= $"{ID:000000000000}";
			Compounding			= compounding;
			CompoundAccID		= compAccID;
			Balance				= 0;
			Interest			= interest;
			Opened				= DateTime.Now;
			Topupable			= topup;
			WithdrawalAllowed	= withdrawal;
			RecalcPeriod		= recalc;
			Duration			= duration;
		}

		/// <summary>
		/// Конструктор для генератора случайных счетов. Добавляется дата
		/// </summary>
		/// <param name="clientID"></param>
		/// <param name="clientType"></param>
		/// <param name="compounding"></param>
		/// <param name="compAccID"></param>
		/// <param name="interest"></param>
		public Account(uint clientID, ClientType clientType, bool compounding, uint compAccID, double interest,
						DateTime opened,
						bool topup, bool withdrawal, RecalcPeriod recalc, int duration)
		{
			ClientID			= clientID;
			ClientType			= clientType;
			ID					= NextID();
			AccountNumber		= $"{ID:000000000000}";
			Compounding			= compounding;
			CompoundAccID		= compAccID;
			Balance				= 0;
			Interest			= interest;
			Opened				= opened;
			Topupable			= topup;
			WithdrawalAllowed	= withdrawal;
			RecalcPeriod		= recalc;
			Duration			= duration;
		}

		#endregion

		public void TopUp(double amount)
		{
			if (Topupable) Balance += amount;
		}
	}
}
