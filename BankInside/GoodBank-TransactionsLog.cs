using GoodBankNS.DTO;
using GoodBankNS.Interfaces_Actions;
using GoodBankNS.Interfaces_Data;
using GoodBankNS.Transaction_Class;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodBankNS.BankInside
{
	public partial class GoodBank : ITransactions
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
		public ObservableCollection<ITransaction> GetAccountTransactionsLog(uint accID)
		{
			ObservableCollection<ITransaction> accountLog = new ObservableCollection<ITransaction>();
			foreach (var t in log)
				if (t.TransactionAccountID == accID) accountLog.Add(t);
			return accountLog;
		}
	}
}
