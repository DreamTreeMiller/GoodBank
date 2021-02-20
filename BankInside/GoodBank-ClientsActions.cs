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
		/// <summary>
		/// Находит клиента с указанным ID
		/// </summary>
		/// <param name="id">ID клиента</param>
		/// <returns></returns>
		public Client GetClientByID(int id)
		{
			return db.Clients.Find(id);
		}

		/// <summary>
		/// Находит клиента с указанным ID
		/// </summary>
		/// <param name="id">ID клиента</param>
		/// <returns></returns>
		public IClientDTO GetClientDTObyID(int id)
		{
			return new ClientDTO(db.Clients.Find(id));
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
			db.Clients.Add(newClient);
			db.SaveChanges();
			return new ClientDTO(newClient);
		}

		public ObservableCollection<IClientDTO> GetClientsList(ClientType clientType)
		{
			// Here should be ling to entity query
			switch(clientType)
			{
				case ClientType.VIP:
					return GetClientsList<ClientVIP>();
				case ClientType.Simple:
					return GetClientsList<ClientSIM>();
				case ClientType.Organization:
					return GetClientsList<ClientORG>();
			}
			return GetClientsList<Client>();
		}

		private ObservableCollection<IClientDTO> GetClientsList<TClient>()
		{
			ObservableCollection<IClientDTO> clientsList = new ObservableCollection<IClientDTO>();

			// TODO Here should be linq to entity query
			foreach (Client c in db.Clients)
				if (c is TClient) clientsList.Add(new ClientDTO(c));
			return clientsList;
		}

		public void UpdateClient(IClientDTO updatedClient)
		{
			Client client = db.Clients.Find(updatedClient.ID);
			client.UpdateMyself(updatedClient);
			db.SaveChanges();
		}
	}
}
