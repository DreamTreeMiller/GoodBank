using System;
using System.Collections.Generic;
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
}
}
