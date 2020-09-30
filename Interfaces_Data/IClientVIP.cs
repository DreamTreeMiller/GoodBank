using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodBank.Interfaces_Data
{
	public interface IClientVIP : IClient
	{
		string FirstName		{ get; set; }
		string MiddleName		{ get; set; }
		string LastName			{ get; set; }
		string PasspostNumber	{ get; set; }
		DateTime BirthDate		{ get; set; }
	}
}
