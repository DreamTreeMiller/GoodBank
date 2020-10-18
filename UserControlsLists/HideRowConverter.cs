using GoodBankNS.AccountClasses;
using GoodBankNS.DTO;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace GoodBankNS.UserControlsLists
{
	[ValueConversion(typeof(Object[]), typeof(Visibility))]
	public class HideRowConverter : IMultiValueConverter
	{
		public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value != null) 
			{
				// AccType == AccountType.Current && CurrentAccountsCB.IsChecked == True
				if ((value[0] as AccountDTO).EndDate == null &&
					(value[0] as AccountDTO).AccType == AccountType.Current)
					return (bool)value[1] ? Visibility.Visible : Visibility.Collapsed;

				// AccType == AccountType.Deposit && DepositCB.IsChecked == True
				if ((value[0] as AccountDTO).EndDate == null &&
					(value[0] as AccountDTO).AccType == AccountType.Deposit)
					return (bool)value[2] ? Visibility.Visible : Visibility.Collapsed;

				// AccType == AccountType.Credit && CreditCB.IsChecked == True
				if ((value[0] as AccountDTO).EndDate == null &&
					(value[0] as AccountDTO).AccType == AccountType.Credit)
					return (bool)value[3] ? Visibility.Visible : Visibility.Collapsed;

				if ((value[0] as AccountDTO).EndDate != null)
					return (bool)value[4] ? Visibility.Visible : Visibility.Collapsed;
			}
			return Visibility.Visible;

		}

		public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
