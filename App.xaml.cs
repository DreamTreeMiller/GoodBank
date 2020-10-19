﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace GoodBankNS
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
	private void Application_Startup(object sender, StartupEventArgs e)
	{
		Thread.CurrentThread.CurrentCulture   = new CultureInfo("ru-RU");
		Thread.CurrentThread.CurrentUICulture = new CultureInfo("ru-RU");
	}
	}

}
