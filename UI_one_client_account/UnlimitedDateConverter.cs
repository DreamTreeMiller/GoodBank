using GoodBankNS.AccountClasses;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace GoodBankNS.UI_one_client_account
{
	[ValueConversion(typeof(DateTime?), typeof(string))]
	public class UnlimitedDateConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null) return "бессрочный";
			return $"{(DateTime)value:dd.MM.yyyy}";
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	public class StillOpenConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null) return "счет открыт";
			return $"{(DateTime)value:dd.MM.yyyy}";
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	[ValueConversion(typeof(bool), typeof(string))]

	public class YesNoStringConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if ((bool)value) return "да";
			return "нет";
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	[ValueConversion(typeof(RecalcPeriod), typeof(string))]

	public class RecalcPeriodConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			switch((RecalcPeriod)value)
			{
				case RecalcPeriod.Daily:
					return "ежедневно";
				case RecalcPeriod.Monthly:
					return "ежемесячно";
				case RecalcPeriod.Annually:
					return "ежегодно";
				case RecalcPeriod.AtTheEnd:
					return "в конце периода";
			}
			return "--";
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
