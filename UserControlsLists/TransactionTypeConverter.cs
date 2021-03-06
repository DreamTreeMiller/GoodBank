﻿using GoodBankNS.AccountClasses;
using GoodBankNS.DTO;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace GoodBankNS.UserControlsLists
{
	[ValueConversion(typeof(OperationType), typeof(string))]
	public class TransactionTypeConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			switch ((OperationType)value)
			{
				case OperationType.OpenAccount:
					return "открытие счета";
				case OperationType.CloseAccount:
					return "закрытие счета";
				case OperationType.CashDeposit:
					return "внесение наличных";
				case OperationType.CashWithdrawal:
					return "снятие наличных";
				case OperationType.ReceiveWireFromAccount:
					return "получение на счет";
				case OperationType.SendWireToAccount:
					return "перевод со счета";
				case OperationType.InterestAccrual:
					return "начисление процентов";
				case OperationType.BlockAccount:
					return "счет заблокирован";
			}
			return "";
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
