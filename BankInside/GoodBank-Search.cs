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
	public partial class GoodBank : ISearch
	{
		/// <summary>
		/// Ищет клиентов, удовлетворяющих условиям в предикате. 
		/// Предикат - набор делегатов, каждый из которых проверяет одно из полей клиента.
		/// Если поиск по какому-либо полю не задан, то делегат предиката равен null
		/// </summary>
		/// <param name="predicate"></param>
		/// <returns></returns>
		public ObservableCollection<IClientDTO> GetClientsList(Compare predicate)
		{
			ObservableCollection<IClientDTO> clientsList = new ObservableCollection<IClientDTO>();
			foreach (Client c in db.Clients)
			{
				bool flag = true;
				flag = predicate(c, ref flag);
				if (flag) clientsList.Add(new ClientDTO(c));
			}
			return clientsList;
		}
	}
}
