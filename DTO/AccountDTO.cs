using GoodBank.AccountClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodBank.DTO
{
	public struct AccountDTO
	{
		AccountType AccountType { get; set; }
		uint ID { get; }
		string AccountNumber { get; set; }
		string DisplayName { get; set; }
		int CurrentAmount { get; set; }
		int DepositAmount { get; set; }
		int DebtAmount { get; set; }
		int Interest { get; set; }
		AccountStatus AccountStatus { get; set; }
		DateTime Opened { get; set; }
		DateTime Closed { get; set; }

	}
}
