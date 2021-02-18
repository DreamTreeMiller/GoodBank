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
		private List<Transaction> log;

		public void WriteLog(Transaction t)
		{
			log.Add(t);
		}

		/// <summary>
		/// Формирует список всех транзакций указанного счета
		/// </summary>
		/// <param name="account"></param>
		/// <returns></returns>
		public ObservableCollection<ITransactionDTO> GetAccountTransactionsLog(int accID)
		{
			ObservableCollection<ITransactionDTO> accountLog = new ObservableCollection<ITransactionDTO>();
			foreach (var t in log)
				if (t.TransactionAccountID == accID) accountLog.Add(t);
			return accountLog;
		}
	}
}
