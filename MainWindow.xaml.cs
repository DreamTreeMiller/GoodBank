using GoodBankNS.Binding_UI_CondeBehind;
using GoodBankNS.Imitation;
using GoodBankNS.Interfaces_Data;
using GoodBankNS.UI_clients;
using GoodBankNS.BankInside;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GoodBankNS
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private IGoodBank GoodBank;
		private BankActions BA;
		public MainWindow()
		{
			InitializeComponent();
			InitializeBank();
			InitializeUI();
		}

		private void InitializeBank()
		{
			GoodBank = new GoodBank();
		}

		private void InitializeUI()
		{
			BA = new BankActions(GoodBank);
		}

		private void VipClientsDeptButton_Click(object sender, RoutedEventArgs e)
		{
			VIPclientsWindow vipClientsWin = new VIPclientsWindow(BA);
			vipClientsWin.ShowDialog();
		}

		private void SimpleClientsDeptButton_Click(object sender, RoutedEventArgs e)
		{

		}

		private void OrgClientsDeptButton_Click(object sender, RoutedEventArgs e)
		{

		}

		private void BankManagerButton_Click(object sender, RoutedEventArgs e)
		{

		}

		private void SearchButton_Click(object sender, RoutedEventArgs e)
		{

		}

		private void TimeMachineButton_Click(object sender, RoutedEventArgs e)
		{

		}

		private void GenerateButton_Click(object sender, RoutedEventArgs e)
		{
			GoodBank = Generate.Bank(20, 100, 40);
			MessageBox.Show("Клиенты и счета созданы!");
		}
	}
}
