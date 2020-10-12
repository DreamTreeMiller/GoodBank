using GoodBankNS.ClientClasses;
using GoodBankNS.Interfaces_Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;

namespace GoodBankNS.DTO
{
	/// <summary>
	/// Data Transfer Object для передачи данных при работе с клиентом 
	/// При показе или вводе данных о клиенте
	/// Ручной ввод осуществляется только в свойства с { get; set; }
	/// </summary>
	public class ClientDTO : IClientDTO
	{
		public uint			ID						{ get; }
		public ClientType	ClientType				{ get; set; }
		public string		ClientTypeTag			
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
		public string		DirectorName			{ get; }
		public DateTime		CreationDate			{ get; set; }
		public string		PassportOrTIN			{ get; set; }
		public string		Telephone				{ get; set; }
		public string		Email					{ get; set; }
		public string		Address					{ get; set; }
		public int			NumberOfCurrentAccounts { get; } = 0;
		public int			NumberOfDeposits		{ get; } = 0;
		public int			NumberOfCredits			{ get; } = 0;
		public int			NumberOfClosedAccounts	{ get; } = 0;

		public ClientDTO()
		{
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
				MainName		= LastName + " " + FirstName +
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
				MainName		= LastName + " " + FirstName +
					(String.IsNullOrEmpty(MiddleName) ? "" : " ") +
					MiddleName;
				DirectorName	= "";
			}

			if (c is IClientOrg)
			{
				ClientType		= ClientType.Organization;
				MainName		= (c as IClientOrg).OrgName;
				FirstName		= (c as IClientOrg).DirectorFirstName;
				MiddleName		= (c as IClientOrg).DirectorMiddleName;
				LastName		= (c as IClientOrg).DirectorLastName;
				DirectorName	= 
					(c as IClientOrg).DirectorLastName + " " +
					(c as IClientOrg).DirectorFirstName +
					(String.IsNullOrEmpty((c as IClientOrg).DirectorMiddleName) ? "" : " ") +
					(c as IClientOrg).DirectorMiddleName;
				PassportOrTIN	= (c as IClientOrg).TIN;
				CreationDate	= (c as IClientOrg).RegistrationDate;
			}
		}

	}
}
