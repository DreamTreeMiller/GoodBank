using GoodBank.Client_Classes;
using GoodBank.Interfaces_Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodBank.Imitation
{
	public static class Generate
	{
		static IGoodBank goodBank;
		public static IGoodBank Bank(int vip, int sim, int org)
		{
			goodBank = new BankInside.GoodBank();
			GenerateVIPclientsAndAccounts(vip);
			GenerateSIMclientsAndAccounts(sim);
			GenerateORGclientsAndAccounts(org);
			return goodBank;
		}

		private void GenerateVIPclientsAndAccounts(int num)
		{
			for(int i = 0; i < num; i++)
				goodBank.AddClient(ClientType.VIP)
		}
	}
}
