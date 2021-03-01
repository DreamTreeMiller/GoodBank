using Interfaces_Actions;
using BankInside;
using Repository;
using Search;
using LoggingNS;

namespace Binding_UI_CondeBehind
{
	public class BankActions
	{
		public IAccountActions	Accounts;
		public IClientActions	Clients;
		public ILogActions		Log;
		public IDateManagement	GBDateTime;
		public ISearch			Search;
		private IRepository		dbe; 

		public BankActions()
		{
			dbe	 = new DataBaseEngine();
			Accounts	= new AccountActions(dbe);
			Clients		= new ClientActions(dbe);
			Log			= new Logging(dbe);
			GBDateTime	= new DateManagement(dbe);
			Search		= new SearchEngine(dbe);
		}
	}
}
