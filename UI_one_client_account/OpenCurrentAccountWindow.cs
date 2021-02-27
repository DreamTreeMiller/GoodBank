using System;
using System.Threading;
using System.Windows;
using BankInside;
using Binding_UI_CondeBehind;

namespace UI_one_client_account
{
	/// <summary>
	/// Interaction logic for OpenAccountWindow.xaml
	/// </summary>
	public partial class OpenCurrentAccountWindow : Window
	{
		public double startAmount = 0;

		public DateTime	Opened { get; }

		private readonly BankActions BA;

		public OpenCurrentAccountWindow(BankActions ba)
		{
			InitializeComponent();
			BA = ba;
			BankTodayDate.Text = $"Сегодня {BA.GBDateTime.Today():dd.MM.yyyy} г.";
			Opened = BA.GBDateTime.Today();
			DataContext = this;
			Dispatcher.BeginInvoke((ThreadStart)delegate
			{
				StartAmountEntryBox.Text = "0.00";
				StartAmountEntryBox.Focus();
				StartAmountEntryBox.SelectionStart = StartAmountEntryBox.Text.Length;
			});
		}

		/// <summary>
		/// Проверяет, является ли введенная строка корректным числом с плав. запятой
		/// </summary>
		/// <param name="input">Введенная строка</param>
		/// <param name="tmp">Преобразованное значение. Если ввод некорректный, то значение неопределено</param>
		/// <returns>true/false. Если true, то в tmp результат преобразования</returns>
		private bool IsInputValid(string input, out double tmp)
		{
			if(String.IsNullOrEmpty(input))
			{
				MessageBox.Show("Введите число.");
				Dispatcher.BeginInvoke((ThreadStart)delegate
				{
					StartAmountEntryBox.Focus();
					StartAmountEntryBox.SelectionStart = StartAmountEntryBox.Text.Length;
				});
				tmp = 0;
				return false;
			}
			if (!Double.TryParse(input, out tmp))
			{
				MessageBox.Show("Некорректрый ввод! Введите число.");
				Dispatcher.BeginInvoke((ThreadStart)delegate
				{
					StartAmountEntryBox.Text = "0.00";
					StartAmountEntryBox.Focus();
					StartAmountEntryBox.SelectionStart = StartAmountEntryBox.Text.Length;
				});
				return false;
			}
			if (tmp < 0)
			{
				MessageBox.Show("Число не должно быть отрицательным");
				Dispatcher.BeginInvoke((ThreadStart)delegate
				{
					StartAmountEntryBox.Focus();
					StartAmountEntryBox.SelectionStart = StartAmountEntryBox.Text.Length;
				});
				return false;
			}
			return true;
		}

		private void btnOk_OpenCurrentAccount_Click(object sender, RoutedEventArgs e)
		{
			if (IsInputValid(StartAmountEntryBox.Text, out double tmp))
			{
				startAmount  = tmp;
				DialogResult = true;
			}
			else
				Dispatcher.BeginInvoke((ThreadStart)delegate
				{
					StartAmountEntryBox.Text = "0.00";
					StartAmountEntryBox.Focus();
					StartAmountEntryBox.SelectionStart = StartAmountEntryBox.Text.Length;
				});
		}
	}
}
