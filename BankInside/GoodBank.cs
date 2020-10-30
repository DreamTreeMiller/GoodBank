using GoodBankNS.AccountClasses;
using GoodBankNS.ClientClasses;
using GoodBankNS.Interfaces_Actions;
using GoodBankNS.Interfaces_Data;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using GoodBankNS.Transaction_Class;
using System;

namespace GoodBankNS.BankInside
{
	public partial class GoodBank : IGoodBank
	{
		public static DateTime BankFoundationDay = new DateTime(1992, 1, 1);
		public static DateTime Today			 = DateTime.Now;
		public GoodBank()
		{
			clients  = new List<Client>();
			accounts = new List<Account>();
			log		 = new List<Transaction>();
		}

		public static DateTime GetBanksTodayWithCurrentTime()
		{
			return new DateTime(Today.Year, Today.Month, Today.Day, 
				DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
		}
	}
}
