using System.Collections.ObjectModel;
using Interfaces_Data;
using Search;
using ClientClasses;

namespace Interfaces_Actions
{
	public interface IClientsActions
	{
		IClientDTO GetClientDTObyID(int id);
		ObservableCollection<IClientDTO> GetClientsList(ClientType clientType);
		ObservableCollection<IClientDTO> GetClientsList(Compare predicate);
		IClientDTO AddClient(IClientDTO client);
		void UpdateClient(IClientDTO updatedClient);
	}
}
