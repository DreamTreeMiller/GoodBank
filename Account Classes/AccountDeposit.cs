using BankInside;
using Interfaces_Data;
using LoggingNS;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountClasses
{
	[Table("AccountsDeposit")]
	public class AccountDeposit : Account
	{
		/// <summary>
		/// ID счета, куда перечислять проценты.
		/// При капитализации, совпадает с ИД счета депозита
		/// Без капитализации равен 0
		/// </summary>
		public int InterestAccumulationAccID { get; } = 0;

		/// <summary>
		/// Номер счета, куда перечислять проценты.
		/// При капитализации, совпадает с номером счета депозита
		/// Без капитализации имеет значение "внутренний счет"
		/// </summary>
		public string InterestAccumulationAccNum { get; } 

		/// <summary>
		/// Накомпленные проценты 
		/// </summary>
		public double AccumulatedInterest { get; set; } = 0;

		/// <summary>
		/// Конструктор для работы Entity Framework
		/// </summary>
		public AccountDeposit() { }

		/// <summary>
		/// Констркуктор для создания счета. 
		/// Включает в себя поле даты открытия счета
		/// </summary>
		/// <param name="acc"></param>
		/// <param name="opened"></param>
		public AccountDeposit(IAccountDTO acc, DateTime opened, Action<Transaction> writeloghandler)
			: base(acc.ClientID, acc.ClientType, AccountType.Deposit, acc.Compounding, acc.Interest,
				  opened,
				  acc.Topupable, acc.WithdrawalAllowed, acc.RecalcPeriod, acc.Duration,
				  writeloghandler)
		{
			AccountNumber = "DEP" + AccountNumber;
			Balance = acc.Balance;
			if (acc.Compounding)
			{
				InterestAccumulationAccID  = AccountID;
				InterestAccumulationAccNum = AccountNumber;
			}
			else
			{
				InterestAccumulationAccID  = acc.InterestAccumulationAccID;
				InterestAccumulationAccNum = acc.InterestAccumulationAccNum;
			}

			MonthsElapsed = acc.MonthsElapsed;

			Transaction openAccountTransaction = new Transaction(
				AccountID,
				Opened,
				"",
				"",
				OperationType.OpenAccount,
				Balance,
				"Вклад " + AccountNumber
				+ $" с начальной суммой {Balance:N2} руб."
				+ " открыт."
				);
			OnWriteLog(openAccountTransaction);
		}

		/// <summary>
		/// Этот метод вызывается точно один раз в месяц
		/// </summary>
		/// <param name="date"></param>
		public override double RecalculateInterest(DateTime currentBankTime)
		{
			if (Closed != null) return 0;

			if (IsBlocked) return 0;

			NumberOfTopUpsInDay = 0;

			// Пересчёт не нужен. Клиент должен снять средства и закрыть счет
			if (Duration == MonthsElapsed) return 0;
			double calculatedInterest = 0;
			MonthsElapsed++;

			switch (RecalcPeriod)
			{
				// Счет, у которого начисление происходит всего один раз в конце периода
				case RecalcPeriod.AtTheEnd:
					if (Duration == MonthsElapsed)
					{
						AccumulatedInterest = Balance * Interest;

						// Срок истёк. Пополнять больше нельзя. 
						// Владельцу счета надо снять деньги и закрыть счет
						Topupable		  = false;
						WithdrawalAllowed = true;

							UpdateLog(calculatedInterest, currentBankTime);

					}
					break;

				// Счет, у которого начисление происходит раз в год
				case RecalcPeriod.Annually:
					// Срок год или больше, и прошёл ровно год 
					if (MonthsElapsed % 12 == 0)
					{
						calculatedInterest   = Balance * Interest;
						AccumulatedInterest += calculatedInterest;

						UpdateLog(calculatedInterest, currentBankTime);
					}

					if (MonthsElapsed == Duration)
					{
						// Срок истёк. Пополнять больше нельзя. 
						// Владельцу счета надо снять деньги и закрыть счет
						Topupable		  = false;
						WithdrawalAllowed = true;

						// Если прошло меньше года после последнего начисления процента
						// то надо доначислить проценты 
						int MonthsRemained = MonthsElapsed % 12;
						if (MonthsRemained != 0)
						{
							calculatedInterest   = Balance * Interest * MonthsRemained / 12;
							AccumulatedInterest += calculatedInterest;

							UpdateLog(calculatedInterest, currentBankTime);
						}
					}	
					break;
				// Счет, у которого начисление происходит раз в месяц
				case RecalcPeriod.Monthly:
					calculatedInterest   = Balance * Interest / 12;
					AccumulatedInterest += calculatedInterest;

					if (Duration == MonthsElapsed)
					{
						// Срок истёк. Пополнять больше нельзя. 
						// Владельцу счета надо снять деньги и закрыть счет
						Topupable		  = false;
						WithdrawalAllowed = true;
					}

					UpdateLog(calculatedInterest, currentBankTime);
					break;
			}

			if (Compounding) Balance += calculatedInterest;

			return calculatedInterest;
		}

		/// <summary>
		/// Переводит на другой счет накопленный процент.
		/// Это нельзя делать обычным переводом, т.к. деньги не списываются с основного счета
		/// </summary>
		/// <param name="destAcc"></param>
		/// <param name="accumulatedInterest"></param>
		public void SendInterestToAccount(string destAccNum, double accumulatedInterest, DateTime currentBankTime)
		{
			Transaction withdrawCashTransaction = new Transaction(
				AccountID,
				currentBankTime,
				"накопленный процент",
				destAccNum,
				OperationType.SendWireToAccount,
				accumulatedInterest,
				"Перевод накопленных процентов"
				+ " на счет " + destAccNum
				+ $" в размере {accumulatedInterest:N2} руб."
				);
			OnWriteLog(withdrawCashTransaction);
		}

		private void UpdateLog(double calculatedInterest, DateTime currentBankTime)
		{
			string comment;

			if (InterestAccumulationAccID == 0)
			{
				comment = "Начисление процентов на внутренний счет"
							+ $" на сумму {calculatedInterest:N2} руб.";
			}
			else
			{
				comment = $"Начисление процентов на сумму {calculatedInterest:N2} руб.";
			}


			Transaction interestAccrualTransaction = new Transaction(
				AccountID,
				currentBankTime,
				"",
				InterestAccumulationAccNum,
				OperationType.InterestAccrual,
				calculatedInterest,
				comment
				);

			OnWriteLog(interestAccrualTransaction);
		}

		public override double CloseAccount(DateTime currentBankTime)
		{
			// Если без капитализации и накапливали на внутреннем счету,
			// тогда этот процент переводим на основной счет
			if (!Compounding && InterestAccumulationAccID == 0)
			{
				Balance += AccumulatedInterest;
				AccumulatedInterest = 0;
			}

			return base.CloseAccount(currentBankTime);
		}

	}
}
