using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodBank.AccountClasses
{
	public enum AccountType
	{
		Current,
		Deposit,
		Credit
	}

	public enum AccountStatus
	{
		Opened,
		Closed
	}

	public enum OperationType
	{
		CashDeposit,
		CashWithdrawal,
		DepositFromAccount,
		TransferToAccount
	}
}
