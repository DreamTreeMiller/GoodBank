﻿using Interfaces_Data;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClientClasses
{
	[Table("ClientsVIP")]
	public class ClientVIP : Client
	{
		#region ФИО, № паспорта, дата рождения

		public string	FirstName		{ get; set; }
		public string	MiddleName		{ get; set; }
		public string	LastName		{ get; set; }
		public string	PassportNumber	{ get; set; }
		public DateTime	BirthDate		{ get; set; }

		#endregion

		#region Конструкторы

		/// <summary>
		/// Конструктор для корректной работы Entity Framework
		/// </summary>
		public ClientVIP() { }

		public ClientVIP(IClientDTO newClient)
			: base(newClient.Telephone, newClient.Email, newClient.Address)
		{
			FirstName		= newClient.FirstName;
			MiddleName		= newClient.MiddleName;
			LastName		= newClient.LastName;
			PassportNumber	= newClient.PassportOrTIN;
			BirthDate		= (DateTime)newClient.CreationDate;
		}

		public override void UpdateMyself(IClientDTO updatedClient)
		{
			FirstName		= updatedClient.FirstName;
			MiddleName		= updatedClient.MiddleName;
			LastName		= updatedClient.LastName;
			PassportNumber	= updatedClient.PassportOrTIN;
			BirthDate		= (DateTime)updatedClient.CreationDate;
			Telephone		= updatedClient.Telephone;
			Email			= updatedClient.Email;
			Address			= updatedClient.Address;
		}

		#endregion
	}
}
