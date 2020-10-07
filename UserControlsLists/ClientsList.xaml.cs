using GoodBankNS.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GoodBankNS.UserControlsLists
{
	/// <summary>
	/// Interaction logic for ClientsList.xaml
	/// </summary>
	public partial class ClientsList : UserControl
	{
		public ClientsList(ClientsViewNameTags tags)
		{
			InitializeComponent();
			ClientsDataGrid.Items.Clear(); // Почему-то вставляется пустой элемент после инициализации
										   // надо удалить, чтобы корректно всё работало
			InitializeColumnsTags(tags);
		}

		private void InitializeColumnsTags(ClientsViewNameTags tags)
		{
			// Названия чекбоксов
			CreationDateCheckBox.Content  = tags.CreationDateCBTag;
			PassportOrTINCheckBox.Content = tags.PassportOrTIN_CB_Tag;

			// Таблица 
			// Показывать колонку типa (физик / юрик) или нет
			if (tags.ClientTypeColumn)
				ClientTypeColumn.Visibility = Visibility.Visible;
			else
				ClientTypeColumn.Visibility = Visibility.Collapsed;

			// ФИО или Название организации
			MainNameColumn.Header		  = tags.MainNameTag;    

			// Сводка: ВИП клиентов, физиков, юриков
			ClientsTotalNumberTitle.Text  = tags.ClientNameTag;  
		}

		#region CheckBoxes handlers

		private void CreationDateCheckBox_Click(object sender, RoutedEventArgs e)
		{
			if (CreationDateCheckBox.IsChecked == true)
				CreationDateColumn.Visibility = Visibility.Visible;
			else
				CreationDateColumn.Visibility = Visibility.Collapsed;
		}

		private void PassportOrTINCheckBox_Click(object sender, RoutedEventArgs e)
		{
			if (PassportOrTINCheckBox.IsChecked == true)
				PassportOrTINColumn.Visibility = Visibility.Visible;
			else
				PassportOrTINColumn.Visibility = Visibility.Collapsed;
		}

		private void TelCheckBox_Click(object sender, RoutedEventArgs e)
		{
			if (TelCheckBox.IsChecked == true)
				TelephoneColumn.Visibility = Visibility.Visible;
			else
				TelephoneColumn.Visibility = Visibility.Collapsed;
		}

		private void EmailCheckBox_Click(object sender, RoutedEventArgs e)
		{
			if (EmailCheckBox.IsChecked == true)
				EmailColumn.Visibility = Visibility.Visible;
			else
				EmailColumn.Visibility = Visibility.Collapsed;
		}

		private void AddressCheckBox_Click(object sender, RoutedEventArgs e)
		{
			if (AddressCheckBox.IsChecked == true)
				AddressColumn.Visibility = Visibility.Visible;
			else
				AddressColumn.Visibility = Visibility.Collapsed;
		}

		private void NumOfClosedAccountsCheckBox_Click(object sender, RoutedEventArgs e)
		{
			if (NumOfClosedAccountsCheckBox.IsChecked == true)
				NummberOfClosedAccountsColumn.Visibility = Visibility.Visible;
			else
				NummberOfClosedAccountsColumn.Visibility = Visibility.Collapsed;
		}

		#endregion

	}
}
