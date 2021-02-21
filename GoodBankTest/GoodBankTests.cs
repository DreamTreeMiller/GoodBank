using Microsoft.VisualStudio.TestTools.UnitTesting;
using AccountClasses;
using ClientClasses;
using Interfaces_Data;
using DTO;
using System;

namespace BankInside.Tests
{
	[TestClass()]
	public class GoodBankTests
	{
		[TestMethod()]
		public void GenerateAccountTest()
		{

			
			// Arrange
			// Создаём банк
			GoodBank goodBank = new GoodBank();
			// Создаём контейнер данных о клиенте
			IClientDTO client = new ClientDTO
				(ClientType.VIP, "John", "", "Doe", new DateTime(1980, 1, 12),
				"1111 223344", "+1234567890","e@mail.ru", "1-23, Long Street, City 321 Nation");
			// Создаём запись о клиенте в базе, на основе данных из контейнера
			client = goodBank.AddClient(client);
			// Получаем ID созданного клиента, точнее ID в базе записи о клиенте
			int clientID = client.ID;

			// Создаём контейнер данных о счёте
			IAccountDTO testAcc = new AccountDTO()
			{
				AccountNumber = "",
				AccType = AccountType.Credit,
				AccumulatedInterest = 0,
				Balance = 1000.0,
				ClientID = clientID,
				ClientType = ClientType.VIP,
				Closed = null,
				Compounding = true,
				Duration = 24,
				Interest = 0.07,
				InterestAccumulationAccID = 0,
				InterestAccumulationAccNum = "internal account",
				IsBlocked = false,
				MonthsElapsed = 0,
				Opened = DateTime.Now,
				RecalcPeriod = RecalcPeriod.Monthly,
				Topupable = true,
				WithdrawalAllowed = false
			};


			// Act 
			// Создаём в базе запись о счёте
			IAccountDTO acc = goodBank.GenerateAccount(testAcc);

			// Assert
			// Проверяем, что в результате сформировалась запись о счёте в базен
			// и ей был присвоен ID, отличный от нуля
			Assert.AreNotEqual(0, acc.AccountID);

			// По идее, надо ещё проверить, что метод вернул структуру
			// которая отличается от исходной только ID
		}
	}
}