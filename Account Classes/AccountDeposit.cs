using GoodBankNS.Interfaces_Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodBankNS.AccountClasses
{
	public class AccountDeposit : Account, IAccountDeposit
	{
		public override AccountType AccType { get => AccountType.Deposit; }

		public override double Balance { get; set; }

		/// <summary>
		/// ID счета, куда перечислять проценты.
		/// При капитализации, совпадает с ИД счета депозита
		/// Без капитализации равен 0
		/// </summary>
		public uint InterestAccumulationAccID { get; } = 0;

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
		/// Создание счета на основе введенных данных
		/// </summary>
		/// <param name="acc">Данные для открытия счета</param>
		/// Напоминалка, что инициализируется в базовом классе
		/// ClientID	  = clientID;				--> потом из IAccountDTO acc
		/// ClientType	  = clientType;				--> потом из IAccountDTO acc
		/// ID			  = NextID();
		/// AccountNumber = $"{ID:000000000000}";	--> потом добавляется CUR
		/// Compounding	  = compounding;			--> потом из IAccountDTO acc
		/// CompoundAccID = compAccID;				--> потом из IAccountDTO acc
		/// Balance		  = 0;
		/// Interest	  = interest;				--> потом из IAccountDTO acc
		/// AccountStatus = AccountStatus.Opened;
		/// Opened		  = GoodBank.Today;
		/// Topupable	  =							--> из IAccountDTO acc
		/// WithdrawalAllowed	=					--> из IAccountDTO acc
		/// RecalcPeriod  =							--> из IAccountDTO acc
		/// EndDate		  =							--> из IAccountDTO acc 
		public AccountDeposit(IAccountDTO acc)
			: base(acc.ClientID, acc.ClientType, acc.Compounding, acc.Interest,
				  acc.Topupable, acc.WithdrawalAllowed, acc.RecalcPeriod, acc.Duration)
		{
			AccountNumber	= "DEP" + AccountNumber;
			Balance			= acc.Balance;
			InterestAccumulationAccID  = acc.InterestAccumulationAccID;
			InterestAccumulationAccNum = acc.InterestAccumulationAccNum;
		}

		/// <summary>
		/// Констркуктор для искусственной генерации счета. 
		/// Включает в себя поле даты открытия счета
		/// </summary>
		/// <param name="acc"></param>
		/// <param name="opened"></param>
		public AccountDeposit(IAccountDTO acc, DateTime opened)
			: base(acc.ClientID, acc.ClientType, acc.Compounding, acc.Interest,
				  opened,
				  acc.Topupable, acc.WithdrawalAllowed, acc.RecalcPeriod, acc.Duration)
		{
			AccountNumber = "DEP" + AccountNumber;
			Balance = acc.Balance;
			InterestAccumulationAccID  = acc.InterestAccumulationAccID;
			InterestAccumulationAccNum = acc.InterestAccumulationAccNum;
			MonthsElapsed = acc.MonthsElapsed;
		}

		/// <summary>
		/// Этот метод вызывается точно один раз в месяц
		/// </summary>
		/// <param name="date"></param>
		public override double RecalculateInterest()
		{
			if (Closed != null) return 0;

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
					}
					break;
				// Счет, у которого начисление происходит раз в год
				case RecalcPeriod.Annually:
					// Срок год или больше, и прошёл ровно год 
					if (MonthsElapsed % 12 == 0)
					{
						calculatedInterest   = Balance * Interest;
						AccumulatedInterest += calculatedInterest;
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
						}
					}	
					break;
				// Счет, у которого начисление происходит раз в год
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
					break;
			}

			if (Compounding) Balance += calculatedInterest;

			return calculatedInterest;
		}

		public override double CloseAccount()
		{
			double totalBalance = base.CloseAccount();

			// Если без капитализации и накапливали на внутреннем счету,
			// тогда этот процент надо тоже суммировать
			if (!Compounding && InterestAccumulationAccID == 0)
				totalBalance += AccumulatedInterest;
			AccumulatedInterest = 0;

			return totalBalance;
		}

	}
}
