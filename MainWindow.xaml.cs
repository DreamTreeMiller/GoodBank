using GoodBank.Binding_UI_CondeBehind;
using GoodBank.Interfaces_Data;
using GoodBank.UI_clients;
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

namespace GoodBank
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private IGoodBank GoodBank;
		private ActionsUI UI;
		public MainWindow()
		{
			InitializeComponent();
		}

		private void VipClientsDeptButton_Click(object sender, RoutedEventArgs e)
		{
			VIPclientsWindow vipClientsWin = new VIPclientsWindow();
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
	}
}
