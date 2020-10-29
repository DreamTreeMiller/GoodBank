using GoodBankNS.AccountClasses;
using GoodBankNS.ClientClasses;
using GoodBankNS.DTO;
using GoodBankNS.Interfaces_Actions;
using GoodBankNS.Interfaces_Data;
using GoodBankNS.Search;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace GoodBankNS.BankInside
{
	public partial class GoodBank : IClientsActions
	{
		/// <summary>
		/// Находит клиента с указанным ID
		/// </summary>
		/// <param name="id">ID клиента</param>
		/// <returns></returns>
		public IClient GetClientByID(uint id)
		{
			return clients.Find(c => c.ID == id);
		}

		public IClientDTO AddClient(IClientDTO client)
		{
			Client newClient = null;
			switch (client.ClientType)
			{
				case ClientType.VIP:
					newClient = new ClientVIP(client);
					break;
				case ClientType.Simple:
					newClient = new СlientSIM(client);
					break;
				case ClientType.Organization:
					newClient = new СlientORG(client);
					break;
			}
			clients.Add(newClient);
			return new ClientDTO(newClient);
		}

		public ObservableCollection<ClientDTO> GetClientsList<TClient>()
		{
			ObservableCollection<ClientDTO> clientsList = new ObservableCollection<ClientDTO>();
			foreach (var c in clients)
				if (c is TClient) clientsList.Add(new ClientDTO(c));
			return clientsList;
		}

		public ObservableCollection<IClientDTO> GetClientsList(Compare predicate)
		{
			ObservableCollection<IClientDTO> clientsList = new ObservableCollection<IClientDTO>();
			foreach (var c in clients)
			{
				bool flag = true;
				flag = predicate(c, ref flag);
				if (flag) clientsList.Add(new ClientDTO(c));
			}
			return clientsList;
		}
		public void UpdateClient(IClientDTO updatedClient)
		{
			int ci = clients.FindIndex(c => c.ID == updatedClient.ID);
			clients[ci].UpdateMyself(updatedClient);
		}
	}
}
