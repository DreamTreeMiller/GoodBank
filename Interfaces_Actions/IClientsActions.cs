using System.Collections.ObjectModel;
using Interfaces_Data;
using ClientClasses;

namespace Interfaces_Actions
{
	public interface IClientsActions
	{
		IClientDTO GetClientDTObyID(int id);
		ObservableCollection<IClientDTO> GetClientsList(ClientType clientType);
		IClientDTO AddClient(IClientDTO client);
		void UpdateClient(IClientDTO updatedClient);
	}
}
