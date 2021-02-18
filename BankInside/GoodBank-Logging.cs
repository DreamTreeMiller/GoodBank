using Interfaces_Actions;
using Interfaces_Data;
using Transaction_Class;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace BankInside
{
	public partial class GoodBank : ILogActions
	{
		public void WriteLog(Transaction t)
		{
			db.Log.Add(t);
			db.SaveChanges();
		}

		/// <summary>
		/// Формирует список всех транзакций указанного счета
		/// </summary>
		/// <param name="account"></param>
		/// <returns></returns>
		public ObservableCollection<ITransactionDTO> GetAccountTransactionsLog(int accID)
		{
			ObservableCollection<ITransactionDTO> accountLog = new ObservableCollection<ITransactionDTO>();
			foreach (Transaction t in db.Log)
				if (t.TransactionAccountID == accID) accountLog.Add(t);
			return accountLog;
		}
	}
}
