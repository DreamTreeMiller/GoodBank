using GoodBank.AccountClasses;
using GoodBank.Client_Classes;
using GoodBank.ClientClasses;
using GoodBank.DTO;
using GoodBank.Interfaces_Actions;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace GoodBank.BankInside
{
	public partial class GoodBank : IClientsActions
	{
		public ObservableCollection<ClientDTO> GetClientsList<TClient>()
		{
			ObservableCollection<ClientDTO> clientsList = new ObservableCollection<ClientDTO>();
			foreach (var c in clients)
				if (c is TClient) clientsList.Add(new ClientDTO(c));
			return clientsList;
		}
	}
}
