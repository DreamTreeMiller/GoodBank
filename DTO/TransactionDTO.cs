using GoodBank.AccountClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodBank.DTO
{
	public struct TransactionDTO
	{
		/// <summary>
		/// Уникальный ID транзакции
		/// </summary>
		uint ID { get; }

		/// <summary>
		/// Дата и время транзакции
		/// </summary>
		DateTime Date { get; }

		/// <summary>
		/// Счёт, над которым совершили транзакцию
		/// </summary>
		Account Account { get; }

		/// <summary>
		/// Тип операции - вклад или снятие налички, перевод с/на счёт
		/// </summary>
		OperationType OperationType { get; }

		/// <summary>
		/// Счёт, куда или откуда переводят деньги. Null - если операция с наличкой
		/// </summary>
		Account PartnerAccount { get; }

		/// <summary>
		/// Сумма операции. Плюс - вклад, минус - снятие
		/// </summary>
		int Amount { get; }

		/// <summary>
		/// Процент в операции.  0 для текщего счета
		/// </summary>
		int Interest { get; }

		/// <summary>
		/// Комментарий
		/// </summary>
		string Comment { get; }
	}
}
