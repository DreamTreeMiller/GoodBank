using GoodBank.AccountClasses;
using GoodBank.Interfaces_Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodBank.Transaction_Class
{
	public class Transaction : ITransactionDTO
	{
		/// <summary>
		/// Текущий ID счета
		/// </summary>
		private static uint staticID;

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
		private static uint NextID()
		{
			staticID++;
			return staticID;
		}

		/// <summary>
		/// Конструктор для создания новой транзакции
		/// </summary>
		/// <param name="dt">Дата и время транзакции</param>
		/// <param name="baseAcc">Счёт, над которым совершили транзакцию</param>
		/// <param name="partnerAcc">Счёт, куда или откуда переводят деньги</param>
		/// <param name="opType">Тип операции - вклад или снятие налички, перевод с/на счёт</param>
		/// <param name="amount">Сумма операции. Плюс - вклад, минус - снятие</param>
		/// <param name="interest">Процент в операции. 0 - текщий счет</param>
		/// <param name="comment">Комментарий</param>
		public Transaction (DateTime dt, 
							Account baseAcc, 
							Account partnerAcc, 
							OperationType opType, 
							int amount, 
							int interest, 
							string comment)
		{
			ID				= NextID();
			Date			= dt;
			Account			= baseAcc;
			PartnerAccount	= partnerAcc;
			OperationType	= opType;
			Amount			= amount;
			Interest		= interest;
			Comment			= comment;
		}

		/// <summary>
		/// Уникальный ID транзакции
		/// </summary>
		public uint				ID				{ get; }

		/// <summary>
		/// Дата и время транзакции
		/// </summary>
		public DateTime			Date			{ get; }

		/// <summary>
		/// Счёт, над которым совершили транзакцию
		/// </summary>
		public Account			Account			{ get; }

		/// <summary>
		/// Тип операции - вклад или снятие налички, перевод с/на счёт
		/// </summary>
		public OperationType	OperationType	{ get; }

		/// <summary>
		/// Счёт, куда или откуда переводят деньги. Null - если операция с наличкой
		/// </summary>
		public Account			PartnerAccount	{ get; }

		/// <summary>
		/// Сумма операции. Плюс - вклад, минус - снятие
		/// </summary>
		public int				Amount			{ get; }

		/// <summary>
		/// Процент в операции.  0 для текщего счета
		/// </summary>
		public int				Interest		{ get; }

		/// <summary>
		/// Комментарий
		/// </summary>
		public string			Comment			{ get; }
	}
}
