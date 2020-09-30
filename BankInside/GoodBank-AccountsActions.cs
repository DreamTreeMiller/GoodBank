using GoodBank.Client_Classes;
using GoodBank.DTO;
using GoodBank.Interfaces_Actions;
using GoodBank.Interfaces_Data;
using System.Collections.ObjectModel;

namespace GoodBank.BankInside
{
	public partial class GoodBank : IAccountsActions
	{
		public ObservableCollection<ShowAccountDTO> GetAccountsList(ClientType clientType)
		{
			ObservableCollection<ShowAccountDTO> accList = new ObservableCollection<ShowAccountDTO>();
			for (int i = 0; i < accounts.Count; i++)
				if (accounts[i].ClientType == clientType)
					accList.Add(new ShowAccountDTO(accounts[i]));
			return accList;
		}
	}
}
