using System;
using System.Linq;
using System.Collections.Generic;
using AccountClasses;
using ClientClasses;
using Interfaces_Actions;
using LoggingNS;

namespace GoodBankTest
{
	public class MockRepository : IRepository
	{
		private DateTime FoundationDay = new DateTime(1992, 1, 1);
		private DateTime Today		   = DateTime.Now;

		private int accountCounter = 0;
		private List<Account> accounts = new List<Account>();

		private int clientCounter = 0;
		private List<Client>   clients = new List<Client>();

		private int transCounter = 0;
		private List<Transaction>  log = new List<Transaction>();
		public Account AddAccount(Account account)
		{
			account.AccountID = ++accountCounter;
			if(account is AccountCurrent)
			{
				account.AccountNumber = $"SAV{accountCounter:000000000000}";
			}
			else if(account is AccountDeposit)
			{
				account.AccountNumber = $"DEP{accountCounter:000000000000}";
			}
			else if(account is AccountCredit)
			{
				account.AccountNumber = $"CRE{accountCounter:000000000000}";
			}
			accounts.Add(account);
			return account;
		}

		public Client AddClient(Client client)
		{
			client.ID = ++clientCounter;
			clients.Add(client);
			return client;
		}

		public void		AddOneMonthToBankDate()		{ Today = Today.AddMonths(1); }
		public DateTime GetBankCurrentDateAndTime()	{ return Today;	}
		public DateTime GetBankFoundationDate()		{ return FoundationDay; }


		public Account GetAccountByID(int accountID) 
		{ return accounts.Find(acc => acc.AccountID == accountID); }

		public Client GetClientByID(int clientID)
		{ return clients.Find(c => c.ID == clientID); }

		public void SaveChanges() {	}

		public void WriteLog(Transaction t)
		{ t.TransactionID = ++transCounter; log.Add(t); }

		public IQueryable<Client> GetClients() { return clients.AsQueryable(); }
		public IQueryable<Account> GetAccounts() { return accounts.AsQueryable(); }
		public IQueryable<Transaction> GetLog() { return log.AsQueryable(); }
	}
}
