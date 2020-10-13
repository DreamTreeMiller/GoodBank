using GoodBankNS.ClientClasses;
using GoodBankNS.Interfaces_Data;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace GoodBankNS.DTO
{
	/// <summary>
	/// Data Transfer Object для передачи данных при работе с клиентом 
	/// При показе или вводе данных о клиенте
	/// Ручной ввод осуществляется только в свойства с { get; set; }
	/// </summary>
	public class ClientDTO : IClientDTO, INotifyPropertyChanged
	{
		#region Свойства

		public uint			ID						{ get; }
		public ClientType	ClientType				{ get; set; }
		public string		ClientTypeTag			
		{
			get
			{
				string tmp = "";
				switch(ClientType)
				{
					case ClientType.VIP:
						tmp = "ВИП";
						break;
					case ClientType.Simple:
						tmp = "Физик";
						break;
					case ClientType.Organization:
						tmp = "Юрик";
						break;
				}
				return tmp;
			}
		}
		public string		FirstName
		{ 
			get => _firstName; 
			set
			{
				if (!IsFirstNameCorrect(value)) return;
				_firstName = value;
				NotifyPropertyChanged();
				NotifyMainNameOrDirName();
			}
		}

		public string		MiddleName
		{
			get => _middleName;
			set
			{
				_middleName = value;
				NotifyPropertyChanged();
				NotifyMainNameOrDirName();
			}
		}

		public string		LastName
		{
			get => _lastName;
			set
			{
				if (!IsLastNameCorrect(value)) return;
				_lastName = value;
				NotifyPropertyChanged();
				NotifyMainNameOrDirName();
			}
		}

		/// <summary>
		/// Содержит либо полноые ФИО, либо название организации
		/// в зависимости от типа клиента
		/// </summary>
		public string		MainName				
		{ 
			get
			{
				if (ClientType == ClientType.Organization) return _orgName;
				// Это надо для показа клиента
				return	LastName + " " + FirstName +
						(String.IsNullOrEmpty(MiddleName) ? "" : " ") +
						MiddleName;

			}
			set { _orgName = value; }
		}

		/// <summary>
		/// Поле для показа в списке
		/// </summary>
		public string		DirectorName			
		{ 
			get
			{
				if (ClientType != ClientType.Organization) return "";
				return	LastName + " " + FirstName +
						(String.IsNullOrEmpty(MiddleName) ? "" : " ") +
						MiddleName;
			}
		}

		public DateTime?	CreationDate
		{ 
			get => _creationDate; 
			set
			{
				if (!DateInTheRange(value)) return;
				_creationDate = value;
				NotifyPropertyChanged();
			}
		}

		public string		PassportOrTIN
		{ 
			get => _passportOrTIN; 
			set
			{
				if (ClientType == ClientType.Organization)
				{
					if (!ValidTIN(value)) return;
				}
				else
				{
					if (!ValidPassportNum(ref value)) return;
				}
				_passportOrTIN = value;
				NotifyPropertyChanged();
			}
		}

		public string		Telephone
		{ 
			get => _telephone; 
			set
			{
				_telephone = value;
				NotifyPropertyChanged();
			}
		}

		public string		Email
		{ 
			get => _email; 
			set
			{
				_email = value;
				NotifyPropertyChanged();
			}
		}
		public string		Address
		{ 
			get => _address; 
			set
			{
				_address = value;
				NotifyPropertyChanged();
			}
		}

		public int			NumberOfCurrentAccounts { get; } = 0;
		public int			NumberOfDeposits		{ get; } = 0;
		public int			NumberOfCredits			{ get; } = 0;
		public int			NumberOfClosedAccounts	{ get; } = 0;

		#endregion

		#region Поля

		private string		_firstName;
		private string		_middleName;
		private string		_lastName;
		private string		_orgName;
		private DateTime?	_creationDate = null;
		private string		_passportOrTIN;
		private string		_telephone;
		private string		_email;
		private string		_address;

		#endregion

		#region Конструкторы

		public ClientDTO() {}

		/// <summary>
		/// Конструктор для генерации ВИП или физика. 
		/// Подразумевает, что входные данные верны
		/// </summary>
		public ClientDTO(ClientType ct, string fn, string mn, string ln,
						 DateTime bd, string pNum,
						 string tel, string email, string address)
		{
			ClientType		= ct;
			_firstName		= fn;
			_middleName		= mn;
			_lastName		= ln;
			_creationDate	= bd;
			_passportOrTIN	= pNum;
			_telephone		= tel;
			_email			= email;
			_address		= address;
		}

		/// <summary>
		/// Конструктор для генерации Организации. 
		/// Подразумевает, что входные данные верны
		/// </summary>
		public ClientDTO(ClientType ct, string orgN, string dfn, string dmn, string dln,
						 DateTime rd, string tin,
						 string tel, string email, string address)
		{
			ClientType		= ct;
			_orgName		= orgN;
			_firstName		= dfn;
			_middleName		= dmn;
			_lastName		= dln;
			_creationDate	= rd;
			_passportOrTIN	= tin;
			_telephone		= tel;
			_email			= email;
			_address		= address;
		}

		/// <summary>
		/// Конструктор для выборки из базы и показа в списке
		/// Подразумевается, что все данные введены корректно
		/// </summary>
		/// <param name="c">Клиент из базы</param>
		public ClientDTO(IClient c)
		{
			ID						= c.ID;
			_telephone				= c.Telephone;
			_email					= c.Email;
			_address				= c.Address;
			NumberOfCurrentAccounts = c.NumberOfCurrentAccounts;
			NumberOfDeposits		= c.NumberOfDeposits;
			NumberOfCredits			= c.NumberOfCredits;
			NumberOfClosedAccounts  = c.NumberOfClosedAccounts;

			if (c is IClientVIP)
			{
				ClientType		= ClientType.VIP;
				_firstName		= (c as IClientVIP).FirstName;
				_middleName		= (c as IClientVIP).MiddleName;
				_lastName		= (c as IClientVIP).LastName;
				_passportOrTIN	= (c as IClientVIP).PasspostNumber;
				_creationDate	= (c as IClientVIP).BirthDate;
				return;
			}

			if (c is IClientSimple)
			{
				ClientType		= ClientType.Simple;
				_firstName		= (c as IClientSimple).FirstName;
				_middleName		= (c as IClientSimple).MiddleName;
				_lastName		= (c as IClientSimple).LastName;
				_passportOrTIN	= (c as IClientSimple).PasspostNumber;
				_creationDate	= (c as IClientSimple).BirthDate;
			}

			if (c is IClientOrg)
			{
				ClientType		= ClientType.Organization;
				_orgName		= (c as IClientOrg).OrgName;
				_firstName		= (c as IClientOrg).DirectorFirstName;
				_middleName		= (c as IClientOrg).DirectorMiddleName;
				_lastName		= (c as IClientOrg).DirectorLastName;
				_passportOrTIN	= (c as IClientOrg).TIN;
				_creationDate	= (c as IClientOrg).RegistrationDate;
			}
		}

		#endregion

		#region Валидаторы свойств для окошка ввода данных

		private bool IsFirstNameCorrect(string fn)
		{
			if (!String.IsNullOrEmpty(fn)) return true;
			MessageBox.Show("Имя не может быть пустым");
			return false;
		}

		private bool IsLastNameCorrect(string ln)
		{
			if (!String.IsNullOrEmpty(ln)) return true;
			MessageBox.Show("Фамилия не может быть пустой");
			return false;
		}

		private bool DateInTheRange(DateTime? date)
		{
			if (ClientType == ClientType.Organization)
			{
				if (date > DateTime.Now)
				{
					MessageBox.Show("Дата не может превосходить сегодняшний день");
					return false;
				}
				return true;
			}
			if ((DateTime.Now - (DateTime)date).TotalDays / 365.25 < 18)
			{
				MessageBox.Show("Только лица, достигшие 18 лет, могут быть клиентами банка.");
				return false;
			}
			if ((DateTime.Now - (DateTime)date).TotalDays / 365.25 > 118)
			{
				MessageBox.Show("Сейчас на земле нет людей, которым больше 118 лет.");
				return false;
			}
			return true;
		}

		/// <summary>
		/// Проверка валидности ИНН
		/// </summary>
		/// <param name="tin">ИНН</param>
		/// <returns></returns>
		private bool ValidTIN(string tin)
		{
			int part;
			string errorMessage = "		Неверный формат ИНН\n" +
									"\n" +
									"	ррННхххххС\n" +
									"	рр - код региона России от 1 до 85\n" +
									"	НН - номер от 1 до 99 налоговой в регионе\n" +
									"	ххххх - код (от 1) организации\n" +
									"	С - контрольная цифра";
			tin = tin.Trim();
			if (tin.Length == 10)
				if (Int32.TryParse(tin.Substring(0, 2), out part))
					if (0 < part && part <= 85)
						if (Int32.TryParse(tin.Substring(2, 2), out part))
							if (0 < part)
								if (Int32.TryParse(tin.Substring(4, 6), out part))
									if (0 < part)
										return true;
			MessageBox.Show(errorMessage);
			return false;
		}

		/// <summary>
		/// Проверка валидности номера паспорта
		/// </summary>
		/// <param name="pNum">Номер паспорта в формате СССС ХХХХХХ</param>
		/// <returns></returns>
		private bool ValidPassportNum(ref string pNum)
		{
			int series, number;
			string errorMessage = "          Неверный формат номера паспорта!\n" +
									"           Используйте формат CCCC ХХХХХХ\n" +
									"    CCCC   - 4 цифры серии, первая не может быть 0\n" +
									"    ХХХХХХ - 6 цифр номера, первая не может быть 0\n" +
									"Количество пробелов до, между и после групп цифр может быть любым";
			pNum = pNum.Replace(" ", "");
			if (pNum.Length == 10)
				if (Int32.TryParse(pNum.Substring(0, 4), out series))
					if (0 < series)
						if (Int32.TryParse(pNum.Substring(4), out number))
							if (0 < number && number < 1_000_000)
							{
								pNum = $"{series:0000} {number:000000}";
								return true;
							}
			MessageBox.Show(errorMessage);
			return false;
		}

		#endregion

		#region Обработчики изменения свойств

		public event PropertyChangedEventHandler PropertyChanged;

		// This method is called by the Set accessor of each property.  
		// The CallerMemberName attribute that is applied to the optional propertyName  
		// parameter causes the property name of the caller to be substituted as an argument.  
		private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		private void NotifyMainNameOrDirName()
		{
			if (ClientType == ClientType.Organization)
				NotifyPropertyChanged("DirectorName");
			else
				NotifyPropertyChanged("MainName");
		}

		#endregion

	}
}
