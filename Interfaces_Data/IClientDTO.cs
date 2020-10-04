﻿using GoodBank.ClientClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodBank.Interfaces_Data
{
	public interface IClientDTO
	{
		uint ID { get; }
		ClientType ClientType { get; set; }
		string FirstName { get; set; }
		string MiddleName { get; set; }
		string LastName { get; set; }
		string MainName { get; set; }
		string DirectorName { get; set; }
		string PassportOrTIN { get; set; }
		DateTime CreationDate { get; set; }
		string Telephone { get; set; }
		string Email { get; set; }
		string Address { get; set; }
		int NumberOfCurrentAccounts { get; set; }
		int NumberOfDeposits { get; set; }
		int NumberOfCredits { get; set; }
		int NumberOfClosedAccounts { get; set; }
	}
}
