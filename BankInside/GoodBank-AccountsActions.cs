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

		public ObservableCollection<AccountDTO> GetAccountsList(ClientType clientType)
		{
			ObservableCollection<AccountDTO> accList = new ObservableCollection<AccountDTO>();
			if(clientType == ClientType.All)
			{
				for (int i = 0; i < accounts.Count; i++)
					accList.Add(new AccountDTO(GetClientByID(accounts[i].ClientID), accounts[i]));
				return accList;
			}

			for (int i = 0; i < accounts.Count; i++)
				if (accounts[i].ClientType == clientType)
					accList.Add(new AccountDTO(GetClientByID(accounts[i].ClientID), accounts[i]));
			return accList;
		}
	}
}
