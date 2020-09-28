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
		string FullName { get; set; }
		DateTime BirthDate { get; set; }
		string PassportNum { get; set; }
		string Tel { get; set; }
		string Email { get; set; }
		string Address { get; set; }
		int NumberOfCurrAccs { get; set; }
		int NumOfDeposits { get; set; }
		int NumOfCredits { get; set; }
		int NumOfClosedAccs { get; set; }

	}
}
