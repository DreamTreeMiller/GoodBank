using GoodBank.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace GoodBank.UI_clients
{
	/// <summary>
	/// Interaction logic for VIPclientsWindow.xaml
	/// </summary>
	public partial class VIPclientsWindow : Window
	{
		private ObservableCollection<ClientDTO>  vipClients  { get; set; }
		private ObservableCollection<AccountDTO> vipAccoutns { get; set; }
		public VIPclientsWindow()
		{
			InitializeComponent();

			ShowVIPClients();
			ShowVIPAccounts();
		}

		private void ShowVIPClients()
		{
			VIPclientsDataGrid.ItemsSource = vipClients;
		}

		private void ShowVIPAccounts()
		{
			VIPaccountsDataGrid.ItemsSource = vipAccoutns;
		}

	}
}
