using EF;

namespace BankInside
{
	public partial class GoodBank
	{
		private BankContext	db;
		public GoodBank()
		{
			//using (db = new BankContext())
			//{
				db = new BankContext();
				InitializeBankDate();
			//}
		}
	}
}
