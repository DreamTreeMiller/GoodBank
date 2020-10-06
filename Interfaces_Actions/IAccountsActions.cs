using GoodBankNS.AccountClasses;
using GoodBankNS.ClientClasses;
using GoodBankNS.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodBankNS.Interfaces_Actions
{
	public interface IAccountsActions
	{
		/// <summary>
		/// Находит список всех счетов, принадлежащих клиентам данного типа
		/// </summary>
		/// <param name="clientType">ВИП, обычный клиент или организация</param>
		/// <returns>
		/// Коллекцию счетов, принадлежащих клиентам данного типа
		/// </returns>
		ObservableCollection<AccountDTO> GetAccountsList(ClientType clientType);
	}
}
