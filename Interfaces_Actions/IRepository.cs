using System;
using System.Linq;
using AccountClasses;
using ClientClasses;
using LoggingNS;

namespace Interfaces_Actions
{
	public interface IRepository
	{
		DateTime GetBankFoundationDate();
		DateTime GetBankCurrentDateAndTime();
		void	 AddOneMonthToBankDate();

		IQueryable<Client>	GetClients(); 
		Client	GetClientByID(int clientID);
		Client	AddClient(Client client);

		IQueryable<Account>	GetAccounts();
		Account	GetAccountByID(int accountID);
		Account	AddAccount(Account account);

		IQueryable<Transaction>	 GetLog();
		void	WriteLog(Transaction t);

		void	SaveChanges();
	}
}
