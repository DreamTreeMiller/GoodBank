using GoodBank.AccountClasses;
using GoodBank.Client_Classes;
using GoodBank.ClientClasses;
using GoodBank.Interfaces_Actions;
using GoodBank.Interfaces_Data;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace GoodBank.BankInside
{
	public partial class GoodBank : IClientsActions
	{
		public ObservableCollection<IClientDTO> GetClientsList<TClient>()
		{
			ObservableCollection<IClientDTO> clientsList = new ObservableCollection<IClientDTO>();
			foreach (var c in clients)
				if (c is TClient) clientsList.Add(c as IClientDTO);
			return clientsList;
		}
	}
}
