using GoodBank.Interfaces_Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodBank.ClientClasses
{
	public class ORGclient : Client, IOrgClient
	{
		#region Название организации, ФИО директора, ИНН, дата регистрации

		public string	OrgName				{ get; set; }
		public string	DirectorFirstName	{ get; set; }
		public string	DirectorMiddleName	{ get; set; } = "";
		public string	DirectorLastName	{ get; set; }
		
		/// <summary>
		/// ИНН - Taxpayer Individual Number
		/// </summary>
		public string	TIN	{ get; set; }

		/// <summary>
		/// Дата регистрации организации
		/// </summary>
		public DateTime	RegistrationDate	{ get; set; }

		#endregion

		#region Конструктор

		public ORGclient(string orgName, string TIN, DateTime regDate,
						 string dfn, string dmn,   string dln,
						 string tel, string email, string address)
			: base(tel, email, address)
		{
			OrgName				= orgName;
			this.TIN			= TIN;
			RegistrationDate	= regDate;
			DirectorFirstName	= dfn;
			DirectorMiddleName	= dmn;
			DirectorLastName	= dln;
		}

		#endregion
	}
}
