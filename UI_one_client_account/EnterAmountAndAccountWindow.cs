using GoodBankNS.BankInside;
using GoodBankNS.Binding_UI_CondeBehind;
using GoodBankNS.Interfaces_Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
	/// Interaction logic for EnterAccountNumberWindow.xaml
	/// </summary>
	public partial class EnterAmountAndAccountWindow : Window
	{
		public double amount = 0;

		/// <summary>
		/// Проверяет, является ли введенная строка корректным числом с плав. запятой
		/// </summary>
		/// <param name="input">Введенная строка</param>
		/// <param name="tmp">Преобразованное значение. Если ввод некорректный, то значение неопределено</param>
		/// <returns>true/false. Если true, то в tmp результат преобразования</returns>
		private bool IsInputValid(string input, out double tmp)
		{
			if (String.IsNullOrEmpty(input))
			{
				MessageBox.Show("Введите число.");
				Dispatcher.BeginInvoke((ThreadStart)delegate
				{
					AmountEnterBox.Text = "0.00";
					AmountEnterBox.Focus();
					AmountEnterBox.SelectionStart = AmountEnterBox.Text.Length;
				});
				tmp = 0;
				return false;
			}

			if (!Double.TryParse(input, out tmp))
			{
				MessageBox.Show("Некорректрый ввод! Введите число.");
				Dispatcher.BeginInvoke((ThreadStart)delegate
				{
					AmountEnterBox.Text = "0.00";
					AmountEnterBox.Focus();
					AmountEnterBox.SelectionStart = AmountEnterBox.Text.Length;
				});

				return false;
			}
			if (tmp <= 0)
			{
				MessageBox.Show("Сумма перевода должна быть больше нуля");
				Dispatcher.BeginInvoke((ThreadStart)delegate
				{
					AmountEnterBox.Focus();
					AmountEnterBox.SelectionStart = AmountEnterBox.Text.Length;
				});

				return false;
			}
			return true;
		}

		public IAccount destinationAccount;

		ObservableCollection<IAccount> destinationAccountsList { get; set; }
		public EnterAmountAndAccountWindow(ObservableCollection<IAccount> destAccList)
		{
			InitializeComponent();
			DestinationAccount.ItemsSource = destAccList;
			DataContext = this;
			Dispatcher.BeginInvoke((ThreadStart)delegate
			{
				AmountEnterBox.Text = "0.00";
				AmountEnterBox.Focus();
				AmountEnterBox.SelectionStart = AmountEnterBox.Text.Length;
			});

		}

		private void btnOk_Click(object sender, RoutedEventArgs e)
		{
			if (DestinationAccount.SelectedItem == null)
			{
				MessageBox.Show("Выберите номер счета для перевода");
				return;
			}
			if (IsInputValid(AmountEnterBox.Text, out double tmp))
			{
				destinationAccount = DestinationAccount.SelectedItem as IAccount;
				amount = tmp;
				DialogResult = true;
			}
		}
	}
}
