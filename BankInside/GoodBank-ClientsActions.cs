using AccountClasses;
using ClientClasses;
using DTO;
using Interfaces_Actions;
using Interfaces_Data;
using Search;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BankInside
{
	public partial class GoodBank : IClientsActions
	{
		private List<Client> clients;

		/// <summary>
		/// Находит клиента с указанным ID
		/// </summary>
		/// <param name="id">ID клиента</param>
		/// <returns></returns>
		public Client GetClientByID(int id)
		{
			return clients.Find(c => c.ID == id);
		}

		/// <summary>
		/// Находит клиента с указанным ID
		/// </summary>
		/// <param name="id">ID клиента</param>
		/// <returns></returns>
		public IClientDTO GetClientDTObyID(int id)
		{
			return new ClientDTO(clients.Find(c => c.ID == id));
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
					newClient = new ClientSIM(client);
					break;
				case ClientType.Organization:
					newClient = new ClientORG(client);
					break;
			}
			clients.Add(newClient);
			return new ClientDTO(newClient);
		}

		public ObservableCollection<IClientDTO> GetClientsList(ClientType clientType)
		{
			ObservableCollection<IClientDTO> clientsList = new ObservableCollection<IClientDTO>();

			// Here should be ling to entity query
			//foreach (var c in clients) 
				//if (c is TClient) clientsList.Add(new ClientDTO(c));
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
