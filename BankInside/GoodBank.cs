using GoodBankNS.AccountClasses;
using GoodBankNS.ClientClasses;
using GoodBankNS.Interfaces_Actions;
using GoodBankNS.Interfaces_Data;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using GoodBankNS.Transaction_Class;

namespace GoodBankNS.BankInside
{
	public partial class GoodBank : IGoodBank
	{
		private List<Client>		clients;
		private List<Account>		accounts;
		private List<Transaction>	log;
		public GoodBank()
		{
			clients  = new List<Client>();
			accounts = new List<Account>();
			log		 = new List<Transaction>();
		}

		/// <summary>
		/// Находит клиента с указанным ID
		/// </summary>
		/// <param name="id">ID клиента</param>
		/// <returns></returns>
		public IClient GetClientByID(uint id)
		{
			return clients.Find(c => c.ID == id);
		}

	}
}
