using GoodBankNS.Interfaces_Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodBankNS.ClientClasses
{
	public class ClientVIP : Client, IClientVIP
	{
		#region ФИО, № паспорта, дата рождения

		public string	FirstName		{ get; set; }
		public string	MiddleName		{ get; set; }
		public string	LastName		{ get; set; }
		public string	PasspostNumber	{ get; set; }
		public DateTime	BirthDate		{ get; set; }

		#endregion

		#region Конструктор

		/// <summary>
		/// Конструктор созадет ВИП клиента
		/// </summary>
		/// <param name="fn">Имя</param>
		/// <param name="mn">Отчество</param>
		/// <param name="ln">Фамилия</param>
		/// <param name="passNum">Номер паспорта</param>
		/// <param name="bd">Дата рождения</param>
		/// <param name="tel">Телефон</param>
		/// <param name="email">Электронная почта</param>
		/// <param name="address">Адрес</param>

		public ClientVIP(string fn, string mn, string ln, string passNum, DateTime bd,
						  string tel, string email, string address)
			: base(tel, email, address)
		{
		}

		public ClientVIP(IClientDTO newClient)
			: base(newClient.Telephone, newClient.Email, newClient.Address)
		{
			FirstName		= newClient.FirstName;
			MiddleName		= newClient.MiddleName;
			LastName		= newClient.LastName;
			PasspostNumber	= newClient.PassportOrTIN;
			BirthDate		= newClient.CreationDate;
		}

		#endregion
	}
}
