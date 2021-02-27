using System.Collections.ObjectModel;
using ClientClasses;
using DTO;
using Interfaces_Actions;
using Interfaces_Data;

namespace Search
{
	public class SearchEngine : ISearch
	{
		private readonly IRepository dbe;
		public SearchEngine(IRepository dbengine) { dbe = dbengine; }

		/// <summary>
		/// Ищет клиентов, удовлетворяющих условиям в предикате. 
		/// Предикат - набор делегатов, каждый из которых проверяет одно из полей клиента.
		/// Если поиск по какому-либо полю не задан, то делегат предиката равен null
		/// </summary>
		/// <param name="predicate"></param>
		/// <returns></returns>
		public ObservableCollection<IClientDTO> FindClients(Compare predicate)
		{
			ObservableCollection<IClientDTO> clientsList = new ObservableCollection<IClientDTO>();
			foreach (Client c in dbe.GetClients())
			{
				bool flag = true;
				flag = predicate(c, ref flag);
				if (flag) clientsList.Add(new ClientDTO(c));
			}
			return clientsList;
		}
	}
}
