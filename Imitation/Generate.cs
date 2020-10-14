using GoodBankNS.ClientClasses;
using GoodBankNS.Interfaces_Data;
using GoodBankNS.Interfaces_Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoodBankNS.DTO;
using GoodBankNS.Binding_UI_CondeBehind;

namespace GoodBankNS.Imitation
{
	public static class Generate
	{
		static BankActions BA;
		static Random r = new Random();
		static int orgCount = 1;

		public static void Bank(BankActions ba, int vip, int sim, int org)
		{
			BA = ba;
			GenerateVIPclientsAndAccounts(vip);
			GenerateSIMclientsAndAccounts(sim);
			GenerateORGclientsAndAccounts(org);
		}


		private static void GenerateVIPclientsAndAccounts(int num)
		{
			string FN, MN, LN;
			for (int i = 0; i < num; i++)
			{
				// Half will be men, half women
				if ((i & 1) == 0)
				{   // Male names
					FN = GenMFN(); MN = GenMMN(); LN = GenMLN();
				}
				else
				{   // Female names
					FN = GenFFN(); MN = GenFMN(); LN = GenFLN();
				}

				// Генерируем контейнер для передачи данных в бекэнд
				ClientDTO client =
					new ClientDTO(	ClientType.VIP, FN, MN, LN,
									GenBirthDate(), GenPassportNum(), GenTel(), GenEmail(),
									"Тропики, Лазурный берег, Жемчужный дворец, комната 8");
				BA.Clients.AddClient(client);
			}
		}

		private static void GenerateSIMclientsAndAccounts(int num)
		{
			string FN, MN, LN;
			for (int i = 0; i < num; i++)
			{
				// Half will be men, half women
				if ((i & 1) == 0)
				{   // Male names
					FN = GenMFN(); MN = GenMMN(); LN = GenMLN();
				}
				else
				{   // Female names
					FN = GenFFN(); MN = GenFMN(); LN  = GenFLN();
				}

				// Генерируем контейнер для передачи данных в бекэнд
				ClientDTO client = 
					new ClientDTO(	ClientType.Simple, FN, MN, LN,
									GenBirthDate(), GenPassportNum(), GenTel(), GenEmail(),
									"Мой адрес не дом и не улица. Здесь был Вася.");
				BA.Clients.AddClient(client);
			}
		}

		private static void GenerateORGclientsAndAccounts(int num)
		{
			string DFN, DMN, DLN;
			for (int i = 0; i < num-1; i++)
			{
				// Half will be men, half women
				if ((i & 1) == 0)
				{   // Male names
					DFN = GenMFN(); DMN = GenMMN(); DLN = GenMLN();
				}
				else
				{   // Female names
					DFN = GenFFN(); DMN = GenFMN(); DLN = GenFLN();
				}

				// Генерируем контейнер для передачи данных в бекэнд
				ClientDTO client =
					new ClientDTO(	ClientType.Organization, GenOrgName(), DFN, DMN, DLN,
									GenRegDate(), GenTIN(), 
									GenTel(), GenEmail(), GenOrgAddress());
				BA.Clients.AddClient(client);
			}

			BA.Clients.AddClient(
				new ClientDTO(ClientType.Organization, 
				"Организация с ооооочччченнннь ооооччччееень длиннным названиеммммммммм",
				GenMFN(), GenMMN(), GenMLN(), GenRegDate(), GenTIN(),
									GenTel(), GenEmail(), GenOrgAddress()));
		}

		private static string GenMFN()
		{
			return Names.MFN[r.Next(0, Names.MFN.Length)];
		}

		private static string GenMMN()
		{
			return Names.MMN[r.Next(0, Names.MMN.Length)];
		}

		private static string GenMLN()
		{
			return Names.MLN[r.Next(0, Names.MLN.Length)];
		}

		private static string GenFFN()
		{
			return Names.FFN[r.Next(0, Names.FFN.Length)];
		}

		private static string GenFMN()
		{
			return Names.FMN[r.Next(0, Names.FMN.Length)];
		}

		private static string GenFLN()
		{
			return Names.FLN[r.Next(0, Names.FLN.Length)];
		}

		private static string GenOrgName()
		{
			return $"Организация {++orgCount}";
		}

		private static string GenPassportNum()
		{
			return $"{r.Next(1, 10_000):0000} {r.Next(1,1_000_000):000000}"; 
		}

		private static string GenTIN()
		{
			return $"{r.Next(1,86):00}{r.Next(1,100):00}{r.Next(1,1_000_000):000000}";
		}
		private static string GenTel()
		{
			return "+7 9" + $"{r.Next(0, 100):00} "                        // area code
					+ $"{r.Next(1, 1000):000}-" + $"{r.Next(0, 10000):0000}";	// telephone
		}

		private static DateTime GenBirthDate()
		{
			DateTime startDate =
				new DateTime(DateTime.Now.Year - 100,
							 DateTime.Now.Month,
							 DateTime.Now.Day);
			DateTime endDate =
				new DateTime(DateTime.Now.Year - 19,
							 DateTime.Now.Month,
							 DateTime.Now.Day);
			return GenDate(startDate, endDate);
		}

		private static DateTime GenRegDate()
		{
			DateTime startDate = new DateTime(1990, 1, 1);
			DateTime endDate   = DateTime.Now;
			return GenDate(startDate, endDate);
		}


		private static DateTime GenDate(DateTime sd, DateTime ed)
		{
			if (sd >= ed) return ed;
			DateTime ndate;
			int ny = r.Next(sd.Year, ed.Year + 1);
			int nm = r.Next(1, 13);
			int nd = 1;
			if (nm == 2) nd = r.Next(1, 29);
			else if (nm == 4 || nm == 6 || nm == 9 || nm == 11)
				nd = r.Next(1, 31);
			else nd = r.Next(1, 32);
			ndate = new DateTime(ny, nm, nd);
			if (ndate < sd) ndate = sd;
			if (ndate > ed) ndate = ed;
			return ndate;
		}

		private static string GenEmail()
		{
			return "email" + Name() + "@" + Domain[r.Next(0, Domain.Length)] + TopDomain[r.Next(0, TopDomain.Length)];
		}

		public static string[] Domain = { "gmail", "yahoo", "outlook", "mail", "mymail", "yourmail", "onemail", "bmail", "cmail", "paper"};
		public static string[] TopDomain = { ".com", ".org", ".edu", ".gov", ".ru", ".net", ".xyz" };

		public static string Name()
		{
			return Guid.NewGuid().ToString().Substring(0, 5);
		}

		private static string GenOrgAddress()
		{
			return $"Город_{r.Next(0, 100)}, ул. Улица_{r.Next(0, 100)}, {r.Next(0, 100)} офс. {r.Next(0, 1000)}";
		}

	}
}
