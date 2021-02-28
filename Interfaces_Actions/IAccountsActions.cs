using AccountClasses;
using ClientClasses;
using DTO;
using Interfaces_Data;
using System.Collections.ObjectModel;

namespace Interfaces_Actions
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
		(ObservableCollection<IAccountDTO> accList, double totalCurr, double totalDeposit, double totalCredit)
			GetAccountsList(ClientType clientType);

		(ObservableCollection<IAccountDTO> accList, double totalCurr, double totalDeposit, double totalCredit)
			GetClientAccounts(int ID);

		ObservableCollection<IAccountDTO> GetClientAccounts(int ID, AccountType accType);

		ObservableCollection<IAccountDTO> GetTopupableAccountsToWireTo(int sourceAccID);


		IAccountDTO AddAccount(IAccountDTO acc); 

		IAccountDTO GenerateAccount(IAccountDTO acc);

		IAccountDTO TopUpCash(int accID, double cashAmount);

		IAccountDTO WithdrawCash(int accID, double amount);

		IAccountDTO CloseAccount(int accID, out double accumulatedAmount);

		void Wire(int sourceAccID, int destAccID, double amount);

		/// <summary>
		/// Увеличивает внутреннюю дату на 1 месяц и пересчитывает проценты у всех счетов
		/// </summary>
		void AddOneMonth();
	}
}
