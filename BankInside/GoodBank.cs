using System;
using System.Collections.Generic;
using AccountClasses;
using ClientClasses;
using Transaction_Class;
using EF;

namespace BankInside
{
	public partial class GoodBank
	{
		public static DateTime BankFoundationDay = new DateTime(1992, 1, 1);
		public static DateTime Today			 = DateTime.Now;
		private BankContext		db;
		public GoodBank()
		{
			log		 = new List<Transaction>();
			db		 = new BankContext();
		}

		public static DateTime GetBanksTodayWithCurrentTime()
		{
			return new DateTime(Today.Year, Today.Month, Today.Day, 
				DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
		}
	}
}
