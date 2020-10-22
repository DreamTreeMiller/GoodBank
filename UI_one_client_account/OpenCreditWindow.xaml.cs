using GoodBankNS.BankInside;
using GoodBankNS.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
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
	/// Interaction logic for OpenCreditWindow.xaml
	/// </summary>
	public partial class OpenCreditWindow : Window, INotifyPropertyChanged
	{
		public double creditAmount = 0;
		public string CreditAmount
		{
			get => $"{creditAmount:N2}";
			set
			{
				if (!Double.TryParse(value, out double tmp))
				{
					MessageBox.Show("Некорректрый ввод! Введите число.");
					SetFocusOnCreditAmountEntryBox();
					return;
				}
				if (tmp < 0)
				{
					MessageBox.Show("Число не должно быть отрицательным");
					SetFocusOnCreditAmountEntryBox();
					return;
				}
				creditAmount = tmp;
			}
		}

		public double interest = 0.05;
		public string Interest
		{
			get => $"{(interest * 100):N2}";
			set
			{
				if (!Double.TryParse(value, out double tmp))
				{
					MessageBox.Show("Некорректрый ввод! Введите число.");
					SetFocusOnInterestEntryBox();
					return;
				}
				if (tmp < 0)
				{
					MessageBox.Show("Число не должно быть отрицательным");
					SetFocusOnInterestEntryBox();
					return;
				}
				interest = tmp / 100;
			}
		}
		public DateTime Opened { get; } = GoodBank.Today;

		public int duration = 12;
		public string Duration
		{
			get => $"{duration}";
			set
			{
				if (!Int32.TryParse(value, out int tmp))
				{
					MessageBox.Show("Некорректрый ввод! Введите целое число больше 0");
					SetFocusOnDurationEntryBox();
					return;
				}
				if (tmp < 1)
				{
					MessageBox.Show("Число месяцев должно быть больше 0");
					SetFocusOnDurationEntryBox();
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
		public OpenCreditWindow(ObservableCollection<AccountDTO> creditRecipientAccounts)
		{
			InitializeComponent();
			CreditRecipientAccount.ItemsSource = creditRecipientAccounts;
			DataContext = this;

			SetFocusOnCreditAmountEntryBox();
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		private void btnOk_OpenCredit_Click(object sender, RoutedEventArgs e)
		{
			if (creditAmount == 0)
			{
				MessageBox.Show("Сумма кредита должна быть больше нуля");
				SetFocusOnCreditAmountEntryBox();
				return;
			}

			if (duration == 0)
			{
				MessageBox.Show("Число месяцев должно быть больше 0");
				SetFocusOnDurationEntryBox();
				return;
			}

			DialogResult = true;
		}

		private void SetFocusOnCreditAmountEntryBox()
		{
			Dispatcher.BeginInvoke((ThreadStart)delegate
			{
				CreditAmountEntryBox.Focus();
				CreditAmountEntryBox.SelectionStart = CreditAmountEntryBox.Text.Length;
			});
		}

		private void SetFocusOnInterestEntryBox()
		{
			Dispatcher.BeginInvoke((ThreadStart)delegate
			{
				InterestEntryBox.Focus();
				InterestEntryBox.SelectionStart = InterestEntryBox.Text.Length;
			});
		}

		private void SetFocusOnDurationEntryBox()
		{
			Dispatcher.BeginInvoke((ThreadStart)delegate
			{
				DurationEntryBox.Focus();
				DurationEntryBox.SelectionStart = DurationEntryBox.Text.Length;
			});
		}
	}
}
