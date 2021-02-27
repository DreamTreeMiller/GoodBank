using System;
using System.Linq;
using System.Collections.ObjectModel;
using Interfaces_Actions;
using Interfaces_Data;
using DTO;
using AccountClasses;
using ClientClasses;
using LoggingNS;

namespace BankInside
{
	public class AccountActions : IAccountActions
	{
		private readonly IRepository		 dbe;
		private readonly Action<Transaction> WriteLog;

		public AccountActions(IRepository dbrep)
		{ 
			dbe		 = dbrep;
			WriteLog = dbe.WriteLog;
		}

		public Account GetAccountByID(int id)
		{
			return dbe.GetAccountByID(id);
		}

		/// <summary>
		/// Добавляет счёт
		/// </summary>
		/// <param name="acc"></param>
		/// <returns>Возвращает созданный счет с уникальным ID счета</returns>
		public IAccountDTO AddAccount(IAccountDTO acc)
		{
			Account newAcc = null;
			Client  client = dbe.GetClientByID(acc.ClientID);
			switch (acc.AccType)
			{
				case AccountType.Current:
					newAcc = new AccountCurrent(acc, acc.Opened, WriteLog);
					client.NumberOfCurrentAccounts++;
					break;
				case AccountType.Deposit:
					newAcc = new AccountDeposit(acc, acc.Opened, WriteLog);
					client.NumberOfDeposits++;
					break;
				case AccountType.Credit:
					newAcc = new AccountCredit(acc, acc.Opened, WriteLog);
					client.NumberOfCredits++;
					break;
			}
			// делает ДВА действия db.Accounts.Add + db.SaveChanges
			// это необходимо, чтобы база вернула уникальный ID (IDENTITY(1,1),
			// который потом должен быть записан в AccountID
			newAcc = dbe.AddAccount(newAcc);

			// Необходим этот костыль, чтобы сгенерировать номер счёта,
			// основываясь на AccountID, которую можно получить,
			// только после добавления записи о счёте в базу.
			// Пока не знаю, как за одно обращение к базе генерировать AccountID 
			// и на его основе генерировать номре счёта
			switch (acc.AccType)
			{
				case AccountType.Current:
					newAcc.AccountNumber = $"SAV{newAcc.AccountID:000000000000}";
					break;
				case AccountType.Deposit:
					newAcc.AccountNumber = $"DEP{newAcc.AccountID:000000000000}";
					break;
				case AccountType.Credit:
					newAcc.AccountNumber = $"CRE{newAcc.AccountID:000000000000}";
					break;
			}
			dbe.SaveChanges();
			return new AccountDTO(client, newAcc);
		}

		public IAccountDTO TopUpCash(int accID, double cashAmount)
		{
			Account acc = dbe.GetAccountByID(accID);
			DateTime gbCurrentDateTime = dbe.GetBankCurrentDateAndTime();
			acc.WriteLog += WriteLog;

			if (acc.Topupable) acc.TopUpCash(cashAmount, gbCurrentDateTime);
			dbe.SaveChanges();
			return new AccountDTO(acc);
		}

		public IAccountDTO WithdrawCash(int accID, double cashAmount)
		{
			Account acc = dbe.GetAccountByID(accID);
			DateTime gbCurrentDateTime = dbe.GetBankCurrentDateAndTime();
			acc.WriteLog += WriteLog;

			if (acc.WithdrawalAllowed) acc.WithdrawCash(cashAmount, gbCurrentDateTime);
			dbe.SaveChanges();
			return new AccountDTO(acc);
		}

		/// <summary>
		/// Перевод средств со счета на счет
		/// </summary>
		/// <param name="sourceAccID"></param>
		/// <param name="destAccID"></param>
		/// <param name="wireAmount"></param>
		public void Wire(int sourceAccID, int destAccID, double wireAmount)
		{
			Account sourceAcc = dbe.GetAccountByID(sourceAccID);
			DateTime gbCurrentDateTime = dbe.GetBankCurrentDateAndTime();
			sourceAcc.WriteLog += WriteLog;

			if (sourceAcc.WithdrawalAllowed)
			{
				if (sourceAcc.Balance >= wireAmount)
				{
					Account destAcc = dbe.GetAccountByID(destAccID);
					destAcc.WriteLog += WriteLog;

					if (destAcc.Topupable)
					{
						sourceAcc.SendToAccount(destAcc.AccountNumber, wireAmount, gbCurrentDateTime);
						destAcc.ReceiveFromAccount(sourceAcc.AccountNumber, wireAmount, gbCurrentDateTime);
						dbe.SaveChanges();
					}
				}
			}
		}

		/// <summary>
		/// Закрыть можно только нулевой счет
		/// Проверку на наличие денег на счете осуществляет вызывающий метод
		/// </summary>
		/// <param name="accID"></param>
		/// <returns></returns>
		public IAccountDTO CloseAccount(int accID, out double accumulatedAmount)
		{
			Account acc		  = dbe.GetAccountByID(accID);
			DateTime gbCurrentDateTime = dbe.GetBankCurrentDateAndTime();
			acc.WriteLog	 += WriteLog;
			accumulatedAmount = acc.CloseAccount(gbCurrentDateTime);

			Client client = dbe.GetClientByID(acc.ClientID);
			client.NumberOfClosedAccounts++;

			switch(acc.AccType)
			{
				case AccountType.Current:
					client.NumberOfCurrentAccounts--;
					break;
				case AccountType.Deposit:
					client.NumberOfDeposits--;
					break;
				case AccountType.Credit:
					client.NumberOfCredits--;
					break;
			}
			dbe.SaveChanges();
			return new AccountDTO(acc);
		}

		public void AddOneMonth()
		{
			dbe.AddOneMonthToBankDate();
			DateTime gbCurrentDateTime = dbe.GetBankCurrentDateAndTime();

			foreach (Account acc in dbe.GetAccounts())
			{
				acc.WriteLog += WriteLog;
				double currInterest = acc.RecalculateInterest(gbCurrentDateTime);
				if (acc is AccountDeposit)
				{
					int destAccID = (acc as AccountDeposit).InterestAccumulationAccID;
					if (!(acc as AccountDeposit).Compounding && 
						destAccID    != 0 && 
						currInterest != 0)
					{
						Account destAcc = GetAccountByID(destAccID);
						destAcc.WriteLog += WriteLog;
						WireInterestToAccount(acc as AccountDeposit, destAcc, currInterest, gbCurrentDateTime);
					}
				}
			}
			dbe.SaveChanges();
		}

		private void WireInterestToAccount(	AccountDeposit sourceAcc,	Account destAcc, 
											double accumulatedInterest, DateTime currBankDate)
		{
			sourceAcc.SendInterestToAccount(destAcc.AccountNumber, accumulatedInterest, currBankDate);
			destAcc.ReceiveFromAccount(sourceAcc.AccountNumber, accumulatedInterest, currBankDate);
		}

		#region UI part

		/// <summary>
		/// Формирует список счетов данного типа клиентов.
		/// </summary>
		/// <param name="clientType">Тип клиента</param>
		/// <returns>
		/// коллекцию счетов и общую сумму каждой группы счетов - текущие, вклады, кредиты
		/// </returns>
		public (ObservableCollection<IAccountDTO> accList, double totalCurr, double totalDeposit, double totalCredit)
			GetAccountsList(ClientType clientType)
		{
			ObservableCollection<IAccountDTO> accList = new ObservableCollection<IAccountDTO>();
			double totalCurr = 0, totalDeposit = 0, totalCredit = 0;

			if (clientType == ClientType.All)
			{
				foreach (Account acc in dbe.GetAccounts())
				{
					switch (acc.AccType)
					{
						case AccountType.Current:
							totalCurr += acc.Balance;
							break;
						case AccountType.Deposit:
							totalDeposit += acc.Balance;
							// Добавляем средства, накопленные на внутренних счетах депозитов,
							// которые будут переведены на счет с номером,
							// и соответственно доступны для снятия,
							// только после окончания срока вклада
							if (!acc.Compounding && (acc as AccountDeposit).InterestAccumulationAccID == 0)
								totalDeposit += (acc as AccountDeposit).AccumulatedInterest;
							break;
						case AccountType.Credit:
							totalCredit += acc.Balance;
							break;
					}
					accList.Add(new AccountDTO(dbe.GetClientByID(acc.ClientID), acc));
				}
				return (accList, totalCurr, totalDeposit, totalCredit);
			}

			IQueryable<Account> clientTypeAccounts = from acc in dbe.GetAccounts()
													 where acc.ClientType == clientType
													 select acc;

			foreach (Account acc in clientTypeAccounts)
			{
				switch (acc.AccType)
				{
					case AccountType.Current:
						totalCurr += acc.Balance;
						break;
					case AccountType.Deposit:
						totalDeposit += acc.Balance;
						// Добавляем средства, накопленные на внутренних счетах депозитов,
						// которые будут переведены на счет с номером,
						// и соответственно доступны для снятия,
						// только после окончания срока вклада
						if (!acc.Compounding && (acc as AccountDeposit).InterestAccumulationAccID == 0)
							totalDeposit += (acc as AccountDeposit).AccumulatedInterest;
						break;
					case AccountType.Credit:
						totalCredit += acc.Balance;
						break;
				}
				accList.Add(new AccountDTO(dbe.GetClientByID(acc.ClientID), acc));
			}
			return (accList, totalCurr, totalDeposit, totalCredit);
		}

		/// <summary>
		/// Формирует список счетов заданного клиента.
		/// </summary>
		/// <param name="clientID">ID клиента</param>
		/// <returns>
		/// возвращает коллекцию счетов и общую сумму каждой группы счетов - текущие, вклады, кредиты
		/// </returns>
		public (ObservableCollection<IAccountDTO> accList, double totalCurr, double totalDeposit, double totalCredit)
			GetClientAccounts(int clientID)
		{
			ObservableCollection<IAccountDTO> accList = new ObservableCollection<IAccountDTO>();
			Client client = dbe.GetClientByID(clientID);
			double totalCurr = 0, totalDeposit = 0, totalCredit = 0;

			var clientAccounts = from acc in dbe.GetAccounts()
								 where acc.ClientID == clientID
								 select acc;

			foreach (Account acc in clientAccounts)
			{
				switch (acc.AccType)
				{
					case AccountType.Current:
						totalCurr += acc.Balance;
						break;
					case AccountType.Deposit:
						totalDeposit += acc.Balance;
						break;
					case AccountType.Credit:
						totalCredit += acc.Balance;
						break;
				}
				accList.Add(new AccountDTO(client, acc));
			}
			return (accList, totalCurr, totalDeposit, totalCredit);
		}

		/// <summary>
		/// Фомирует спискок счетов заданного типа у данного клиента
		/// </summary>
		/// <param name="clientID"></param>
		/// <param name="accType"></param>
		/// <returns></returns>
		public ObservableCollection<IAccountDTO> GetClientAccounts(int clientID, AccountType accType)
		{
			ObservableCollection<IAccountDTO> accList = new ObservableCollection<IAccountDTO>();
			Client client = dbe.GetClientByID(clientID);

			var clientAccounts = from acc in dbe.GetAccounts()
								 where acc.ClientID == clientID && acc.AccType == accType
								 select acc;

			foreach (Account acc in clientAccounts) accList.Add(new AccountDTO(client, acc));

			//foreach (Account acc in db.Accounts)
			//{
			//	if (acc.ClientID == clientID && acc.AccType == accType)
			//		accList.Add(new AccountDTO(client, acc));
			//}

			return accList;

		}

		/// <summary>
		/// Формирует коллекцию номеров счетов, на которые можно переводить деньги 
		/// </summary>
		/// <param name="sourceAccID"></param>
		/// <returns></returns>
		public ObservableCollection<IAccountDTO> GetTopupableAccountsToWireTo(int sourceAccID)
		{
			ObservableCollection<IAccountDTO> accList = new ObservableCollection<IAccountDTO>();

			var topupableAccounts = from acc in dbe.GetAccounts()
									where acc.Topupable && acc.AccountID != sourceAccID
									select acc;

			foreach (Account tAcc in topupableAccounts) accList.Add(new AccountDTO(tAcc));

			//foreach (Account acc in db.Accounts)
			//	if (acc.Topupable && acc.AccountID != sourceAccID)
			//	{
			//		accList.Add(new AccountDTO(acc));
			//	}
			return accList;
		}

		#endregion
	}
}
