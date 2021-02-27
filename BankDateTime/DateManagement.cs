using Interfaces_Actions;
using System;

namespace BankInside
{
	public class DateManagement : IDateManagement
	{
		private readonly IRepository dbe;
		public DateManagement(IRepository dbengine)
		{
			dbe = dbengine;
		}

		public DateTime BankFoundationDay()
		{
			return dbe.GetBankFoundationDate();
		}

		public DateTime Today()
		{
			return dbe.GetBankCurrentDateAndTime();
		}
	}
}
