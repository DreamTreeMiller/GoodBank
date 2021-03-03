using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AccountClasses;
using ClientClasses;
using Interfaces_Actions;
using Interfaces_Data;
using DTO;
using BankInside;

namespace GoodBankTest
{
	[TestClass()]
	public class GoodBankTests
	{
		[TestMethod()]
		public void GenerateAccountTest()
		{

			
			// Arrange
			// Создаём банк
			IRepository dbe = new MockRepository();
			AccountActions accActionsTest = new AccountActions(dbe);

			// Создаём запись о клиенте в базе, на основе данных из контейнера
			Client client = new ClientVIP() { ID = 1 };
			client = dbe.AddClient(client);

			// Создаём контейнер данных о счёте
			IAccountDTO testAcc = new AccountDTO()
			{
				AccountNumber = "",
				AccType = AccountType.Credit,
				AccumulatedInterest = 0,
				Balance = 1000.0,
				ClientID = client.ID,
				ClientType = ClientType.VIP,
				Closed = null,
				Compounding = true,
				Duration = 24,
				Interest = 0.07,
				InterestAccumulationAccID = 0,
				InterestAccumulationAccNum = "внутренний счёт",
				IsBlocked = false,
				MonthsElapsed = 0,
				Opened = DateTime.Now,
				RecalcPeriod = RecalcPeriod.Monthly,
				Topupable = true,
				WithdrawalAllowed = false
			};

			// Act 
			// Создаём в базе запись о счёте
			IAccountDTO acc = accActionsTest.AddAccount(testAcc);

			// Assert
			// Проверяем, что в результате сформировалась запись о счёте в базен
			// и ей был присвоен ID, отличный от нуля
			Assert.AreNotEqual(0, acc.AccountID);

			// По идее, надо ещё проверить, что метод вернул структуру
			// которая отличается от исходной только ID
		}
	}
}