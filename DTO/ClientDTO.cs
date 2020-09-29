using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodBank.DTO
{
	public struct ClientDTO
	{
		uint ID { get; }
		string DisplayName { get; set; }
		string IdNumber { get; set; }
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
