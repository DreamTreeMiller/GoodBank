using AccountClasses;
using ClientClasses;
using DTO;
using Interfaces_Data;
using System.Collections.ObjectModel;

namespace Interfaces_Actions
{
	public interface IAccountActions
	{
		#region BackEnd part

		IAccountDTO AddAccount(IAccountDTO acc); 

		IAccountDTO TopUpCash(int accID, double cashAmount);

		IAccountDTO WithdrawCash(int accID, double amount);

		IAccountDTO CloseAccount(int accID, out double accumulatedAmount);

		void Wire(int sourceAccID, int destAccID, double amount);

		/// <summary>
		/// Увеличивает внутреннюю дату на 1 месяц и пересчитывает проценты у всех счетов
		/// </summary>
		void AddOneMonth();

		#endregion

		#region UI part

		/// <summary>
		/// Находит список всех счетов, принадлежащих клиентам данного типа
		/// </summary>
		/// <param name="clientType">ВИП, обычный клиент или организация</param>
		/// <returns>
		/// Коллекцию счетов, принадлежащих клиентам данного типа
		/// Общуюю сумму на каждом типе счетов - текущих, депозитов и кредитов
		/// </returns>
		(ObservableCollection<IAccountDTO> accList, double totalCurr, double totalDeposit, double totalCredit)
			GetAccountsList(ClientType clientType);

		/// <summary>
		/// Находит список всех счетов, принадлежащих данному клиету
		/// </summary>
		/// <param name="clientID">ID клиента или организация</param>
		/// <returns>
		/// Коллекцию счетов, принадлежащих клиентам данного типа
		/// Общуюю сумму на каждом типе счетов - текущих, депозитов и кредитов
		/// </returns>
		(ObservableCollection<IAccountDTO> accList, double totalCurr, double totalDeposit, double totalCredit)
			GetClientAccounts(int clientID);

		ObservableCollection<IAccountDTO> GetClientAccounts(int clientID, AccountType accType);

		/// <summary>
		/// Выдаёт список пополняемых счетов, не включая указанный.
		/// Метод нужен для окна перевода денег со счёта на счёт
		/// </summary>
		/// <param name="sourceAccID">Счёт, с которого переводят деньги</param>
		/// <returns></returns>
		ObservableCollection<IAccountDTO> GetTopupableAccountsToWireTo(int sourceAccID);

		#endregion

	}
}
