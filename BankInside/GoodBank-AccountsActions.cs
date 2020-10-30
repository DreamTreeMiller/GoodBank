using GoodBankNS.AccountClasses;
using GoodBankNS.ClientClasses;
using GoodBankNS.DTO;
using GoodBankNS.Interfaces_Actions;
using GoodBankNS.Interfaces_Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Navigation;

namespace GoodBankNS.BankInside
{
	public partial class GoodBank : IAccountsActions
	{
		private List<Account> accounts;

		public IAccount GetAccountByID(uint id)
		{
			return accounts.Find(a => a.AccID == id);
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
			return new AccountDTO(client, newAcc);
		}

		/// <summary>
		/// Формирует список счетов данного типа клиентов.
		/// </summary>
		/// <param name="clientType">Тип клиента</param>
		/// <returns>
		/// возвращает коллекцию счетов и общую сумму каждой группы счетов - текущие, вклады, кредиты
		/// </returns>
		public (ObservableCollection<AccountDTO> accList, double totalCurr, double totalDeposit, double totalCredit)
			GetAccountsList(ClientType clientType)
		{
			ObservableCollection<AccountDTO> accList = new ObservableCollection<AccountDTO>();
			double totalCurr = 0, totalDeposit = 0, totalCredit = 0;
			IAccount acc;
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
							if (!acc.Compounding && (acc as IAccountDeposit).InterestAccumulationAccID == 0)
								totalDeposit += (acc as IAccountDeposit).AccumulatedInterest;
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
		public (ObservableCollection<AccountDTO> accList, double totalCurr, double totalDeposit, double totalCredit)
			GetClientAccounts(uint clientID)
		{
			ObservableCollection<AccountDTO> accList = new ObservableCollection<AccountDTO>();
			var client = GetClientByID(clientID);
			double totalCurr = 0, totalDeposit = 0, totalCredit = 0;
			IAccount acc;

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

		public ObservableCollection<AccountDTO> GetClientAccounts(uint clientID, AccountType accType)
		{
			ObservableCollection<AccountDTO> accList = new ObservableCollection<AccountDTO>();
			var client = GetClientByID(clientID);

			for (int i = 0; i < accounts.Count; i++)
			{
				var acc = accounts[i];
				if (acc.ClientID == clientID && acc.AccType == accType)
					accList.Add(new AccountDTO(client, acc));
			}
			return accList;

		}

		public ObservableCollection<IAccount> GetTopupableAccountsToWireFrom(uint sourceAccID)
		{
			ObservableCollection<IAccount> accList = new ObservableCollection<IAccount>();
			for (int i = 0; i < accounts.Count; i++)
				if (accounts[i].Topupable && accounts[i].AccID != sourceAccID)
				{
					accList.Add(accounts[i]);
				}
			return accList;
		}

		public IAccount TopUpCash(uint accID, double cashAmount)
		{
			var acc = GetAccountByID(accID);
			if (acc.Topupable) acc.TopUpCash(cashAmount);
			return acc;
		}

		public IAccount WithdrawCash(uint accID, double cashAmount)
		{
			var acc = GetAccountByID(accID);
			if(acc.WithdrawalAllowed) acc.WithdrawCash(cashAmount);
			return acc;
		}

		/// <summary>
		/// Перевод средств со счета на счет
		/// </summary>
		/// <param name="sourceAccID"></param>
		/// <param name="destAccID"></param>
		/// <param name="wireAmount"></param>
		public void Wire(uint sourceAccID, uint destAccID, double wireAmount)
		{
			var sourceAcc = GetAccountByID(sourceAccID);
			if (sourceAcc.WithdrawalAllowed)
			{
				if (sourceAcc.Balance >= wireAmount)
				{
					var destAcc = GetAccountByID(destAccID);
					if (destAcc.Topupable)
					{
						sourceAcc.SendToAccount(destAcc, wireAmount);
						destAcc.ReceiveFromAccount(sourceAcc, wireAmount);
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
		public IAccount CloseAccount(uint accID, out double accumulatedAmount)
		{
			IAccount acc		= GetAccountByID(accID);
			accumulatedAmount	= acc.CloseAccount();

			IClient client = GetClientByID(acc.ClientID);
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
			return acc;
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
					uint destAccID = (acc as AccountDeposit).InterestAccumulationAccID;
					if (!(acc as AccountDeposit).Compounding && 
						destAccID    != 0 && 
						currInterest != 0)
					{
						IAccount destAcc = GetAccountByID(destAccID);
						WireInterestToAccount(acc as IAccountDeposit, destAcc, currInterest);
					}
				}
			}
		}

		private void WireInterestToAccount(IAccountDeposit sourceAcc, IAccount destAcc, double accumulatedInterest)
		{
			sourceAcc.SendInterestToAccount(destAcc, accumulatedInterest);
			destAcc.ReceiveFromAccount(sourceAcc, accumulatedInterest);
		}


	}
}
