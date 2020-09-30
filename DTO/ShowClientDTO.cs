using GoodBank.Client_Classes;
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
	/// Структура для показа данных о любом клиенте
	/// Не меняется в процессе, поэтому все поля заполняются на этапе создания
	/// через конструктор
	/// </summary>
	public struct ShowClientDTO
	{
		public uint			ID						{ get; }
		public string		ClientType				{ get; }
		public string		MainName				{ get; }
		public string		DirectorName			{ get; }
		public string		IdNumber				{ get; }
		public DateTime		CreationDate			{ get; }
		public string		Telephone				{ get; }
		public string		Email					{ get; }
		public string		Address					{ get; }
		public int			NumberOfCurrentAccounts	{ get; }
		public int			NumberOfDeposits		{ get; }
		public int			NumberOfCredits			{ get; }
		public int			NumberOfClosedAccounts	{ get; }

		public ShowClientDTO(uint id, string clType,	   string mainN, string dirN, string idNum, DateTime crD,
							 string tel, string email, string addr,  int numCA, int numDep, int numCr, int numClsdAcc)
		{
			ID						= id;
			ClientType				= clType;
			MainName				= mainN;
			DirectorName			= dirN;
			IdNumber				= idNum;
			CreationDate			= crD;
			Telephone				= tel;
			Email					= email;
			Address					= addr;
			NumberOfCurrentAccounts = numCA;
			NumberOfDeposits		= numDep;
			NumberOfCredits			= numCr;
			NumberOfClosedAccounts	= numClsdAcc;
		}

		public ShowClientDTO(IClient c)
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
			ClientType	 = "VIP";
			MainName	 = "";
			DirectorName = "";
			IdNumber	 = "";
			CreationDate = DateTime.Now;

			if (c is IClientVIP)
			{
				MainName =
					(c as IClientVIP).LastName +
					(c as IClientVIP).FirstName +
					(String.IsNullOrEmpty((c as IClientVIP).MiddleName) ? "" : " ") +
					(c as IClientVIP).MiddleName;
				DirectorName = "";
				IdNumber = (c as IClientSimple).PasspostNumber;
				CreationDate = (c as IClientSimple).BirthDate;
			}

			if (c is IClientSimple)
			{
				ClientType = "Simple";
				MainName =
					(c as IClientSimple).LastName +
					(c as IClientSimple).FirstName +
					(String.IsNullOrEmpty((c as IClientSimple).MiddleName) ? "" : " ") +
					(c as IClientSimple).MiddleName;
				DirectorName = "";
				IdNumber = (c as IClientSimple).PasspostNumber;
				CreationDate = (c as IClientSimple).BirthDate;
			}

			if (c is IClientOrg)
			{
				ClientType = "Org";
				MainName = (c as IClientOrg).OrgName;
				DirectorName = 
					(c as IClientOrg).DirectorLastName +
					(c as IClientOrg).DirectorFirstName +
					(String.IsNullOrEmpty((c as IClientOrg).DirectorMiddleName) ? "" : " ") +
					(c as IClientOrg).DirectorMiddleName;
				IdNumber = (c as IClientOrg).TIN;
				CreationDate = (c as IClientOrg).RegistrationDate;
			}
		}

	}
}
