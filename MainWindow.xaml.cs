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
using GoodBankNS.UserControlsLists;

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
			DepartmentWindow vipClientsWin = new DepartmentWindow(WindowID.DepartmentVIP, BA);
			vipClientsWin.ShowDialog();
		}

		private void SimpleClientsDeptButton_Click(object sender, RoutedEventArgs e)
		{
			DepartmentWindow simpleClientsWin = new DepartmentWindow(WindowID.DepartmentSIM, BA);
			simpleClientsWin.ShowDialog();
		}

		private void OrgClientsDeptButton_Click(object sender, RoutedEventArgs e)
		{
			DepartmentWindow orgClientsWin = new DepartmentWindow(WindowID.DepartmentORG, BA);
			orgClientsWin.ShowDialog();
		}

		private void BankManagerButton_Click(object sender, RoutedEventArgs e)
		{
			DepartmentWindow allClientsWin = new DepartmentWindow(WindowID.DepartmentALL, BA);
			allClientsWin.ShowDialog();
		}

		private void SearchButton_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("Функция поиска в разработке");
		}

		private void TimeMachineButton_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("Прокрутить время на месяц вперёд");
		}

		private void GenerateButton_Click(object sender, RoutedEventArgs e)
		{
			Generate.Bank(BA, 1_000_000, 100, 40);
			MessageBox.Show("Клиенты и счета созданы!");
		}
	}
}
