using GoodBankNS.BankInside;
using GoodBankNS.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
	/// Interaction logic for OpenDepositWindow.xaml
	/// </summary>
	public partial class OpenDepositWindow : Window, INotifyPropertyChanged
	{
		public double	startAmount = 0;
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

		public double   interest = 0.05;
		public string	Interest	
		{ 
			get => $"{(interest * 100):N2}"; 
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
				interest = tmp / 100;
			}
		}
		public DateTime Opened		{ get; }	  = GoodBank.Today;

		public int		duration = 12;
		public string Duration 
		{ 
			get => $"{duration}"; 
			set
			{
				if (!Int32.TryParse(value, out int tmp))
				{
					MessageBox.Show("Некорректрый ввод! Введите целое число больше 0");
					return;
				}
				if (tmp < 1)
				{
					MessageBox.Show("Число месяцев должно быть больше 0");
					return;
				}
				duration = tmp;
				NotifyPropertyChanged("EndDate");
			}
		}
		public DateTime EndDate 
		{ 
			get => Opened.AddMonths(duration);
		}
		public OpenDepositWindow(ObservableCollection<AccountDTO> accumulationAccounts)
		{
			InitializeComponent();
			AccumulationAccount.ItemsSource = accumulationAccounts;
			DataContext = this;
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		private void btnOk_OpenDeposit_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = true;
		}

		private void CompoundingCheckBox_Click(object sender, RoutedEventArgs e)
		{
			if ((bool)(sender as CheckBox).IsChecked)
			{
				AccumulationAccountLabel.Visibility = Visibility.Collapsed;
				AccumulationAccount.Visibility = Visibility.Collapsed;
			}
			else
			{
				AccumulationAccountLabel.Visibility = Visibility.Visible;
				AccumulationAccount.Visibility = Visibility.Visible;
			}
		}
	}
}
