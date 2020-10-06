using GoodBankNS.AccountClasses;
using GoodBankNS.ClientClasses;
using GoodBankNS.DTO;
using GoodBankNS.Interfaces_Actions;
using GoodBankNS.Interfaces_Data;
using System.Collections.ObjectModel;

namespace GoodBankNS.BankInside
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
