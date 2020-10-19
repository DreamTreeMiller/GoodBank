using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodBankNS.AccountClasses
{
	public enum AccountType
	{
		Current,
		Deposit,
		Credit,
		Total
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

	public enum RecalcPeriod
	{
		NoRecalc,
		Daily,
		Monthly,
		Annually,
		AtTheEnd
	}
}
