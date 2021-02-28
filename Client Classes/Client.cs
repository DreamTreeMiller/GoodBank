using Interfaces_Data;

namespace ClientClasses
{
	public abstract class Client
	{

		#region Свойства одинаковые для всех клиентов

		/// <summary>
		/// ID клиента в базе
		/// </summary>
		public int		ID						{ get; set; }

		public string	Telephone				{ get; set; }
		public string	Email					{ get; set; }
		public string	Address					{ get; set; }

		public int		NumberOfCurrentAccounts	{ get; set; }
		public int		NumberOfDeposits		{ get; set; }
		public int		NumberOfCredits			{ get; set; }
		public int		NumberOfClosedAccounts	{ get; set; }

		#endregion

		#region Конструкторы

		/// <summary>
		/// Базовый конструктор для любого клиента. Обнуляет количество всех счетов
		/// </summary>
		/// <param name="tel">Телефон</param>
		/// <param name="email">Электронная почта</param>
		/// <param name="address">Адрес</param>
		public Client(string tel, string email, string address)
		{
			Telephone				= tel;
			Email					= email;
			Address					= address;
			NumberOfCurrentAccounts	= 0;
			NumberOfDeposits		= 0;
			NumberOfCredits			= 0;
			NumberOfClosedAccounts	= 0;
		}

		/// <summary>
		/// Конструктор для работы Entity Framework
		/// </summary>
		public Client() { }

		#endregion

		public abstract void UpdateMyself(IClientDTO updatedClient);
	}
}
