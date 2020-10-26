using GoodBankNS.BankInside;
using GoodBankNS.ClientClasses;
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
		public	double		creditAmount = 0;
		public	string		CreditAmount
		{
			get => $"{creditAmount:N2}";
			set
			{
				if (!IsDoubleValid(value, out double tmp))
				{
					SetFocusOnCreditAmountEntryBox();
					return;
				}
				creditAmount = tmp;
			}
		}

		private double minInterest, maxInterest;
		public	double		interest = 0.05;
		public	string		Interest
		{
			get => $"{(interest * 100):N2}";
			set
			{
				if (!IsInterestValid(value, out double tmp))
				{
					SetFocusOnInterestEntryBox();
					return;
				}
				interest = tmp / 100;
			}
		}
		public	DateTime	Opened { get; } = GoodBank.Today;

		public	int			duration = 12;
		public	string		Duration
		{
			get => $"{duration}";
			set
			{
				if (!IsDurationValid(value, out int tmp))
				{
					SetFocusOnDurationEntryBox();
					return;
				}
				duration = tmp;
				NotifyPropertyChanged("EndDate");
			}
		}

		public	DateTime	EndDate
		{
			get => Opened.AddMonths(duration);
		}
		public event PropertyChangedEventHandler PropertyChanged;

		private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		/// <summary>
		/// Проверяет, является ли введенная строка корректным числом с плав. запятой
		/// </summary>
		/// <param name="input">Введенная строка</param>
		/// <param name="tmp">Преобразованное значение. Если ввод некорректный, то значение неопределено</param>
		/// <returns>true/false. Если true, то в tmp результат преобразования</returns>
		private bool IsDoubleValid(string input, out double tmp)
		{
			if (String.IsNullOrEmpty(input))
			{
				MessageBox.Show("Введите число.");
				tmp = 0;
				return false;
			}
			if (!Double.TryParse(input, out tmp))
			{
				MessageBox.Show("Некорректрый ввод! Введите число.");
				return false;
			}
			if (tmp < 0)
			{
				MessageBox.Show("Число не должно быть отрицательным");
				return false;
			}
			return true;
		}

		private bool IsInterestValid(string input, out double tmp)
		{
			if (String.IsNullOrEmpty(input))
			{
				MessageBox.Show("Введите число.");
				tmp = 0;
				return false;
			}
			if (!Double.TryParse(input, out tmp))
			{
				MessageBox.Show("Некорректрый ввод! Введите число.");
				return false;
			}
			if (tmp < minInterest || maxInterest < tmp)
			{
				MessageBox.Show($"Процент должен быть между {minInterest:N2} ~ {maxInterest:N2} %");
				return false;
			}
			return true;
		}

		private bool IsDurationValid(string input, out int tmp)
		{
			if (String.IsNullOrEmpty(input))
			{
				MessageBox.Show("Введите число.");
				tmp = 0;
				return false;
			}
			if (!Int32.TryParse(input, out tmp))
			{
				MessageBox.Show("Некорректрый ввод! Введите число.");
				return false;
			}
			if (tmp < 1)
			{
				MessageBox.Show("Число месяцев должно быть больше 0");
				return false;
			}
			return true;
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

		public OpenCreditWindow(ObservableCollection<AccountDTO> creditRecipientAccounts, ClientType clientType)
		{
			InitializeComponent();
			InitializeWindowLabelsAndData(creditRecipientAccounts, clientType);

			SetFocusOnCreditAmountEntryBox();
		}

		private void InitializeWindowLabelsAndData(ObservableCollection<AccountDTO> creditRecipientAccounts, ClientType clientType)
		{
			BankTodayDate.Text = $"Сегодня {GoodBankNS.BankInside.GoodBank.Today:dd.MM.yyyy} г.";
			CreditRecipientAccount.ItemsSource = creditRecipientAccounts;

			switch (clientType)
			{
				case ClientType.VIP:
					InterestLabel.Text = "Процент (7 ~ 12 %)";
					minInterest = 7;
					maxInterest = 12;
					interest	= 0.12;
					break;
				case ClientType.Simple:
					InterestLabel.Text = "Процент (12 ~ 20 %)";
					minInterest = 12;
					maxInterest = 20;
					interest	= 0.20;
					break;
				case ClientType.Organization:
					InterestLabel.Text = "Процент (15 ~ 25 %)";
					minInterest = 15;
					maxInterest = 25;
					interest	= 0.25;
					break;
			}
			DataContext = this;
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

	}
}
