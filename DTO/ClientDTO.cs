using GoodBank.ClientClasses;
using GoodBank.Interfaces_Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;

namespace GoodBank.DTO
{
	/// <summary>
	/// Структура для передачи данных при работе с клиентом 
	/// При показе или вводе данных о клиенте
	/// Не меняется в процессе, поэтому все поля заполняются на этапе создания
	/// через конструктор
	/// </summary>
	public class ClientDTO : IClientDTO
	{
		public uint			ID						{ get; }
		public ClientType	ClientType				{ get; set; }
		public string		ClientTypeShow			
		{
			get
			{
				string tmp = "";
				switch(ClientType)
				{
					case ClientType.VIP:
						tmp = "VIP";
						break;
					case ClientType.Simple:
						tmp = "Обычный";
						break;
					case ClientType.Organization:
						tmp = "Организация";
						break;
				}
				return tmp;
			}
		}
		public string		FirstName				{ get; set; }
		public string		MiddleName				{ get; set; }
		public string		LastName				{ get; set; }
		public string		MainName				{ get; set; }
		public string		DirectorName			{ get; set; }
		public string		PassportOrTIN			{ get; set; }
		public DateTime		CreationDate			{ get; set; }
		public string		Telephone				{ get; set; }
		public string		Email					{ get; set; }
		public string		Address					{ get; set; }
		public int			NumberOfCurrentAccounts { get; set; } = 0;
		public int			NumberOfDeposits		{ get; set; } = 0;
		public int			NumberOfCredits			{ get; set; } = 0;
		public int			NumberOfClosedAccounts	{ get; set; } = 0;

		public ClientDTO()
		{
		}
		/// <summary>
		/// Конструктор заполняет поля структуры 
		/// либо для показа, либо при создании
		/// </summary>
		/// <param name="id">Уникальный номер клиента</param>
		/// <param name="clType">Тип (класс) клиента</param>
		/// <param name="firstName">Имя клиента (если организация, то директора)</param>
		/// <param name="middleName">Отчество клиента (если организация, то директора)</param>
		/// <param name="lastName">Фамилия клиента (если организация, то директора)</param>
		/// <param name="mainN">ФИО либо название организации</param>
		/// <param name="dirN">ФИО директора</param>
		/// <param name="idNum">Номер паспорта или ИНН</param>
		/// <param name="crD">Дата рождения или создания организации</param>
		/// <param name="tel">Телефон</param>
		/// <param name="email">Эл. почта</param>
		/// <param name="addr">Адрес</param>
		/// <param name="numCA">Количество текущих счетов</param>
		/// <param name="numDep">Количество вкладов</param>
		/// <param name="numCr">Количество кредитов</param>
		/// <param name="numClsdAcc">Количество закрытых счетов</param>
		public ClientDTO(uint id, ClientType clType,
			string firstName, string middleName, string lastName,
			string mainN, string dirN, string idNum, DateTime crD,
			string tel, string email,  string addr,  
			int numCA, int numDep, int numCr, int numClsdAcc)
		{
			ID						= id;
			ClientType				= clType;
			FirstName				= firstName;
			MiddleName				= middleName;
			LastName				= lastName;
			MainName				= mainN;
			DirectorName			= dirN;
			PassportOrTIN			= idNum;
			CreationDate			= crD;
			Telephone				= tel;
			Email					= email;
			Address					= addr;
			NumberOfCurrentAccounts = numCA;
			NumberOfDeposits		= numDep;
			NumberOfCredits			= numCr;
			NumberOfClosedAccounts	= numClsdAcc;
		}

		public ClientDTO(IClient c)
		{
			ID						= c.ID;
			Telephone				= c.Telephone;
			Email					= c.Email;
			Address					= c.Address;
			NumberOfCurrentAccounts = c.NumberOfCurrentAccounts;
			NumberOfDeposits		= c.NumberOfDeposits;
			NumberOfCredits			= c.NumberOfCredits;
			NumberOfClosedAccounts  = c.NumberOfClosedAccounts;

			// Надо присвоить хоть какие-то значения, иначе компилятор будет ругаться
			ClientType		= ClientType.VIP;
			FirstName = MiddleName = LastName = "";
			MainName		= "";
			DirectorName	= "";
			PassportOrTIN	= "";
			CreationDate = DateTime.Now;

			if (c is IClientVIP)
			{
				FirstName		= (c as IClientVIP).FirstName;
				MiddleName		= (c as IClientVIP).MiddleName;
				LastName		= (c as IClientVIP).LastName;
				PassportOrTIN	= (c as IClientVIP).PasspostNumber;
				CreationDate	= (c as IClientVIP).BirthDate;

				// Это надо для показа клиента
				MainName		= LastName + FirstName +
					(String.IsNullOrEmpty(MiddleName) ? "" : " ") +
					MiddleName;
				DirectorName	= "";
				return;
			}

			if (c is IClientSimple)
			{
				ClientType		= ClientType.Simple;
				FirstName		= (c as IClientSimple).FirstName;
				MiddleName		= (c as IClientSimple).MiddleName;
				LastName		= (c as IClientSimple).LastName;
				PassportOrTIN	= (c as IClientSimple).PasspostNumber;
				CreationDate	= (c as IClientSimple).BirthDate;

				// Это надо для показа клиента
				MainName		= LastName + FirstName +
					(String.IsNullOrEmpty(MiddleName) ? "" : " ") +
					MiddleName;
				DirectorName	= "";
			}

			if (c is IClientOrg)
			{
				ClientType	  = ClientType.Organization;
				MainName	  = (c as IClientOrg).OrgName;
				DirectorName  = 
					(c as IClientOrg).DirectorLastName +
					(c as IClientOrg).DirectorFirstName +
					(String.IsNullOrEmpty((c as IClientOrg).DirectorMiddleName) ? "" : " ") +
					(c as IClientOrg).DirectorMiddleName;
				PassportOrTIN = (c as IClientOrg).TIN;
				CreationDate  = (c as IClientOrg).RegistrationDate;
			}
		}

	}
}
