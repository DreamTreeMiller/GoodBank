using GoodBank.AccountClasses;
using GoodBank.ClientClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodBank.Interfaces_Data
{
	public interface IAccount
	{
		/// <summary>
		/// Тип клиента, которому принадлежит счет.
		/// Это избыточное поле, но благодаря ему делается всего один проход по базе 
		/// при показе счетов одного типа клиентов
		/// </summary>
		ClientType ClientType { get; set; }

		/// <summary>
		/// ID владельца счета. 
		/// Это избыточное поле, но так быстрее найти данные владельца 
		/// при показе счетов одного типа клиентов
		/// </summary>
		uint ClientID { get; set; }

		/// <summary>
		/// Тип счета текущий, вклад или кредит
		/// </summary>
		AccountType AccountType { get; }

		/// <summary>
		/// Уникальный ID счёта - используем для базы
		/// </summary>
		uint ID { get; }

		/// <summary>
		/// Уникальный номер счёта. 
		/// Числовая часть совпадает с ID. 
		/// Есть префикс, указывающий тип счета
		/// </summary>
		string AccountNumber { get; set; }

		/// <summary>
		/// Баланс счёта. Для разных типов разный
		/// Текущий - остаток
		/// Вклад	- сумма вклада
		/// Кредит	- сумма долга
		/// </summary>
		int Balance { get; set; }

		/// <summary>
		/// Процент. 0 для текущего, прирорст для вклада, минус для долга
		/// </summary>
		int Interest { get; set; }

		/// <summary>
		/// Открытый или закрытый счет
		/// </summary>
		AccountStatus AccountStatus { get; set; }

		/// <summary>
		/// Дата открытия счета
		/// </summary>
		DateTime Opened { get; set; }

		/// <summary>
		/// Дата закрытия счета. Только для закрытых
		/// </summary>
		DateTime? Closed { get; set; }
	}
}
