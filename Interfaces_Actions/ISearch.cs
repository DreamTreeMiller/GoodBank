using System.Collections.ObjectModel;
using Interfaces_Data;
using Search;

namespace Interfaces_Actions
{
	public interface ISearch
	{
		ObservableCollection<IClientDTO> FindClients(Compare predicate);
	}
}
