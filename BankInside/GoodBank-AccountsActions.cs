using AccountClasses;
using ClientClasses;
using DTO;
using Interfaces_Actions;
using Interfaces_Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Navigation;

namespace BankInside
{
	public partial class GoodBank : IAccountsActions
	{
		private List<Account> accounts;

		public Account GetAccountByID(int id)
		{
			return accounts.Find(a => a.AccountID == id);
		}

		/// <summary>
		/// Добавляет счет, данные которого получены от ручного ввода
		/// Эти данные не содержат ID и номера счета
		/// </summary>
		/// <param name="acc"></param>
		/// <returns>Возвращает созданный счет с уникальным ID счета</returns>
		public IAccountDTO AddAccount(IAccountDTO acc)
		{
			Account newAcc = null;
			var client = GetClientByID(acc.ClientID);
			switch(acc.AccType)
			{
				case AccountType.Current:
					newAcc = new AccountCurrent(acc, WriteLog);
					client.NumberOfCurrentAccounts++;
					break;
				case AccountType.Deposit:
					newAcc = new AccountDeposit(acc, WriteLog);
					client.NumberOfDeposits++;
					break;
				case AccountType.Credit:
					newAcc = new AccountCredit(acc, WriteLog);
					client.NumberOfCredits++;
					break;
			}
			accounts.Add(newAcc);
			db.Accounts.Add(newAcc);
			db.SaveChanges();
			return new AccountDTO(client, newAcc);
		}

		/// <summary>
		/// Добавляет счет, данные которого получены от ручного ввода
		/// Эти данные не содержат ID и номера счета
		/// </summary>
		/// <param name="acc"></param>
		/// <returns>Возвращает созданный счет с уникальным ID счета</returns>
		public IAccountDTO GenerateAccount(IAccountDTO acc)
		{
			Account newAcc = null;
			var client = GetClientByID(acc.ClientID);
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
			accounts.Add(newAcc);
			db.Accounts.Add(newAcc);
			db.SaveChanges();
			return new AccountDTO(client, newAcc);
		}

		/// <summary>
		/// Формирует список счетов данного типа клиентов.
		/// </summary>
		/// <param name="clientType">Тип клиента</param>
		/// <returns>
		/// возвращает коллекцию счетов и общую сумму каждой группы счетов - текущие, вклады, кредиты
		/// </returns>
		public (ObservableCollection<IAccountDTO> accList, double totalCurr, double totalDeposit, double totalCredit)
			GetAccountsList(ClientType clientType)
		{
			ObservableCollection<IAccountDTO> accList = new ObservableCollection<IAccountDTO>();
			double totalCurr = 0, totalDeposit = 0, totalCredit = 0;
			Account acc;
			if (clientType == ClientType.All)
			{
				for (int i = 0; i < accounts.Count; i++)
				{
					acc = accounts[i];
					switch(acc.AccType)
					{
						case AccountType.Current:
							totalCurr += acc.Balance;
							break;
						case AccountType.Deposit:
							totalDeposit += acc.Balance;
							if (!acc.Compounding && (acc as AccountDeposit).InterestAccumulationAccID == 0)
								totalDeposit += (acc as AccountDeposit).AccumulatedInterest;
							break;
						case AccountType.Credit:
							totalCredit += acc.Balance;
							break;
					}
					accList.Add(new AccountDTO(GetClientByID(accounts[i].ClientID), acc));
				}
				return (accList, totalCurr, totalDeposit, totalCredit);
			}

			for (int i = 0; i < accounts.Count; i++)
				if (accounts[i].ClientType == clientType)
				{
					acc = accounts[i];
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
					accList.Add(new AccountDTO(GetClientByID(accounts[i].ClientID), acc));
				}
			return (accList, totalCurr, totalDeposit, totalCredit);
		}

		/// <summary>
		/// Формирует список счетов заданного клиентов.
		/// </summary>
		/// <param name="clientID">ID клиента</param>
		/// <returns>
		/// возвращает коллекцию счетов и общую сумму каждой группы счетов - текущие, вклады, кредиты
		/// </returns>
		public (ObservableCollection<IAccountDTO> accList, double totalCurr, double totalDeposit, double totalCredit)
			GetClientAccounts(int clientID)
		{
			ObservableCollection<IAccountDTO> accList = new ObservableCollection<IAccountDTO>();
			var client = GetClientByID(clientID);
			double totalCurr = 0, totalDeposit = 0, totalCredit = 0;
			Account acc;

			for (int i = 0; i < accounts.Count; i++)
				if (accounts[i].ClientID == clientID)
				{
					acc = accounts[i];
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

		public ObservableCollection<IAccountDTO> GetClientAccounts(int clientID, AccountType accType)
		{
			ObservableCollection<IAccountDTO> accList = new ObservableCollection<IAccountDTO>();
			var client = GetClientByID(clientID);

			for (int i = 0; i < accounts.Count; i++)
			{
				var acc = accounts[i];
				if (acc.ClientID == clientID && acc.AccType == accType)
					accList.Add(new AccountDTO(client, acc));
			}
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
			for (int i = 0; i < accounts.Count; i++)
				if (accounts[i].Topupable && accounts[i].AccountID != sourceAccID)
				{
					accList.Add(new AccountDTO(accounts[i]));
				}
			return accList;
		}

		public IAccountDTO TopUpCash(int accID, double cashAmount)
		{
			var acc = GetAccountByID(accID);
			if (acc.Topupable) acc.TopUpCash(cashAmount);
			return new AccountDTO(acc);
		}

		public IAccountDTO WithdrawCash(int accID, double cashAmount)
		{
			var acc = GetAccountByID(accID);
			if(acc.WithdrawalAllowed) acc.WithdrawCash(cashAmount);
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
			Account sourceAcc = GetAccountByID(sourceAccID);
			if (sourceAcc.WithdrawalAllowed)
			{
				if (sourceAcc.Balance >= wireAmount)
				{
					Account destAcc = GetAccountByID(destAccID);
					if (destAcc.Topupable)
					{
						sourceAcc.SendToAccount(destAcc.AccountNumber, wireAmount);
						destAcc.ReceiveFromAccount(sourceAcc.AccountNumber, wireAmount);
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
			Account acc			= GetAccountByID(accID);
			accumulatedAmount	= acc.CloseAccount();

			Client client		= GetClientByID(acc.ClientID);
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
			return new AccountDTO(acc);
		}

		public void AddOneMonth()
		{
			GoodBank.Today = GoodBank.Today.AddMonths(1);

			for (int i = 0; i< accounts.Count; i++)
			{
				Account acc = accounts[i];
				double currInterest = acc.RecalculateInterest();
				if (acc is AccountDeposit)
				{
					int destAccID = (acc as AccountDeposit).InterestAccumulationAccID;
					if (!(acc as AccountDeposit).Compounding && 
						destAccID    != 0 && 
						currInterest != 0)
					{
						Account destAcc = GetAccountByID(destAccID);
						WireInterestToAccount(acc as AccountDeposit, destAcc, currInterest);
					}
				}
			}
		}

		private void WireInterestToAccount(AccountDeposit sourceAcc, Account destAcc, double accumulatedInterest)
		{
			sourceAcc.SendInterestToAccount(destAcc.AccountNumber, accumulatedInterest);
			destAcc.ReceiveFromAccount(sourceAcc.AccountNumber, accumulatedInterest);
		}


	}
}
