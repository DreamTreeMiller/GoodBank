using System.Data.Entity;
using AccountClasses;
using ClientClasses;
using LoggingNS;
using BankDate;

namespace EF
{
	public class BankContext : DbContext
	{
		public BankContext() : base("dbBankConnection") { }

		public DbSet<Account>		Accounts { get; set; }
		public DbSet<Client>		Clients	 { get; set; }
		public DbSet<Transaction>	Log		 { get; set; }
		public DbSet<GoodBankDate>	GBDate	 { get; set; }
	}
}
