using System;
using System.Linq;
using Interfaces_Actions;
using BankDateTime;
using AccountClasses;
using ClientClasses;
using LoggingNS;

namespace Repository
{
	public class DataBaseEngine : IRepository
	{
		private readonly BankContext db;
		private readonly DateTime	 gbFoundationDay;
		private			 DateTime	 today;
		public DataBaseEngine()
		{
			db = new BankContext();

			// Считываем текущую дату в банке
			GoodBankDates gbDate = db.GBDate.Find(1);

			// Если база только что создана, то устанавливаем текущую дату на сегодняшнюю
			if (gbDate == null)
			{
				gbDate = new GoodBankDates()
				{
					BankFoundationDay = new DateTime(1992, 1, 1),
					Today			  = DateTime.Now
				};
				db.GBDate.Add(gbDate);
				db.SaveChanges();
			}
			gbFoundationDay = gbDate.BankFoundationDay;
			today			= gbDate.Today;
		}

		/// <summary>
		/// Возвращает дату основания банка
		/// </summary>
		/// <returns></returns>
		public DateTime GetBankFoundationDate() { return gbFoundationDay; }

		/// <summary>
		/// Возвращает текущие дату и время банка
		/// </summary>
		/// <returns></returns>
		public DateTime GetBankCurrentDateAndTime()
		{
			return new DateTime(today.Year, today.Month, today.Day,
				DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
		}

		/// <summary>
		/// Увеличивает текущую дату банка на 1 месяц
		/// </summary>
		public void		AddOneMonthToBankDate()
		{
			today = today.AddMonths(1);
			GoodBankDates gbDate = db.GBDate.Find(1);
			gbDate.Today = today;
			db.SaveChanges();
		}

		public IQueryable<Client>  GetClients()	 { return db.Clients; }
		public Client GetClientByID(int id)		 { return db.Clients.Find(id);
		}
		public Client AddClient(Client client)
		{
			db.Clients.Add(client);
			db.SaveChanges();
			return client;
		}

		public IQueryable<Account> GetAccounts() { return db.Accounts; }
		public Account GetAccountByID(int id)	 { return db.Accounts.Find(id); }
		public Account AddAccount(Account account)
		{
			db.Accounts.Add(account);
			db.SaveChanges();
			return account;
		}

		public IQueryable<Transaction> GetLog()	 { return db.Log; }
		public void WriteLog(Transaction t)		 { db.Log.Add(t); }

		public void SaveChanges()				 { db.SaveChanges(); }
	}
}
