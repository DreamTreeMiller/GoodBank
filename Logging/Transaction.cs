using AccountClasses;
using Interfaces_Data;
using System;

namespace LoggingNS
{
	public class Transaction : ITransactionDTO
	{
		#region Статический генератор уникального ID

		/// <summary>
		/// Текущий ID счета
		/// </summary>
		private static int staticID;

		/// <summary>
		/// Статический конструктор. Обнуляет счетчик ID
		/// </summary>
		static Transaction()
		{
			staticID = 0;
		}

		/// <summary>
		/// Герерирует следующий ID
		/// </summary>
		/// <returns>New unique ID</returns>
		private static int NextID()
		{
			staticID++;
			return staticID;
		}

		#endregion

		/// <summary>
		/// Уникальный ID транзакции
		/// </summary>
		public int				TransactionID		{ get; set; }

		/// <summary>
		/// Счет, над которым совершено действие
		/// </summary>
		public int				TransactionAccountID { get; set; }

		/// <summary>
		/// Дата и время транзакции
		/// </summary>
		public DateTime			TransactionDateTime	{ get; set; }

		/// <summary>
		/// Счёт, над которым совершили транзакцию
		/// </summary>
		public string			SourceAccount		{ get; set; }

		/// <summary>
		/// Счёт, куда или откуда переводят деньги. Null - если операция с наличкой
		/// </summary>
		public string			DestinationAccount	{ get; set; }

		/// <summary>
		/// Тип операции - вклад или снятие налички, перевод с/на счёт
		/// </summary>
		public OperationType	OperationType		{ get; set; }

		/// <summary>
		/// Сумма операции. Плюс - вклад, минус - снятие
		/// </summary>
		public double			Amount				{ get; set; }

		/// <summary>
		/// Комментарий
		/// </summary>
		public string			Comment				{ get; set; }

		/// <summary>
		/// Конструктор для корректной работы Entity Framework
		/// </summary>
		public Transaction() { }

		/// <summary>
		/// Конструктор для создания новой транзакции
		/// </summary>
		/// <param name="dt">Дата и время транзакции</param>
		/// <param name="sourceAcc">Счёт, над которым совершили транзакцию</param>
		/// <param name="destinationAcc">Счёт, куда или откуда переводят деньги</param>
		/// <param name="opType">Тип операции - вклад или снятие налички, перевод с/на счёт</param>
		/// <param name="amount">Сумма операции. Плюс - вклад, минус - снятие</param>
		/// <param name="interest">Процент в операции. 0 - текщий счет</param>
		/// <param name="comment">Комментарий</param>
		public Transaction(int senderAccID,
							DateTime dt,
							string sourceAcc,
							string destinationAcc,
							OperationType opType,
							double amount,
							string comment)
		{
			TransactionID		 = NextID();
			TransactionAccountID = senderAccID;
			TransactionDateTime	 = dt;
			SourceAccount		 = sourceAcc;
			DestinationAccount	 = destinationAcc;
			OperationType		 = opType;
			Amount				 = amount;
			Comment				 = comment;
		}

	}
}
