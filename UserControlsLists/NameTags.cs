using GoodBankNS.ClientClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GoodBankNS.UserControlsLists
{
	/// <summary>
	/// Номер окошка. В зависимости от номера, разное наполнение
	/// </summary>
	public enum WindowID
	{
		DepartmentVIP,
		DepartmentSIM,
		DepartmentORG,
		DepartmentALL,
		ClientVIP,
		ClientSIM,
		ClientORG,
		Account
	}

	public class WindowNameTags
	{
		public string SystemWindowTitle;
		public string WindowHeader;
		public string SelectClientTag;
		public string AddClientTag;

		public WindowNameTags(WindowID wid)
		{
			switch(wid)
			{
				case WindowID.DepartmentVIP:
					SystemWindowTitle = "Очень важные персоны";
					WindowHeader = "ОЧЕНЬ ВАЖНЫЕ ПЕРСОНЫ";
					SelectClientTag = "Показать персону";
					AddClientTag = "Добавить персону";
					break;
				case WindowID.DepartmentSIM:
					SystemWindowTitle = "Физики";
					WindowHeader = "ФИЗИКИ";
					SelectClientTag = "Показать физика";
					AddClientTag = "Добавить физика";
					break;
				case WindowID.DepartmentORG:
					SystemWindowTitle = "Юрики";
					WindowHeader = "ЮРИКИ";
					SelectClientTag = "Показать юрика";
					AddClientTag = "Добавить юрика";
					break;
				case WindowID.DepartmentALL:
					SystemWindowTitle = "Управляющий банком";
					WindowHeader = "ВСЕ, ВСЕ, ВСЕ";
					SelectClientTag = "Показать клиента";
					AddClientTag = "Добавить клиента";
					break;
			}
		}
	}
	/// <summary>
	/// Структура для передачи текста полей списка клиентов в зависимости от окна и типа клиентов
	/// </summary>
	public class ClientsViewNameTags
	{
		public string CreationDateCBTag;        // Дата рождения или дата регистрации
		public string PassportOrTIN_CB_Tag;     // Номер паспорта или ИНН
		public Visibility ShowDirectorCB;       // Показывать checkbox директор
		public Visibility   ShowClientTypeColumn;     // Показывать колонку тип - физик или юрик
		public string MainNameTag;              // ФИО или Название организации
		public string TotalNameTag;             // Сводка: ВИП клиентов, физиков, юриков

		public ClientsViewNameTags(WindowID wid)
		{
			switch(wid)
			{
				case WindowID.DepartmentVIP:
					CreationDateCBTag	 = "Дата рождения";
					PassportOrTIN_CB_Tag = "Паспорт";
					ShowClientTypeColumn = Visibility.Collapsed;
					ShowDirectorCB	     = Visibility.Collapsed;
					MainNameTag			 = "ФИО";
					TotalNameTag		 = "ВИП клиентов:";
					break;
				case WindowID.DepartmentSIM:
					CreationDateCBTag = "Дата рождения";
					PassportOrTIN_CB_Tag = "Паспорт";
					ShowClientTypeColumn = Visibility.Collapsed;
					ShowDirectorCB = Visibility.Collapsed;
					MainNameTag = "ФИО";
					TotalNameTag = "физиков:";
					break;
				case WindowID.DepartmentORG:
					CreationDateCBTag = "Дата регистрации";
					PassportOrTIN_CB_Tag = "ИНН";
					ShowClientTypeColumn = Visibility.Collapsed;
					ShowDirectorCB = Visibility.Visible;
					MainNameTag = "Название";
					TotalNameTag = "юриков:";
					break;
				case WindowID.DepartmentALL:
					CreationDateCBTag = "Дата рожд./рег.";
					PassportOrTIN_CB_Tag = "Паспорт/ИНН";
					ShowClientTypeColumn = Visibility.Visible;
					ShowDirectorCB = Visibility.Visible;
					MainNameTag = "ФИО / Название";
					TotalNameTag = "клиентов:";
					break;
			}
		}
	}

	/// <summary>
	/// Структура для передачи текста названий полей списка счетов в зависимости от окна
	/// </summary>
	public class AccountsViewNameTags
	{
	}

}
