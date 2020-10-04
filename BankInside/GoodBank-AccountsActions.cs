using GoodBank.AccountClasses;
using GoodBank.ClientClasses;
using GoodBank.DTO;
using GoodBank.Interfaces_Actions;
using GoodBank.Interfaces_Data;
using System.Collections.ObjectModel;

namespace GoodBank.BankInside
{
	public partial class GoodBank : IAccountsActions
	{
		public void AddAccount(IAccount account)
		{
			accounts.Add(account as Account);
		}

		public ObservableCollection<AccountDTO> GetAccountsList(ClientType clientType)
		{
			ObservableCollection<AccountDTO> accList = new ObservableCollection<AccountDTO>();
			for (int i = 0; i < accounts.Count; i++)
				if (accounts[i].ClientType == clientType)
					accList.Add(new AccountDTO(accounts[i], GetClientByID(accounts[i].ClientID)));
			return accList;
		}
	}
}
