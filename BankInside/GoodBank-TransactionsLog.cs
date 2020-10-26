using GoodBankNS.DTO;
using GoodBankNS.Interfaces_Actions;
using GoodBankNS.Interfaces_Data;
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
		/// <summary>
		/// Формирует список всех транзакций указанного счета
		/// </summary>
		/// <param name="account"></param>
		/// <returns></returns>
		public ObservableCollection<ITransactionDTO> GetTransactionsLog(AccountDTO account)
		{
			ObservableCollection<ITransactionDTO> log = new ObservableCollection<ITransactionDTO>();
			foreach (var t in log)
				if (t.Account.ID == account.ID) log.Add(t);
			return log;
		}
	}
}
