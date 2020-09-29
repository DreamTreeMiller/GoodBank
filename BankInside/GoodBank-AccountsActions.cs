using GoodBank.Client_Classes;
using GoodBank.DTO;
using GoodBank.Interfaces_Actions;
using System.Collections.ObjectModel;

namespace GoodBank.BankInside
{
	public partial class GoodBank : IAccountsActions
	{
		public ObservableCollection<AccountDTO> GetAccountsList(ClientType clientType)
		{
			ObservableCollection<AccountDTO> accList = new ObservableCollection<AccountDTO>();
			return accList;
		}
	}
}
