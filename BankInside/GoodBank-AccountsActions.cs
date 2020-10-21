using GoodBankNS.AccountClasses;
using GoodBankNS.ClientClasses;
using GoodBankNS.DTO;
using GoodBankNS.Interfaces_Actions;
using GoodBankNS.Interfaces_Data;
using System;
using System.Collections.ObjectModel;
using System.Windows.Navigation;

namespace GoodBankNS.BankInside
{
	public partial class GoodBank : IAccountsActions
	{
		public IAccount GetAccountByID(uint id)
		{
			return accounts.Find(a => a.ID == id);
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
					newAcc = new AccountCurrent(acc);
					client.NumberOfCurrentAccounts++;
					break;
				case AccountType.Deposit:
					newAcc = new AccountDeposit(acc);
					client.NumberOfDeposits++;
					break;
				case AccountType.Credit:
					newAcc = new AccountCredit(acc);
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
					newAcc = new AccountCurrent(acc, acc.Opened);
					client.NumberOfCurrentAccounts++;
					break;
				case AccountType.Deposit:
					newAcc = new AccountDeposit(acc, acc.Opened);
					client.NumberOfDeposits++;
					break;
				case AccountType.Credit:
					newAcc = new AccountCredit(acc, acc.Opened);
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

		public IAccount TopUp(uint accID, double amount)
		{
			var acc = GetAccountByID(accID);
			acc.TopUp(amount);
			return acc;
		}
	}
}
