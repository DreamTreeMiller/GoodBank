using GoodBankNS.DTO;
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
	/// <summary>
	/// Interaction logic for AddVIPClientWindow.xaml
	/// </summary>
	public partial class AddClientWindow : Window
	{
		public ClientDTO newClientData = new ClientDTO();
		public AddClientWindow()
		{
			InitializeComponent();
			InitializeTextFields();
		}

		private void InitializeTextFields()
		{
			Title		= "Добавить важную персону";
			Header.Text = "ДАННЫЕ ВАЖНОЙ ПЕРСОНЫ";
			newClientData.CreationDate	= DateTime.Now;
			DataContext					= newClientData;
		}
		private void btnOk_AddClient_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = true;
		}
	}
}
