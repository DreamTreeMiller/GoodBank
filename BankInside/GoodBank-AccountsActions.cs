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
		/// <summary>
		/// Добавляет счет, данные которого получены от ручного ввода
		/// Эти данные не содержат ID и номера счета
		/// </summary>
		/// <param name="acc"></param>
		/// <returns>Возвращает созданный счет с уникальным ID счета</returns>
		public IAccountDTO AddAccount(IAccountDTO acc)
		{
			Account newAcc = null;
			switch(acc.AccType)
			{
				case AccountType.Current:
					newAcc = new AccountCurrent(acc);
					break;
				case AccountType.Deposit:
					newAcc = new AccountDeposit(acc);
					break;
				case AccountType.Credit:
					newAcc = new AccountCredit(acc);
					break;
			}
			accounts.Add(newAcc);
			var client = GetClientByID(acc.ClientID);
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
			switch (acc.AccType)
			{
				case AccountType.Current:
					newAcc = new AccountCurrent(acc, acc.Opened);
					break;
				case AccountType.Deposit:
					newAcc = new AccountDeposit(acc, acc.Opened);
					break;
				case AccountType.Credit:
					newAcc = new AccountCredit(acc, acc.Opened);
					break;
			}
			accounts.Add(newAcc);
			var client = GetClientByID(acc.ClientID);
			return new AccountDTO(client, newAcc);
		}

		public (ObservableCollection<AccountDTO> accList, double totalCurr, double totalDeposit, double totalCredit)
			GetAccountsList(ClientType clientType)
		{
			ObservableCollection<AccountDTO> accList = new ObservableCollection<AccountDTO>();
			double totalCurr = 0, totalDeposit = 0, totalCredit = 0;
			IAccount acc;
			AccountDTO totalRecord;
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

		public (ObservableCollection<AccountDTO> accList, double totalCurr, double totalDeposit, double totalCredit)
			GetClientAccounts(uint clientID)
		{
			ObservableCollection<AccountDTO> accList = new ObservableCollection<AccountDTO>();
			var client = GetClientByID(clientID);
			double totalCurr = 0, totalDeposit = 0, totalCredit = 0;
			IAccount acc;
			AccountDTO totalRecord;

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

	}
}
