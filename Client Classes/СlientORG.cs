using GoodBankNS.Interfaces_Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodBankNS.ClientClasses
{
	public class СlientORG : Client, IClientOrg
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

		/// <summary>
		/// Конструктор создает организацию.
		/// </summary>
		/// <param name="orgName">Название организации</param>
		/// <param name="TIN">ИНН</param>
		/// <param name="regDate">Дата регистрации</param>
		/// <param name="dfn">Имя директора</param>
		/// <param name="dmn">Отчетство директора</param>
		/// <param name="dln">Фамилия директора</param>
		/// <param name="tel">Телефон организации</param>
		/// <param name="email">Эл. почта организации</param>
		/// <param name="address">Адрес организации</param>
		public СlientORG(string orgName, string TIN, DateTime regDate,
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

		public СlientORG(IClientDTO newClient)
			: base(newClient.Telephone, newClient.Email, newClient.Address)
		{
			OrgName				= newClient.MainName;
			this.TIN			= newClient.PassportOrTIN;
			RegistrationDate	= (DateTime)newClient.CreationDate;
			DirectorFirstName	= newClient.FirstName;
			DirectorMiddleName	= newClient.MiddleName;
			DirectorLastName	= newClient.LastName;
		}

		#endregion
	}
}
