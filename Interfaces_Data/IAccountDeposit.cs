﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodBankNS.Interfaces_Data
{
	interface IAccountDeposit : IAccount
	{
		/// <summary>
		/// ID счета, куда перечислять проценты.
		/// При капитализации, совпадает с ИД счета депозита
		/// Без капитализации равен 0
		/// </summary>
		uint InterestAccumulationAccID { get; }

		/// <summary>
		/// Номер счета, куда перечислять проценты.
		/// При капитализации, совпадает с номером счета депозита
		/// Без капитализации имеет значение 
		/// </summary>
		string InterestAccumulationAccNum { get; }

		/// <summary>
		/// Если без капитализации 
		/// </summary>
		double AccumulatedInterest { get; set; }

		/// <summary>
		/// Переводит на другой счет накопленный процент.
		/// Это нельзя делать обычным переводом, т.к. деньги не списываются с основного счета
		/// </summary>
		/// <param name="destAcc"></param>
		/// <param name="accumulatedInterest"></param>
		void SendInterestToAccount(IAccount destAcc, double accumulatedInterest);
	}
}
