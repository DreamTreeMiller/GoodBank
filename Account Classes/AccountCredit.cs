using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodBankNS.AccountClasses
{
	public class AccountCredit : Account
	{
		public override AccountType AccountType { get => AccountType.Credit; }
		public override int Balance { get; set; }

		public AccountCredit()
		{
			AccountNumber = "CRE" + AccountNumber;
		}
	}
}
