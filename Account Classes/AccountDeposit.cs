using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodBank.AccountClasses
{
	public class AccountDeposit : Account
	{
		public override AccountType AccountType { get => AccountType.Deposit; }
		public override int Balance { get; set; }

		public AccountDeposit()
		{
			AccountNumber = "DEP" + AccountNumber;
		}
	}
}
