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
using System.Windows.Shapes;

namespace GoodBankNS.Imitation
{
	/// <summary>
	/// Interaction logic for GenerateWindow.xaml
	/// </summary>
	public partial class GenerateWindow : Window
	{
		public int vipClients;
		public string VIPclients 
		{ 
			get => $"{vipClients}";
			set 
			{
				if (!Int32.TryParse(value, out int tmp)) return;
				if (tmp < 0) tmp = 1;
				vipClients = tmp; 
			}
		}
		public int simClients;
		public string SIMclients
		{
			get => $"{simClients}";
			set
			{
				if (!Int32.TryParse(value, out int tmp)) return;
				if (tmp < 0) tmp = 1;
				simClients = tmp;
			}
		}
		public int orgClients;
		public string ORGclients
		{
			get => $"{orgClients}";
			set
			{
				if (!Int32.TryParse(value, out int tmp)) return;
				if (tmp < 0) tmp = 1;
				orgClients = tmp;
			}
		}

		public GenerateWindow()
		{
			InitializeComponent();
			vipClients = 10;
			simClients = 10;
			orgClients = 10;
			DataContext = this;
		}

		private void btnOk_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = true;
		}
	}
}
