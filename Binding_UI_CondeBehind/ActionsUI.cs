using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoodBank.Interfaces_Actions;
using GoodBank.Interfaces_Data;

namespace GoodBank.Binding_UI_CondeBehind
{
	class ActionsUI
	{
		public IClientsActions	Clients;
		public IAccountsActions	Accounts;
		public ITransactions	Log;

		public ActionsUI(IGoodBank bank)
		{
			Clients  = bank as IClientsActions;
			Accounts = bank as IAccountsActions;
			Log		 = bank as ITransactions;
		}
	}
}
