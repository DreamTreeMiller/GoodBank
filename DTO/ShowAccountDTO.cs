using GoodBank.AccountClasses;
using GoodBank.Interfaces_Actions;
using GoodBank.Interfaces_Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodBank.DTO
{
	/// <summary>
	/// Структура для показа данных о любом счете
	/// Не меняется в процессе, поэтому все поля заполняются на этапе создания
	/// через конструктор
	/// </summary>
	public struct ShowAccountDTO
	{
		public string			ClientType		{ get; }
		public string			ClientName		{ get; }
		public string			AccountType		{ get; }
		public uint				ID				{ get; }
		public string			AccountNumber	{ get; }
		public int				CurrentAmount	{ get; }
		public int				DepositAmount	{ get; }
		public int				DebtAmount		{ get; }
		public int				Interest		{ get; }
		public string			AccountStatus	{ get; }
		public DateTime			Opened			{ get; }
		public DateTime			Closed			{ get; }

		public ShowAccountDTO(IAccount acc)
		{
			ClientType		
			ClientName
			AccountType
			ID
			AccountNumber
			CurrentAmount
			DepositAmount
			DebtAmount
			Interest
			AccountStatus
			Opened
			Closed
		}
	}
}
