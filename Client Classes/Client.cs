﻿using GoodBankNS.Interfaces_Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodBankNS.ClientClasses
{
	public abstract class Client : IClient
	{

		#region Статическая часть для генерации уникального ID

		/// <summary>
		/// Текущий ID счета
		/// </summary>
		private static uint staticID;

		/// <summary>
		/// Статический конструктор. Обнуляет счетчик ID
		/// </summary>
		static Client()
		{
			staticID = 0;
		}

		/// <summary>
		/// Герерирует следующий ID
		/// </summary>
		/// <returns>New unique ID</returns>
		private static uint NextID()
		{
			staticID++;
			return staticID;
		}

		#endregion

		#region Свойства одинаковые для всех клиентов

		/// <summary>
		/// ID клиента в базе
		/// </summary>
		public uint		ID						{ get; }

		public string	Telephone				{ get; set; }
		public string	Email					{ get; set; }
		public string	Address					{ get; set; }

		public int		NumberOfCurrentAccounts	{ get; set; }
		public int		NumberOfDeposits		{ get; set; }
		public int		NumberOfCredits			{ get; set; }
		public int		NumberOfClosedAccounts	{ get; set; }

		#endregion

		#region Конструктор нового клиента

		/// <summary>
		/// Базовый конструктор для любого клиента. Обнуляет количество всех счетов
		/// </summary>
		/// <param name="tel">Телефон</param>
		/// <param name="email">Электронная почта</param>
		/// <param name="address">Адрес</param>
		public Client(string tel, string email, string address)
		{
			ID						= NextID();
			Telephone				= tel;
			Email					= email;
			Address					= address;
			NumberOfCurrentAccounts	= 0;
			NumberOfDeposits		= 0;
			NumberOfCredits			= 0;
			NumberOfClosedAccounts	= 0;
		}

		#endregion

		public abstract void UpdateMyself(IClientDTO updatedClient);
	}
}
