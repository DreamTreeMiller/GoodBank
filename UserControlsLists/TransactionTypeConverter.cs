using LoggingNS;
using System;
using System.Globalization;
using System.Windows.Data;

namespace UserControlsLists
{
	[ValueConversion(typeof(TransactionType), typeof(string))]
	public class TransactionTypeConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			switch ((TransactionType)value)
			{
				case TransactionType.OpenAccount:
					return "открытие счета";
				case TransactionType.CloseAccount:
					return "закрытие счета";
				case TransactionType.CashDeposit:
					return "внесение наличных";
				case TransactionType.CashWithdrawal:
					return "снятие наличных";
				case TransactionType.ReceiveWireFromAccount:
					return "получение на счет";
				case TransactionType.SendWireToAccount:
					return "перевод со счета";
				case TransactionType.InterestAccrual:
					return "начисление процентов";
				case TransactionType.BlockAccount:
					return "счет заблокирован";
				case TransactionType.TransactionFailed:
					return "неудача";
				default:
					return "неизвестная операция";
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
