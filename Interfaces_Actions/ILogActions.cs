using DTO;
using Interfaces_Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces_Actions
{
	public interface ILogActions
	{
		ObservableCollection<ITransactionDTO> GetAccountTransactionsLog(int accID);
	}
}
