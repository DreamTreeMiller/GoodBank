using Interfaces_Data;
using System;

namespace LoggingNS
{
	public class Transaction : ITransactionDTO
	{
		/// <summary>
		/// Уникальный ID транзакции
		/// </summary>
		public int				TransactionID			{ get; set; }

		/// <summary>
		/// Счет, над которым совершено действие
		/// </summary>
		public int				TransactionAccountID	{ get; set; }

		/// <summary>
		/// Дата и время транзакции
		/// </summary>
		public DateTime			TransactionDateTime		{ get; set; }

		/// <summary>
		/// Тип операции - вклад или снятие налички, перевод с/на счёт
		/// </summary>
		public TransactionType	TransactionType			{ get; set; }

		/// <summary>
		/// Счёт, над которым совершили транзакцию
		/// </summary>
		public string			SourceAccount			{ get; set; }

		/// <summary>
		/// Счёт, куда или откуда переводят деньги. Null - если операция с наличкой
		/// </summary>
		public string			DestinationAccount		{ get; set; }

		/// <summary>
		/// Сумма операции. Плюс - вклад, минус - снятие
		/// </summary>
		public double			Amount					{ get; set; }

		/// <summary>
		/// Комментарий
		/// </summary>
		public string			Comment					{ get; set; }

		/// <summary>
		/// Конструктор для корректной работы Entity Framework
		/// </summary>
		public Transaction() { }
	}
}
