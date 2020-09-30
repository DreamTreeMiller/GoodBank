using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodBank.AccountClasses
{
	public class AccountCurrent : Account
	{
		public override int Balance { get; set; }
		public AccountCurrent()
		{
			AccountNumber = "CUR" + AccountNumber;
		}
	}
}
