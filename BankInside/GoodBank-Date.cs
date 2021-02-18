using System;
using BankDate;

namespace BankInside
{
	public partial class GoodBank
	{
		public static DateTime BankFoundationDay;
		public static DateTime Today;
		private GoodBankDate   gbDate;

		public void InitializeBankDate()
		{
			// Считываем текущую дату в банке
			gbDate = db.GBDate.Find(1);

			// Если база только что создана, то устанавливаем текущую дату на сегодняшнюю
			if (gbDate == null)
			{
				gbDate = new GoodBankDate()
				{
					BankFoundationDay = new DateTime(1992, 1, 1),
					Today			  = DateTime.Now
				};
				db.GBDate.Add(gbDate);
				db.SaveChanges();
			}
			BankFoundationDay = gbDate.BankFoundationDay;
			Today = gbDate.Today;
		}
		public static DateTime GetBanksTodayWithCurrentTime()
		{
			return new DateTime(Today.Year, Today.Month, Today.Day,
				DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
		}

		public void AddOneMonthToBankDate()
		{
			Today = Today.AddMonths(1);
			gbDate.Today = Today;
			db.SaveChanges();

		}

	}
}
