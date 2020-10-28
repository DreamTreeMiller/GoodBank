using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodBankNS.Interfaces_Data
{
	public interface IClientSimple : IClient
	{
		string FirstName		{ get; set; }
		string MiddleName		{ get; set; }
		string LastName			{ get; set; }
		string PassportNumber	{ get; set; }
		DateTime BirthDate		{ get; set; }
	}
}
