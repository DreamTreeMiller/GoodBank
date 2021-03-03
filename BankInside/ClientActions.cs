﻿using System.Linq;
using System.Collections.ObjectModel;
using ClientClasses;
using DTO;
using Interfaces_Actions;
using Interfaces_Data;

namespace BankInside
{
	public class ClientActions : IClientActions
	{
		private readonly IRepository dbe;
		public ClientActions(IRepository dbengine) { dbe = dbengine; }

		/// <summary>
		/// Находит клиента с указанным ID
		/// </summary>
		/// <param name="id">ID клиента</param>
		/// <returns></returns>
		public Client GetClientByID(int id)
		{
			return dbe.GetClientByID(id);
		}

		/// <summary>
		/// Находит клиента с указанным ID
		/// </summary>
		/// <param name="id">ID клиента</param>
		/// <returns></returns>
		public IClientDTO GetClientDTObyID(int id)
		{
			return new ClientDTO(dbe.GetClientByID(id));
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
			dbe.AddClient(newClient);
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
			foreach (Client c in dbe.GetClients())
				if (c is TClient) clientsList.Add(new ClientDTO(c));
			clientsList = new ObservableCollection<IClientDTO>(clientsList.OrderBy(c => c.ID));
			return clientsList;
		}

		public void UpdateClient(IClientDTO updatedClient)
		{
			Client client = dbe.GetClientByID(updatedClient.ID);
			client.UpdateMyself(updatedClient);
			dbe.SaveChanges();
		}
	}
}
