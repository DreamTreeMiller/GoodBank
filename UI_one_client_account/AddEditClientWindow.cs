using GoodBankNS.DTO;
using GoodBankNS.UserControlsLists;
using GoodBankNS.ClientClasses;
using GoodBankNS.Interfaces_Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Threading;

namespace GoodBankNS.UI_one_client_account
{
	public partial class AddEditClientWindow : Window
	{
		public ClientDTO client = null;
		public AddEditClientWindow(AddEditClientNameTags nameTags, ClientDTO client)
		{
			InitializeComponent();
			InitializeTextFields(nameTags, client);
		}

		private void InitializeTextFields(AddEditClientNameTags nameTags, ClientDTO client)
		{
			Title		= nameTags.SystemWindowTitle;
			Header.Text = nameTags.WindowHeader;

			// Если окошко вызвали для создания нового клиента
			// а это происходит тогда, когда клиент на входе равен null
			// То в болванку ДТО надо поместить тип создаваемого клиента
			if (client == null)
			{
				this.client				 = new ClientDTO();
				this.client.ClientType	 = nameTags.ClientType;
			}
			else 
				this.client				= client;
			
			DataContext	= this.client;
		}
		private void btnOk_AddClient_Click(object sender, RoutedEventArgs e)
		{
			if (client.ClientType == ClientType.Organization)
			{
				if (!IsOrgNameEntered())			return;
				if (!IsTINEntered())				return;
				if (!IsRegistrationDateEntered())	return;
			}
			else
			{
				if (!IsFirstLastNamesEntered())		return;
				if (!IsPassportNumEntered())		return;
				if (!IsBirthDateEntered())			return;
			}
			
			DialogResult = true;
		}

		#region Проверка заполненности полей

		private bool IsOrgNameEntered()
		{
			if (String.IsNullOrEmpty(client.MainName))
			{
				MessageBox.Show("Введите название организации");
				return false;
			}
			return true;
		}

		private bool IsTINEntered()
		{
			if (String.IsNullOrEmpty(client.PassportOrTIN))
			{
				MessageBox.Show("Введите ИНН");
				// код возвращения фокуса в только что покинутое поле
				Dispatcher.BeginInvoke((ThreadStart)delegate
				{
					PassportOrTINEntryBox.Focus();
					PassportOrTINEntryBox.SelectionStart = PassportOrTINEntryBox.Text.Length;
				});
				return false;
			}
			return true;
		}

		private bool IsRegistrationDateEntered()
		{
			if (client.CreationDate == null)
			{
				MessageBox.Show("Введите дату регистрации организации");
				// код возвращения фокуса в только что покинутое поле
				Dispatcher.BeginInvoke((ThreadStart)delegate
				{
					CreationDateEntryBox.Focus();
				});
				return false;
			}
			return true;
		}

		private bool IsFirstLastNamesEntered()
		{
			if (String.IsNullOrEmpty(client.FirstName))
			{
				MessageBox.Show("Имя клиента не должно быть пустым!\n" +
								"     Введите имя клиента.");
				// код возвращения фокуса в только что покинутое поле
				Dispatcher.BeginInvoke((ThreadStart)delegate
				{
					FirstNameEntryBox.Focus();
					FirstNameEntryBox.SelectionStart = FirstNameEntryBox.Text.Length;
				});
				return false;
			}
			if (String.IsNullOrEmpty(client.LastName))
			{
				MessageBox.Show("Фамилия клиента не должна быть пустой!\n" +
								"     Введите фамилию клиента.");
				// код возвращения фокуса в только что покинутое поле
				Dispatcher.BeginInvoke((ThreadStart)delegate
				{
					LastNameEntryBox.Focus();
					LastNameEntryBox.SelectionStart = LastNameEntryBox.Text.Length;
				});
				return false;
			}
			return true;
		}
	
		private bool IsPassportNumEntered()
		{
			if (String.IsNullOrEmpty(client.PassportOrTIN))
			{
				MessageBox.Show("Введите номер паспорта");
				// код возвращения фокуса в только что покинутое поле
				Dispatcher.BeginInvoke((ThreadStart)delegate
				{
					PassportOrTINEntryBox.Focus();
					PassportOrTINEntryBox.SelectionStart = PassportOrTINEntryBox.Text.Length;
				});
				return false;
			}
			return true;
		}

		private bool IsBirthDateEntered()
		{
			if (client.CreationDate == null)
			{
				MessageBox.Show("Введите дату рождения клиента");
				// код возвращения фокуса в только что покинутое поле
				Dispatcher.BeginInvoke((ThreadStart)delegate
				{
					CreationDateEntryBox.Focus();
				});
				return false;
			}
			return true;
		}

		#endregion

		/*

		 */
	}
}
