using GoodBankNS.DTO;
using GoodBankNS.Interfaces_Data;
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
		ObservableCollection<ClientDTO> GetClientsList<TClient>();
		IClientDTO AddClient(IClientDTO client);
		void UpdateClient(IClientDTO updatedClient);
	}
}
