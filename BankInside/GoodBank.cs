using System;
using EF;

namespace BankInside
{
	public partial class GoodBank
	{
		private BankContext	db;
		public GoodBank()
		{
			db = new BankContext();
			InitializeBankDate();
		}
	}
}
