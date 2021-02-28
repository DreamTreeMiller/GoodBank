using System.Collections.ObjectModel;
using System.Linq;
using Interfaces_Actions;
using Interfaces_Data;
using LoggingNS;
using EF;

namespace BankInside
{
	public partial class GoodBank : ILogActions
	{
		public void WriteLog(Transaction t)
		{
			//using (db = new BankContext())
			//{
				db.Log.Add(t);
				//db.SaveChanges();
			//}
		}

		/// <summary>
		/// Формирует список всех транзакций указанного счета
		/// </summary>
		/// <param name="account"></param>
		/// <returns></returns>
		public ObservableCollection<ITransactionDTO> GetAccountTransactionsLog(int accID)
		{
			IQueryable<Transaction> accLog = from t in db.Log
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
