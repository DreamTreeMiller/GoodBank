using GoodBankNS.DTO;
using GoodBankNS.Interfaces_Data;
using GoodBankNS.Search;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodBankNS.Interfaces_Actions
{
	public interface IClientsActions
	{
		IClient GetClientByID(uint id);
		ObservableCollection<ClientDTO> GetClientsList<TClient>();
		ObservableCollection<IClientDTO> GetClientsList(Compare predicate);
		IClientDTO AddClient(IClientDTO client);
		void UpdateClient(IClientDTO updatedClient);
	}
}
