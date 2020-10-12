using GoodBankNS.ClientClasses;
using GoodBankNS.Interfaces_Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GoodBankNS.DTO
{
	/// <summary>
	/// Data Transfer Object для передачи данных при работе с клиентом 
	/// При показе или вводе данных о клиенте
	/// Ручной ввод осуществляется только в свойства с { get; set; }
	/// </summary>
	public class ClientDTO : IClientDTO, INotifyPropertyChanged
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
						tmp = "ВИП";
						break;
					case ClientType.Simple:
						tmp = "Физик";
						break;
					case ClientType.Organization:
						tmp = "Юрик";
						break;
				}
				return tmp;
			}
		}
		private string		_firstName;
		public string		FirstName
		{ 
			get => _firstName; 
			set
			{
				_firstName = value;
				NotifyPropertyChanged();
				if (ClientType == ClientType.Organization)
					NotifyPropertyChanged("DirectorName");
				else
					NotifyPropertyChanged("MainName");
			}
		}

		private string		_middleName;
		public string		MiddleName
		{
			get => _middleName;
			set
			{
				_middleName = value;
				NotifyPropertyChanged();
				if (ClientType == ClientType.Organization)
					NotifyPropertyChanged("DirectorName");
				else
					NotifyPropertyChanged("MainName");
			}
		}

		private string		_lastName;
		public string		LastName
		{
			get => _lastName;
			set
			{
				_lastName = value;
				NotifyPropertyChanged();
				if (ClientType == ClientType.Organization)
					NotifyPropertyChanged("DirectorName");
				else
					NotifyPropertyChanged("MainName");
			}
		}

		private string		orgName;

		/// <summary>
		/// Содержит либо полноые ФИО, либо название организации
		/// в зависимости от типа клиента
		/// </summary>
		public string		MainName				
		{ 
			get
			{
				if (ClientType == ClientType.Organization) return orgName;
				// Это надо для показа клиента
				return	LastName + " " + FirstName +
						(String.IsNullOrEmpty(MiddleName) ? "" : " ") +
						MiddleName;

			}
			set { orgName = value; }
		}

		/// <summary>
		/// Поле для показа в списке
		/// </summary>
		public string		DirectorName			
		{ 
			get
			{
				if (ClientType != ClientType.Organization) return "";
				return	LastName + " " + FirstName +
						(String.IsNullOrEmpty(MiddleName) ? "" : " ") +
						MiddleName;
			}
		}
		public DateTime		CreationDate			{ get; set; }
		public string		PassportOrTIN			{ get; set; }
		public string		Telephone				{ get; set; }
		public string		Email					{ get; set; }
		public string		Address					{ get; set; }
		public int			NumberOfCurrentAccounts { get; } = 0;
		public int			NumberOfDeposits		{ get; } = 0;
		public int			NumberOfCredits			{ get; } = 0;
		public int			NumberOfClosedAccounts	{ get; } = 0;

		public ClientDTO() { }

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

			if (c is IClientVIP)
			{
				ClientType		= ClientType.VIP;
				FirstName		= (c as IClientVIP).FirstName;
				MiddleName		= (c as IClientVIP).MiddleName;
				LastName		= (c as IClientVIP).LastName;
				PassportOrTIN	= (c as IClientVIP).PasspostNumber;
				CreationDate	= (c as IClientVIP).BirthDate;
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
			}

			if (c is IClientOrg)
			{
				ClientType		= ClientType.Organization;
				MainName		= (c as IClientOrg).OrgName;
				FirstName		= (c as IClientOrg).DirectorFirstName;
				MiddleName		= (c as IClientOrg).DirectorMiddleName;
				LastName		= (c as IClientOrg).DirectorLastName;
				PassportOrTIN	= (c as IClientOrg).TIN;
				CreationDate	= (c as IClientOrg).RegistrationDate;
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		// This method is called by the Set accessor of each property.  
		// The CallerMemberName attribute that is applied to the optional propertyName  
		// parameter causes the property name of the caller to be substituted as an argument.  
		private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
