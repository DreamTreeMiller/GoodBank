using GoodBankNS.AccountClasses;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace GoodBankNS.UserControlsLists
{
	[ValueConversion(typeof(AccountType), typeof(bool))]
	public class HideRowConverter : IValueConverter
	{
		public static HideRowConverter Instance = new HideRowConverter();

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if ((AccountType)value == AccountType.Current /*&& CurrentAccountsCB.IsChecked == false*/)
			{
				return true;
			}
			else
			{
				return false;
			}

		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
