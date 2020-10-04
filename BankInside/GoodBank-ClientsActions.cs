using GoodBank.AccountClasses;
using GoodBank.ClientClasses;
using GoodBank.DTO;
using GoodBank.Interfaces_Actions;
using GoodBank.Interfaces_Data;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace GoodBank.BankInside
{
	public partial class GoodBank : IClientsActions
	{
		public void AddClient(IClientDTO client)
		{
			switch (client.ClientType)
			{
				case ClientType.VIP:
					clients.Add(new ClientVIP(client));
					break;
				case ClientType.Simple:
					clients.Add(new СlientSIM(client));
					break;
				case ClientType.Organization:
					clients.Add(new СlientORG(client));
					break;
			}
		}

		public ObservableCollection<ClientDTO> GetClientsList<TClient>()
		{
			ObservableCollection<ClientDTO> clientsList = new ObservableCollection<ClientDTO>();
			foreach (var c in clients)
				if (c is TClient) clientsList.Add(new ClientDTO(c));
			return clientsList;
		}
	}
}
