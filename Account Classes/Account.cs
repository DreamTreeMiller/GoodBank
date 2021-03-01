using System;
using ClientClasses;

namespace AccountClasses
{
	public abstract class Account
	{
		#region Общие поля для всех счетов
		
		/// <summary>
		/// Тип клиента, которому принадлежит счет.
		/// Это избыточное поле, но благодаря ему делается всего один проход по базе 
		/// при показе счетов одного типа клиентов
		/// </summary>
		public ClientType		ClientType			{ get; set; }

		/// <summary>
		/// ID владельца счета. 
		/// </summary>
		public int				ClientID			{ get; set; }

		/// <summary>
		/// Тип счета текущий, вклад или кредит
		/// Дублирование, т.к. тип счета определяется его классом
		/// </summary>
		public AccountType		AccType				{ get; set; }

		/// <summary>
		/// Уникальный ID счёта - используем для базы
		/// </summary>
		public int				AccountID			{ get; set; }

		/// <summary>
		/// Уникальный номер счёта. 
		/// Числовая часть совпадает с ID. 
		/// Есть префикс, указывающий тип счета
		/// </summary>
		public string			AccountNumber		{ get; set; }

		/// <summary>
		/// Баланс счёта. Для разных типов разный
		/// Текущий - остаток
		/// Вклад	- сумма вклада
		/// Кредит	- сумма долга
		/// </summary>
		public double			Balance				{ get; set; }

		/// <summary>
		/// Процент. 0 для текущего, прирорст для вклада, минус для долга
		/// </summary>
		public double			Interest			{ get; set; }

		/// <summary>
		/// С капитализацией или без
		/// </summary>
		public bool				Compounding			{ get; set; }

		/// <summary>
		/// Дата открытия счета
		/// </summary>
		public DateTime			Opened				{ get; set; }

		/// <summary>
		/// Количество месяцев, на который открыт вклад, выдан кредит.
		/// 0 - бессрочно
		/// </summary>
		public int				Duration			{ get; set; }

		/// <summary>
		/// Количество месяцев, прошедших с открытия вклада
		/// </summary>
		public int				MonthsElapsed		{ get; set; }

		/// <summary>
		/// Дата окончания вклада/кредита. 
		/// null - бессрочно
		/// </summary>
		public DateTime?		EndDate				{ get; set; }

		/// <summary>
		/// Дата закрытия счета. Только для закрытых
		/// Если счет открыт, то равен null
		/// </summary>
		public DateTime?		Closed				{ get; set; }

		/// <summary>
		/// Пополняемый счет или нет
		/// У закрытого счета - false
		/// </summary>
		public bool				Topupable			{ get; set; }

		/// <summary>
		/// С правом частичного снятия или нет
		/// У закрытого счета - false
		/// </summary>
		public bool				WithdrawalAllowed	{ get; set; }

		/// <summary>
		/// Период пересчета процентов - ежемесячно, ежегодно, один раз в конце
		/// </summary>
		public RecalcPeriod		RecalcPeriod		{ get; set; }

		#endregion

		#region Свойства противодействия отмыванию денег

		protected int NumberOfTopUpsInDay			{ get; set; } = 0;

		public bool				IsBlocked			{ get; set; } = false;

		#endregion

		#region Конструктор

		/// <summary>
		/// Конструктор для создания счетов. Добавляется дата
		/// </summary>
		public Account(int clientID, ClientType clientType, 
						AccountType accType, bool compounding, double interest,
						DateTime opened,
						bool topup, bool withdrawal, RecalcPeriod recalc, int duration)
		{
			ClientID			= clientID;
			ClientType			= clientType;
			AccType				= accType;
			AccountNumber		= $"{AccountID:000000000000}";
			Compounding			= compounding;
			Balance				= 0;
			Interest			= interest;
			Opened				= opened;
			Topupable			= topup;
			WithdrawalAllowed	= withdrawal;
			RecalcPeriod		= recalc;
			Duration			= duration;
			EndDate = Duration == 0 ? null : (DateTime?)Opened.AddMonths(Duration);
		}

		/// <summary>
		/// Конструктор для работы Entity Framework
		/// </summary>
		public Account() { }

		#endregion

		#region Общие методы для всех типов счетов

		/// <summary>
		/// Пополнение счета наличкой
		/// </summary>
		/// <param name="cashAmount"></param>
		/// <returns>
		/// 0  - deposit successful
		/// -1 - account was blocked
		/// </returns>
		public int Deposit(double cashAmount)
		{
			if (IsBlocked) return -1;

			// Предотвращение отмывания денег
			// Если за день счёт пытаются пополнить более 3-х раз на сумму от 1000 руб,
			// то счёт блокируется
			if (cashAmount >= 1000)
			{
				if(++NumberOfTopUpsInDay > 3)
				{
					Topupable			= false;
					WithdrawalAllowed	= false;
					IsBlocked			= true;
					
					return -1;
				}
			}
			Balance += cashAmount;
			return 0;
		}

		/// <summary>
		/// Нужно для начисления процентов на вклад на случай, если уже исчерпан лимит пополнений через Deposit
		/// </summary>
		/// <param name="interest"></param>
		public void DepositNonCash(double interest)
		{
			Balance += interest;
		}

		/// <summary>
		/// Снятие налички со счета
		/// </summary>
		/// <param name="cashAmount"></param>
		public double Withdraw(double cashAmount)
		{
			Balance -= cashAmount;
			return cashAmount;
		}

		#endregion

		#region Абстрактные и виртуальные методы

		/// <summary>
		/// Делает пересчет процентов на указанную дату
		/// Вызывается извне при изменении даты
		/// </summary>
		/// <returns>
		/// Сумму начисленных процентов, если её надо перевести на другой счет
		/// </returns>
		public abstract double RecalculateInterest();

		/// <summary>
		/// Закрывает счет: обнуляет баланс и накопленный процент
		/// ставит запрет на пополнение и снятие.
		/// Устанавливает дату закрытия счета
		/// </summary>
		/// <returns>
		/// Накопленную сумму
		/// </returns>
		public virtual double CloseAccount(DateTime closingDate)
		{
			double tmp			= Withdraw(Balance);
			Topupable			= false;
			WithdrawalAllowed	= false;
			Closed				= closingDate;
			return tmp;
		}

		#endregion
	}
}
