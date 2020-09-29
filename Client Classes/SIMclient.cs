using GoodBank.Interfaces_Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodBank.ClientClasses
{
	public class SIMclient : Client, ISimpleClient
	{
		#region Перегрузка абстрактных свойств

		public override string	 DisplayName	=>
			FirstName + " " +
			MiddleName + (String.IsNullOrEmpty(MiddleName) ? "" : " ") +
			LastName;

		public override string	 IdNumber		=> PasspostNumber;

		public override DateTime CreationDate	=> BirthDate;

		#endregion

		#region ФИО, № паспорта, дата рождения

		public string	FirstName		{ get; set; }
		public string	MiddleName		{ get; set; } = "";
		public string	LastName		{ get; set; }
		public string	PasspostNumber  { get; set; }
		public DateTime BirthDate		{ get; set; }

		#endregion

		#region Конструктор

		public SIMclient(string fn,  string mn,    string ln, string passNum, DateTime bd,
						 string tel, string email, string address)
			: base(tel, email, address)
		{
			FirstName		= fn;
			MiddleName		= mn;
			LastName		= ln;
			PasspostNumber  = passNum;
			BirthDate		= bd;
		}

		#endregion
	}
}
