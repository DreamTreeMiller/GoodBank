using GoodBankNS.BankInside;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
	/// Interaction logic for OpenAccountWindow.xaml
	/// </summary>
	public partial class OpenCurrentAccountWindow : Window, INotifyPropertyChanged
	{
		public double startAmount = 0;
		public string	StartAmount
		{
			get => $"{startAmount:N2}";
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
				startAmount = tmp;
			}
		}

		public DateTime	Opened		{ get; }	  = GoodBank.Today;
		public OpenCurrentAccountWindow()
		{
			InitializeComponent();
			DataContext = this;
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		private void btnOk_OpenCurrentAccount_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = true;
		}
	}
}
