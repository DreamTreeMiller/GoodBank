using GoodBank.DTO;
using GoodBank.Interfaces_Actions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodBank.BankInside
{
	public partial class GoodBank : ITransactions
	{
		/// <summary>
		/// Формирует список всех транзакций указанного счета
		/// </summary>
		/// <param name="account"></param>
		/// <returns></returns>
		public ObservableCollection<TransactionDTO> GetTransactionsLog(AccountDTO account)
		{
			ObservableCollection<TransactionDTO> log = new ObservableCollection<TransactionDTO>();
			foreach (var t in log)
				if (t.Account.ID == account.ID) log.Add(t as TransactionDTO);
			return log;
		}
	}
}
