using GoodBank.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodBank.Interfaces_Actions
{
	public interface IClientsActions
	{
		ObservableCollection<ClientDTO> GetClientsList<TClient>();
	}
}
