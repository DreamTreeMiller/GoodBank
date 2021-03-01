using System;
using System.Linq;
using System.Collections.ObjectModel;
using Interfaces_Actions;
using Interfaces_Data;
using DTO;
using AccountClasses;
using ClientClasses;
using LoggingNS;

namespace BankInside
{
	public class AccountActions : IAccountActions
	{
		private readonly IRepository dbe;

		public AccountActions(IRepository dbrep) { dbe = dbrep; }

		#region DB part

		public Account GetAccountByID(int id)
		{
			return dbe.GetAccountByID(id);
		}

		/// <summary>
		/// Добавляет счёт
		/// </summary>
		/// <param name="acc"></param>
		/// <returns>Возвращает созданный счет с уникальным ID счета</returns>
		public IAccountDTO AddAccount(IAccountDTO acc)
		{
			Account newAcc = null;
			Client  client = dbe.GetClientByID(acc.ClientID);
			switch (acc.AccType)
			{
				case AccountType.Current:
					newAcc = new AccountCurrent(acc, acc.Opened);
					client.NumberOfCurrentAccounts++;
					break;
				case AccountType.Deposit:
					newAcc = new AccountDeposit(acc, acc.Opened);
					client.NumberOfDeposits++;
					break;
				case AccountType.Credit:
					newAcc = new AccountCredit(acc, acc.Opened);
					client.NumberOfCredits++;
					break;
			}
			newAcc = dbe.AddAccount(newAcc);

			#region WriteLog

			string msg = "";
			switch (acc.AccType)
			{
				case AccountType.Current:
					msg =  "Текущий счет " + newAcc.AccountNumber
						+ $" с начальной суммой {newAcc.Balance:N2} руб."
						+  " открыт.";
					break;
				case AccountType.Deposit:
					msg =  "Вклад " + newAcc.AccountNumber
						+ $" с начальной суммой {newAcc.Balance:N2} руб."
						+  " открыт.";
					break;
				case AccountType.Credit:
					msg =  "Кредитный счет " + newAcc.AccountNumber
						+ $" на сумму {newAcc.Balance:N2} руб."
						+  " открыт.";
					break;
			}
			Transaction t = new Transaction()
			{
				TransactionAccountID = newAcc.AccountID,
				TransactionDateTime  = dbe.GetBankCurrentDateAndTime(),
				TransactionType		 = TransactionType.OpenAccount,
				SourceAccount		 = "",
				DestinationAccount	 = "",
				Amount				 = newAcc.Balance,
				Comment				 = msg
			};
			dbe.WriteLog(t); dbe.SaveChanges();

			#endregion

			return new AccountDTO(client, newAcc);
		}

		public IAccountDTO TopUpCash(int accID, double cashAmount)
		{
			Account acc = dbe.GetAccountByID(accID);

			if (acc.Topupable)
			{
				Transaction t = null;
				switch(acc.Deposit(cashAmount))
				{
					case 0:		// Topup successful

						// Update account which was topupped
						dbe.SaveChanges();

						#region Preparing log record 
						t = new Transaction()
						{ 
							TransactionAccountID = acc.AccountID,
							TransactionDateTime  = dbe.GetBankCurrentDateAndTime(),
							TransactionType		 = TransactionType.CashDeposit,
							SourceAccount		 = "",
							DestinationAccount	 = acc.AccountNumber,
							Amount				 = cashAmount,
							Comment				 =
								"Пополнение счета " + acc.AccountNumber + 
								$" наличными средствами на сумму {cashAmount:N2} руб."
						};
						#endregion
						
						break;

					case -1:    // Account was blocked. Nothing to update except log
						#region Preparing log record
						t = new Transaction()
						{
							TransactionAccountID = acc.AccountID,
							TransactionDateTime  = dbe.GetBankCurrentDateAndTime(),
							TransactionType		 = TransactionType.BlockAccount,
							SourceAccount		 = "",
							DestinationAccount	 = acc.AccountNumber,
							Amount				 = 0,
							Comment				 =
								  "Счет заблокирован." 
								+ " Количество пополнений больших или равных 1000 руб." 
								+ " превысило 3."
						};
						#endregion
						break;
				}
				dbe.WriteLog(t); dbe.SaveChanges();
			}
			return new AccountDTO(acc);
		}

		public IAccountDTO WithdrawCash(int accID, double cashAmount)
		{
			Account acc = dbe.GetAccountByID(accID);

			if (acc.WithdrawalAllowed)
			{
				acc.Withdraw(cashAmount);

				#region Write Log
				Transaction t = new Transaction()
				{
					TransactionAccountID = acc.AccountID,
					TransactionDateTime  = dbe.GetBankCurrentDateAndTime(),
					TransactionType		 = TransactionType.CashWithdrawal,
					SourceAccount		 = acc.AccountNumber,
					DestinationAccount	 = "",
					Amount				 = cashAmount,
					Comment				 =
						   "Снятие со счета " + acc.AccountNumber 
						+ $" наличных средств на сумму {cashAmount:N2} руб."
				};
				dbe.WriteLog(t);
				#endregion

				dbe.SaveChanges();
			}
			return new AccountDTO(acc);
		}

		/// <summary>
		/// Перевод средств со счета на счет
		/// </summary>
		/// <param name="sourceAccID"></param>
		/// <param name="destAccID"></param>
		/// <param name="wireAmount"></param>
		public void Wire(int sourceAccID, int destAccID, double wireAmount)
		{
			Account sourceAcc = dbe.GetAccountByID(sourceAccID);
			if (sourceAcc.WithdrawalAllowed)
			{
				if (sourceAcc.Balance >= wireAmount)
				{
					Account destAcc = dbe.GetAccountByID(destAccID);
					if (destAcc.Topupable)
					{
						sourceAcc.Withdraw(wireAmount);
						switch(destAcc.Deposit(wireAmount))
						{
							case 0:
								#region Write Log - send success, receive success

								Transaction sendSuccessTr = new Transaction()
								{
									TransactionAccountID = sourceAccID,
									TransactionDateTime  = dbe.GetBankCurrentDateAndTime(),
									TransactionType		 = TransactionType.SendWireToAccount,
									SourceAccount		 = sourceAcc.AccountNumber,
									DestinationAccount	 = destAcc.AccountNumber,
									Amount				 = wireAmount,
									Comment				 =
											 "Перевод со счета " + sourceAcc.AccountNumber
										  +  " на счет " + destAcc.AccountNumber
										  + $" средств на сумму {wireAmount:N2} руб."
								};
								dbe.WriteLog(sendSuccessTr);

								Transaction receiveSuccessTr = new Transaction()
								{
									TransactionAccountID = destAccID,
									TransactionDateTime  = dbe.GetBankCurrentDateAndTime(),
									TransactionType		 = TransactionType.ReceiveWireFromAccount,
									SourceAccount		 = sourceAcc.AccountNumber,
									DestinationAccount	 = destAcc.AccountNumber,
									Amount				 = wireAmount,
									Comment				 =
										   "Получение со счета " + sourceAcc.AccountNumber
										+  " на счет " + destAcc.AccountNumber
										+ $" средств на сумму {wireAmount:N2} руб."
								};
								dbe.WriteLog(receiveSuccessTr);
								#endregion

								break;
							case -1:  // cannot deposit
								// Return money back
								sourceAcc.DepositNonCash(wireAmount);

								#region Write Log - sending failed

								Transaction wireFailedTr = new Transaction()
								{
									TransactionAccountID = sourceAccID,
									TransactionDateTime  = dbe.GetBankCurrentDateAndTime(),
									TransactionType		 = TransactionType.TransactionFailed,
									SourceAccount		 = sourceAcc.AccountNumber,
									DestinationAccount	 = destAcc.AccountNumber,
									Amount				 = wireAmount,
									Comment				 =
										  $"Неудалось перевести средства на сумму {wireAmount:N2} руб."
										+  " Счет получателя " + destAcc.AccountNumber + " заблокирован."
								};
								dbe.WriteLog(wireFailedTr);
								#endregion

								break;
						}
						dbe.SaveChanges();
					}
				}
			}
		}

		/// <summary>
		/// Закрыть можно только нулевой счет
		/// Проверку на наличие денег на счете осуществляет вызывающий метод
		/// </summary>
		/// <param name="accID"></param>
		/// <returns></returns>
		public IAccountDTO CloseAccount(int accID, out double accumulatedAmount)
		{
			Account acc		  = dbe.GetAccountByID(accID);
			if (acc.IsBlocked)
			{
				accumulatedAmount = 0;
				return new AccountDTO(acc);
			}

			DateTime closingDate = dbe.GetBankCurrentDateAndTime();
			accumulatedAmount = acc.CloseAccount(closingDate);

			#region Write Log : Withdraw the rest and Close account
			if(accumulatedAmount > 0)
			{
				Transaction withdrawTr = new Transaction()
				{
					TransactionAccountID = acc.AccountID,
					TransactionDateTime  = closingDate,
					TransactionType		 = TransactionType.CashWithdrawal,
					SourceAccount		 = acc.AccountNumber,
					DestinationAccount	 = "",
					Amount				 = accumulatedAmount,
					Comment				 =
						   "Снятие со счета " + acc.AccountNumber
						+ $" наличных средств на сумму {accumulatedAmount:N2} руб."
				};
				dbe.WriteLog(withdrawTr);
			}
			Transaction closeAccTr = new Transaction()
			{
				TransactionAccountID = acc.AccountID,
				TransactionDateTime	 = dbe.GetBankCurrentDateAndTime(),
				TransactionType		 = TransactionType.CloseAccount,
				SourceAccount		 = "",
				Amount				 = 0,
				Comment				 = $"Счёт {acc.AccountNumber} закрыт."
			};
			dbe.WriteLog(closeAccTr);
			#endregion

			Client client = dbe.GetClientByID(acc.ClientID);
			client.NumberOfClosedAccounts++;

			switch(acc.AccType)
			{
				case AccountType.Current:
					client.NumberOfCurrentAccounts--;
					break;
				case AccountType.Deposit:
					client.NumberOfDeposits--;
					break;
				case AccountType.Credit:
					client.NumberOfCredits--;
					break;
			}
			dbe.SaveChanges();
			return new AccountDTO(acc);
		}

		public void AddOneMonth()
		{
			dbe.AddOneMonthToBankDate();

			foreach (Account acc in dbe.GetAccounts())
			{
				double accumulatedInterest = acc.RecalculateInterest();

				// У текущего счета accumulatedInterest всегда равен 0
				// Если счёт закрыт или заблокирован, то accumulatedInterest тоже равен 0
				if (accumulatedInterest == 0) continue;

				// Пишем журнал и, если надо, 
				// пересылаем проценты с депозита на другой счёт
				#region Write Log and send interest to another account, if necessary


				// Если счёт с капитализацией, кредит всегда с капитализацией,
				// тогда только одна запись в журнал. Пересылать процент не надо.
				if (acc.Compounding)
				{
					#region Write Log
					Transaction compoundAccuralTr = new Transaction()
					{
						TransactionAccountID = acc.AccountID,
						TransactionDateTime	 = dbe.GetBankCurrentDateAndTime(),
						TransactionType		 = TransactionType.InterestAccrual,
						SourceAccount		 = "накопленный процент",
						DestinationAccount	 = acc.AccountNumber,
						Amount				 = accumulatedInterest,
						Comment				 =
							$"Начисление процентов на сумму {accumulatedInterest:N2} руб."
					};
					dbe.WriteLog(compoundAccuralTr);
					#endregion
					continue;
				}

				// До этого места может дойти только депозит И без капитализации
				int destAccID = (acc as AccountDeposit).InterestAccumulationAccID;

				// Проверяем, указан ли номер счёта для перечисления процентов
				if (destAccID != 0)
				{
					Account destAcc = GetAccountByID(destAccID);

					// Попытаемся перевести проценты на счёт destAcc
					if (destAcc.Deposit(accumulatedInterest) == 0)
					{
						#region Write Log - sending interest successful
						Transaction wireInterestTr = new Transaction()
						{
							TransactionAccountID = acc.AccountID,
							TransactionDateTime	 = dbe.GetBankCurrentDateAndTime(),
							TransactionType		 = TransactionType.SendWireToAccount,
							SourceAccount		 = "накопленный процент",
							DestinationAccount	 = destAcc.AccountNumber,
							Amount				 = accumulatedInterest,
							Comment				 =
								$"Перевод процентов на сумму {accumulatedInterest:N2} руб."
							  +  " на счёт " + destAcc.AccountNumber + "."
						};
						dbe.WriteLog(wireInterestTr);
						#endregion

						#region Write Log - recieving successful

						Transaction receivedSuccessTr = new Transaction()
						{
							TransactionAccountID = destAcc.AccountID,
							TransactionDateTime	 = dbe.GetBankCurrentDateAndTime(),
							TransactionType		 = TransactionType.ReceiveWireFromAccount,
							SourceAccount		 = acc.AccountNumber,
							DestinationAccount	 = destAcc.AccountNumber,
							Amount				 = accumulatedInterest,
							Comment				 = 
								   "Получение со счета " + acc.AccountNumber
								+  " на счет " + destAcc.AccountNumber
								+ $" средств на сумму {accumulatedInterest:N2} руб."
						};
						dbe.WriteLog(receivedSuccessTr);
						#endregion
					}
					else
					{
						// Если не получается перечислить проценты на другой счёт,
						// например, счёт получатель был закрыт или заблокирован,
						// Пишем об этом в журнал
						#region Write Log - sending failed

						Transaction wireFailedTr = new Transaction()
						{
							TransactionAccountID = acc.AccountID,
							TransactionDateTime	 = dbe.GetBankCurrentDateAndTime(),
							TransactionType		 = TransactionType.TransactionFailed,
							SourceAccount		 = "накопленный процент",
							DestinationAccount	 = destAcc.AccountNumber,
							Amount				 = accumulatedInterest,
							Comment				 =
								$"Не удалось перевести проценты на сумму {accumulatedInterest:N2} руб."
							   + " на счёт " + destAcc.AccountNumber + "."
						};
						dbe.WriteLog(wireFailedTr);
						#endregion

						// то накапливаем их на вклад (капитализация)
						acc.DepositNonCash(accumulatedInterest);

						#region Write Log - top up sender account

						Transaction topupSenderAccTr = new Transaction()
						{
							TransactionAccountID = acc.AccountID,
							TransactionDateTime	 = dbe.GetBankCurrentDateAndTime(),
							TransactionType		 = TransactionType.InterestAccrual,
							SourceAccount		 = "накопленный процент",
							DestinationAccount	 = acc.AccountNumber,
							Amount				 = accumulatedInterest,
							Comment				 =
							$"Начисление процентов на сумму {accumulatedInterest:N2} руб."
						};
						dbe.WriteLog(topupSenderAccTr);
						#endregion

					}
				}
				else // начисляем проценты на внутренний счёт
				{
					#region Write Log - internal account interest accrual

					Transaction intAccInterestAccrual = new Transaction()
					{
						TransactionAccountID = acc.AccountID,
						TransactionDateTime	 = dbe.GetBankCurrentDateAndTime(),
						TransactionType		 = TransactionType.InterestAccrual,
						SourceAccount		 = "накопленный процент",
						DestinationAccount	 = "внутренний счет",
						Amount				 = accumulatedInterest,
						Comment				 =
						   "Начисление процентов на внутренний счет"
						+ $" на сумму {accumulatedInterest:N2} руб."
					};
					dbe.WriteLog(intAccInterestAccrual);
					#endregion
				}
				#endregion
			}
			dbe.SaveChanges();
		}
		#endregion

		#region UI part

		/// <summary>
		/// Формирует список счетов данного типа клиентов.
		/// </summary>
		/// <param name="clientType">Тип клиента</param>
		/// <returns>
		/// коллекцию счетов и общую сумму каждой группы счетов - текущие, вклады, кредиты
		/// </returns>
		public (ObservableCollection<IAccountDTO> accList, double totalCurr, double totalDeposit, double totalCredit)
			GetAccountsList(ClientType clientType)
		{
			ObservableCollection<IAccountDTO> accList = new ObservableCollection<IAccountDTO>();
			double totalCurr = 0, totalDeposit = 0, totalCredit = 0;

			if (clientType == ClientType.All)
			{
				foreach (Account acc in dbe.GetAccounts())
				{
					switch (acc.AccType)
					{
						case AccountType.Current:
							totalCurr += acc.Balance;
							break;
						case AccountType.Deposit:
							totalDeposit += acc.Balance;
							// Добавляем средства, накопленные на внутренних счетах депозитов,
							// которые будут переведены на счет с номером,
							// и соответственно доступны для снятия,
							// только после окончания срока вклада
							if (!acc.Compounding && (acc as AccountDeposit).InterestAccumulationAccID == 0)
								totalDeposit += (acc as AccountDeposit).AccumulatedInterest;
							break;
						case AccountType.Credit:
							totalCredit += acc.Balance;
							break;
					}
					accList.Add(new AccountDTO(dbe.GetClientByID(acc.ClientID), acc));
				}
				return (accList, totalCurr, totalDeposit, totalCredit);
			}

			IQueryable<Account> clientTypeAccounts = from acc in dbe.GetAccounts()
													 where acc.ClientType == clientType
													 select acc;

			foreach (Account acc in clientTypeAccounts)
			{
				switch (acc.AccType)
				{
					case AccountType.Current:
						totalCurr += acc.Balance;
						break;
					case AccountType.Deposit:
						totalDeposit += acc.Balance;
						// Добавляем средства, накопленные на внутренних счетах депозитов,
						// которые будут переведены на счет с номером,
						// и соответственно доступны для снятия,
						// только после окончания срока вклада
						if (!acc.Compounding && (acc as AccountDeposit).InterestAccumulationAccID == 0)
							totalDeposit += (acc as AccountDeposit).AccumulatedInterest;
						break;
					case AccountType.Credit:
						totalCredit += acc.Balance;
						break;
				}
				accList.Add(new AccountDTO(dbe.GetClientByID(acc.ClientID), acc));
			}
			return (accList, totalCurr, totalDeposit, totalCredit);
		}

		/// <summary>
		/// Формирует список счетов заданного клиента.
		/// </summary>
		/// <param name="clientID">ID клиента</param>
		/// <returns>
		/// возвращает коллекцию счетов и общую сумму каждой группы счетов - текущие, вклады, кредиты
		/// </returns>
		public (ObservableCollection<IAccountDTO> accList, double totalCurr, double totalDeposit, double totalCredit)
			GetClientAccounts(int clientID)
		{
			ObservableCollection<IAccountDTO> accList = new ObservableCollection<IAccountDTO>();
			Client client = dbe.GetClientByID(clientID);
			double totalCurr = 0, totalDeposit = 0, totalCredit = 0;

			var clientAccounts = from acc in dbe.GetAccounts()
								 where acc.ClientID == clientID
								 select acc;

			foreach (Account acc in clientAccounts)
			{
				switch (acc.AccType)
				{
					case AccountType.Current:
						totalCurr += acc.Balance;
						break;
					case AccountType.Deposit:
						totalDeposit += acc.Balance;
						break;
					case AccountType.Credit:
						totalCredit += acc.Balance;
						break;
				}
				accList.Add(new AccountDTO(client, acc));
			}
			return (accList, totalCurr, totalDeposit, totalCredit);
		}

		/// <summary>
		/// Фомирует спискок счетов заданного типа у данного клиента
		/// </summary>
		/// <param name="clientID"></param>
		/// <param name="accType"></param>
		/// <returns></returns>
		public ObservableCollection<IAccountDTO> GetClientAccounts(int clientID, AccountType accType)
		{
			ObservableCollection<IAccountDTO> accList = new ObservableCollection<IAccountDTO>();
			Client client = dbe.GetClientByID(clientID);

			var clientAccounts = from acc in dbe.GetAccounts()
								 where acc.ClientID == clientID && acc.AccType == accType
								 select acc;

			foreach (Account acc in clientAccounts) accList.Add(new AccountDTO(client, acc));

			//foreach (Account acc in db.Accounts)
			//{
			//	if (acc.ClientID == clientID && acc.AccType == accType)
			//		accList.Add(new AccountDTO(client, acc));
			//}

			return accList;

		}

		/// <summary>
		/// Формирует коллекцию номеров счетов, на которые можно переводить деньги 
		/// </summary>
		/// <param name="sourceAccID"></param>
		/// <returns></returns>
		public ObservableCollection<IAccountDTO> GetTopupableAccountsToWireTo(int sourceAccID)
		{
			ObservableCollection<IAccountDTO> accList = new ObservableCollection<IAccountDTO>();

			var topupableAccounts = from acc in dbe.GetAccounts()
									where acc.Topupable && acc.AccountID != sourceAccID
									select acc;

			foreach (Account tAcc in topupableAccounts) accList.Add(new AccountDTO(tAcc));

			//foreach (Account acc in db.Accounts)
			//	if (acc.Topupable && acc.AccountID != sourceAccID)
			//	{
			//		accList.Add(new AccountDTO(acc));
			//	}
			return accList;
		}
		#endregion
	}
}
