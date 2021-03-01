using System;
using LoggingNS;

namespace Interfaces_Data
{
	public interface ITransactionDTO
	{
		/// <summary>
		/// Уникальный ID транзакции
		/// </summary>
		int				TransactionID			{ get; }

		/// <summary>
		/// Счет, над которым совершено действие
		/// </summary>
		int				TransactionAccountID	{ get; }

		/// <summary>
		/// Дата и время транзакции
		/// </summary>
		DateTime		TransactionDateTime		{ get; }

		/// <summary>
		/// Счёт, над которым совершили транзакцию
		/// </summary>
		string			SourceAccount			{ get; }

		/// <summary>
		/// Тип операции - вклад или снятие налички, перевод с/на счёт
		/// </summary>
		TransactionType	TransactionType			{ get; }

		/// <summary>
		/// Счёт, куда или откуда переводят деньги. Null - если операция с наличкой
		/// </summary>
		string			DestinationAccount		{ get; }

		/// <summary>
		/// Сумма операции. Плюс - вклад, минус - снятие
		/// </summary>
		double			Amount					{ get; }

		/// <summary>
		/// Комментарий
		/// </summary>
		string			Comment					{ get; }
	}
}
