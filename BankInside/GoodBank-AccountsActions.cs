using GoodBank.Client_Classes;
using GoodBank.Interfaces_Actions;
using GoodBank.Interfaces_Data;
using System.Collections.ObjectModel;

namespace GoodBank.BankInside
{
	public partial class GoodBank : IAccountsActions
	{
		public ObservableCollection<IAccountDTO> GetAccountsList(ClientType clientType)
		{
			ObservableCollection<IAccountDTO> accList = new ObservableCollection<IAccountDTO>();
			return accList;
		}
	}
}
