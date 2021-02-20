using Interfaces_Actions;
using BankInside;

namespace Binding_UI_CondeBehind
{
	public class BankActions
	{
		public IClientsActions	Clients;
		public IAccountsActions	Accounts;
		public ILogActions		Log;
		public ISearch			Search;
		private GoodBank		bank; 

		public BankActions()
		{
			bank	 = new GoodBank();
			Clients  = bank;
			Accounts = bank;
			Log		 = bank;
			Search	 = bank;
		}
	}
}
