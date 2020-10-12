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
				this.client				= new ClientDTO();
				this.client.ClientType	= nameTags.ClientType;
			}
			else 
				this.client				= client;
			
			DataContext	= this.client;
		}
		private void btnOk_AddClient_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = true;
		}
	}
}
