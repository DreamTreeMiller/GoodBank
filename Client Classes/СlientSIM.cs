using GoodBank.Interfaces_Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodBank.ClientClasses
{
	public class СlientSIM : Client, IClientSimple
	{
		#region ФИО, № паспорта, дата рождения

		public string	FirstName		{ get; set; }
		public string	MiddleName		{ get; set; } = "";
		public string	LastName		{ get; set; }
		public string	PasspostNumber  { get; set; }
		public DateTime BirthDate		{ get; set; }

		#endregion

		#region Конструктор

		/// <summary>
		/// Конструктор созадет клиента
		/// </summary>
		/// <param name="fn">Имя</param>
		/// <param name="mn">Отчество</param>
		/// <param name="ln">Фамилия</param>
		/// <param name="passNum">Номер паспорта</param>
		/// <param name="bd">Дата рождения</param>
		/// <param name="tel">Телефон</param>
		/// <param name="email">Электронная почта</param>
		/// <param name="address">Адрес</param>
		public СlientSIM(string fn,  string mn,    string ln, string passNum, DateTime bd,
						 string tel, string email, string address)
			: base(tel, email, address)
		{
			FirstName		= fn;
			MiddleName		= mn;
			LastName		= ln;
			PasspostNumber  = passNum;
			BirthDate		= bd;
		}

		public СlientSIM(IClientDTO newClient)
			: base(newClient.Telephone, newClient.Email, newClient.Address)
		{

		}

		#endregion
	}
}
