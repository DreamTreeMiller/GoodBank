using GoodBank.Interfaces_Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodBank.ClientClasses
{
	public abstract class Client : IClientDTO
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

		/// <summary>
		/// ID клиента в базе
		/// </summary>
		public uint ID { get; }

		#region Абстрактные свойства - реализация зависит от типа
		// нужны для показа общего списка клиентов - людей и организаций
		// они у каждого потомка будут реализованы по-разному
		// будут брать соответствующую информацию из конкретного свойства

		/// <summary>
		/// Отображаемое имя - ФИО для человека, название для организации
		/// </summary>
		public abstract string		DisplayName  { get; set; }

		/// <summary>
		/// Официальный Id клиента - номер паспорта для человека, ИНН - для организации
		/// </summary>
		public abstract string		IdNumber	 { get; set;  }

		/// <summary>
		/// Либо день рождения, либо дата регистрации компании
		/// </summary>
		public abstract	DateTime	CreationDate { get; set; }

		#endregion

		#region Свойства одинаковые для всех клиентов
		// Названия полей ниже говорят сами за себя

		public string	Telephone				{ get; set; }
		public string	Email					{ get; set; }
		public string	Address					{ get; set; }

		public int		NumberOfCurrentAccounts	{ get; set; }
		public int		NumberOfDeposits		{ get; set; }
		public int		NumberOfCredits			{ get; set; }
		public int		NumberOfClosedAccounts	{ get; set; }

		#endregion

		#region Конструктор нового счёта
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
	}
}
