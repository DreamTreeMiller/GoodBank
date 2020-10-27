﻿using GoodBankNS.Binding_UI_CondeBehind;
using GoodBankNS.Imitation;
using GoodBankNS.Interfaces_Data;
using GoodBankNS.UI_clients;
using GoodBankNS.BankInside;
using System.Windows;
using GoodBankNS.UserControlsLists;

namespace GoodBankNS
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private IGoodBank GoodBank;
		private BankActions BA;
		public string BankFoundationDay = "Основан " 
			+ $"{GoodBankNS.BankInside.GoodBank.BankFoundationDay:D}"
			;

		public MainWindow()
		{
			InitializeComponent();
			InitializeBank();
			InitializeWelcomeScreenMessages();
			
		}

		private void InitializeBank()
		{
			GoodBank = new GoodBank();
			BA		 = new BankActions(GoodBank);
		}

		private void InitializeWelcomeScreenMessages()
		{
			BankFoundationDayMessage.Text = BankFoundationDay;
			BankTodayDate.Text = $"Сегодня {GoodBankNS.BankInside.GoodBank.Today:dd MMMM yyyy} г.";
		}

		private void VipClientsDeptButton_Click(object sender, RoutedEventArgs e)
		{
			DepartmentWindow vipClientsWin = new DepartmentWindow(WindowID.DepartmentVIP, BA);
			vipClientsWin.ShowDialog();
		}

		private void SimpleClientsDeptButton_Click(object sender, RoutedEventArgs e)
		{
			DepartmentWindow simpleClientsWin = new DepartmentWindow(WindowID.DepartmentSIM, BA);
			simpleClientsWin.ShowDialog();
		}

		private void OrgClientsDeptButton_Click(object sender, RoutedEventArgs e)
		{
			DepartmentWindow orgClientsWin = new DepartmentWindow(WindowID.DepartmentORG, BA);
			orgClientsWin.ShowDialog();
		}

		private void BankManagerButton_Click(object sender, RoutedEventArgs e)
		{
			DepartmentWindow allClientsWin = new DepartmentWindow(WindowID.DepartmentALL, BA);
			allClientsWin.ShowDialog();
		}

		private void SearchButton_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("Функция поиска в разработке");
		}

		private void TimeMachineButton_Click(object sender, RoutedEventArgs e)
		{
			BA.Accounts.AddOneMonth();
			BankTodayDate.Text = $"Сегодня {GoodBankNS.BankInside.GoodBank.Today:dd MMMM yyyy} г.";
			MessageBox.Show("Время в мире, где существует банк, ушло на месяц вперёд.\n"
						  + "Пересчитаны проценты на всех счетах.");

		}

		private void GenerateButton_Click(object sender, RoutedEventArgs e)
		{
			var gw = new GenerateWindow();
			var result = gw.ShowDialog();
			if (result != true) return;
			Generate.Bank(BA, gw.vipClients, gw.simClients, gw.orgClients);
			MessageBox.Show("Клиенты и счета созданы!");
		}
	}
}
