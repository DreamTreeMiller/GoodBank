using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodBankNS.AccountClasses
{
	public class AccountCurrent : Account
	{
		public override AccountType AccountType { get => AccountType.Current; }
		public override int Balance { get; set; }

		public AccountCurrent()
		{
			AccountNumber = "CUR" + AccountNumber;
		}
	}
}
