using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodBank.Interfaces_Data
{
	public interface IClientOrg : IClient
	{
		string OrgName				{ get; set; }
		string DirectorFirstName	{ get; set; }
		string DirectorMiddleName	{ get; set; }
		string DirectorLastName		{ get; set; }

		/// <summary>
		/// ИНН - Taxpayer Individual Number
		/// </summary>
		string TIN					{ get; set; }

		/// <summary>
		/// Дата регистрации организации
		/// </summary>
		DateTime RegistrationDate	{ get; set; }
	}
}
