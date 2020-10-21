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
	/// Interaction logic for EnterTopUpCashAmountWindow.xaml
	/// </summary>
	public partial class EnterTopUpCashAmountWindow : Window
	{
		public double amount = 0;
		public string Amount
		{
			get => $"{amount:N2}";
			set
			{
				if (!Double.TryParse(value, out double tmp))
				{
					MessageBox.Show("Некорректрый ввод! Введите число.");
					return;
				}
				if (tmp < 0)
				{
					MessageBox.Show("Число не должно быть отрицательным");
					return;
				}
				amount = tmp;
			}
		}
		public EnterTopUpCashAmountWindow()
		{
			InitializeComponent();
			DataContext = this;
		}

		private void btnOk_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = true;
		}
	}
}
