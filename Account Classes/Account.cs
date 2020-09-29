using GoodBank.Interfaces_Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodBank.AccountClasses
{
	public abstract class Account
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
		/// Тип счета текущий, вклад или кредит
		/// </summary>
		public AccountType		AccountType		{ get; set; }

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
		public abstract int		Balance			{ get; set; }

		/// <summary>
		/// Процент. 0 для текущего, прирорст для вклада, минус для долга
		/// </summary>
		public int				Interest		{ get; set; }

		/// <summary>
		/// Открытый или закрытый счет
		/// </summary>
		public AccountStatus	AccountStatus	{ get; set; }

		/// <summary>
		/// Дата открытия счета
		/// </summary>
		public DateTime			Opened			{ get; set; }

		/// <summary>
		/// Дата закрытия счета. Только для закрытых
		/// </summary>
		public DateTime			Closed			{ get; set; }

		#endregion

		#region Конструктор

		public Account()
		{
			AccountType   = AccountType.Current;
			ID			  = NextID();
			AccountNumber = $"{ID:000000000000}";
			Balance		  = 0;
			Interest	  = 0;
			AccountStatus = AccountStatus.Opened;
			Opened		  = DateTime.Now;
		}

		#endregion
	}
}
