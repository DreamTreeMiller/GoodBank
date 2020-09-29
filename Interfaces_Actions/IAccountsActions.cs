using GoodBank.AccountClasses;
using GoodBank.Client_Classes;
using GoodBank.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodBank.Interfaces_Actions
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
