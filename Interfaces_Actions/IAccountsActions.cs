using GoodBankNS.AccountClasses;
using GoodBankNS.ClientClasses;
using GoodBankNS.DTO;
using GoodBankNS.Interfaces_Data;
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
		IAccount GetAccountByID(uint id);

		/// <summary>
		/// Находит список всех счетов, принадлежащих клиентам данного типа
		/// </summary>
		/// <param name="clientType">ВИП, обычный клиент или организация</param>
		/// <returns>
		/// Коллекцию счетов, принадлежащих клиентам данного типа
		/// </returns>
		(ObservableCollection<AccountDTO> accList, double totalCurr, double totalDeposit, double totalCredit)
			GetAccountsList(ClientType clientType);

		(ObservableCollection<AccountDTO> accList, double totalCurr, double totalDeposit, double totalCredit)
			GetClientAccounts(uint ID);

		ObservableCollection<AccountDTO> GetClientAccounts(uint ID, AccountType accType);

		ObservableCollection<IAccount> GetTopupableAccountsToWireFrom(uint sourceAccID);


		IAccountDTO AddAccount(IAccountDTO acc); 

		IAccountDTO GenerateAccount(IAccountDTO acc);

		IAccount TopUpCash(uint accID, double cashAmount);

		IAccount WithdrawCash(uint accID, double amount);

		IAccount CloseAccount(uint accID, out double accumulatedAmount);

		void Wire(uint sourceAccID, uint destAccID, double amount);

		/// <summary>
		/// Увеличивает внутреннюю дату на 1 месяц и пересчитывает проценты у всех счетов
		/// </summary>
		void AddOneMonth();
	}
}
