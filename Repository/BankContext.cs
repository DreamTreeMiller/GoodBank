using System.Data.Entity;
using AccountClasses;
using ClientClasses;
using LoggingNS;
using BankDateTime;

namespace Repository
{
	public class BankContext : DbContext
	{
		public BankContext() : base("dbBankConnection") { }

		public DbSet<Account>		Accounts { get; set; }
		public DbSet<Client>		Clients  { get; set; }
		public DbSet<Transaction>	Log		 { get; set; }
		public DbSet<GoodBankDates> GBDate	 { get; set; }
	}
}
