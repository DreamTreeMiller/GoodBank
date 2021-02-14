using System.Data.Entity;
using AccountClasses;
using ClientClasses;
using Transaction_Class;

namespace EF
{
	class BankContext : DbContext
	{
		public BankContext() : base("dbBankConnection") { }

		public DbSet<Account>		Accounts	 { get; set; }
		public DbSet<Client>		Clients		 { get; set; }
		public DbSet<Transaction>	Transactions { get; set; }
	}
}
