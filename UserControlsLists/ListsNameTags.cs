using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodBankNS.UserControlsLists
{
	/// <summary>
	/// Структура для передачи текста названий полей списка счетов в зависимости от окна
	/// </summary>
	public struct AccountsViewNameTags
	{
	}

	/// <summary>
	/// Структура для передачи текста полей списка клиентов в зависимости от окна и типа клиентов
	/// </summary>
	public struct ClientsViewNameTags
	{
		public string CreationDateCBTag;		// Дата рождения или дата регистрации
		public string PassportOrTIN_CB_Tag;		// Номер паспорта или ИНН
		public bool   ClientTypeColumn;			// Показывать колонку тип - физик или юрик
		public string MainNameTag;				// ФИО или Название организации
		public string ClientNameTag;			// Сводка: ВИП клиентов, физиков, юриков
	}
}
