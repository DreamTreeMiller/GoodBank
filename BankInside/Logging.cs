using System.Linq;
using System.Collections.ObjectModel;
using Interfaces_Actions;
using Interfaces_Data;
using LoggingNS;

namespace BankInside
{
	public class Logging : ILogActions
	{
		private readonly IRepository dbe;
		public Logging(IRepository dbengine) { dbe = dbengine; }
		public void WriteLog(Transaction t)
		{
			dbe.WriteLog(t);
		}

		/// <summary>
		/// Формирует список всех транзакций указанного счета
		/// </summary>
		/// <param name="account"></param>
		/// <returns></returns>
		public ObservableCollection<ITransactionDTO> GetAccountTransactionsLog(int accID)
		{
			IQueryable<Transaction> accLog = from t in dbe.GetLog()
											 where t.TransactionAccountID == accID
											 select t;
			ObservableCollection <ITransactionDTO> accountLog = 
				new ObservableCollection<ITransactionDTO>(accLog);

			//foreach (Transaction t in db.Log)
			//	if (t.TransactionAccountID == accID) accountLog.Add(t);

			return accountLog;
		}
	}
}
