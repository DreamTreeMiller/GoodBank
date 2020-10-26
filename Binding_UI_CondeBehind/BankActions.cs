using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoodBankNS.Interfaces_Actions;
using GoodBankNS.Interfaces_Data;

namespace GoodBankNS.Binding_UI_CondeBehind
{
	public class BankActions
	{
		public IClientsActions	Clients;
		public IAccountsActions	Accounts;
		public ITransactions	Log;

		public BankActions(IGoodBank bank)
		{
			Clients  = bank as IClientsActions;
			Accounts = bank as IAccountsActions;
			Log		 = bank as ITransactions;
		}
	}
}
