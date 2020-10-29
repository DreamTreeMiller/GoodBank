using GoodBankNS.Interfaces_Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodBankNS.Search
{
	public class TelephoneComparator
	{
		string objectToFind;

		public TelephoneComparator(string value) { objectToFind = value; }

		public bool Compare(IClient sourceP, ref bool flag)
		{
			if (!flag) return false;
			flag = sourceP.Telephone.Contains(objectToFind);
			return flag;
		}
	}

	public class EmailComparator
	{
		string objectToFind;

		public EmailComparator(string value) { objectToFind = value; }

		public bool Compare(IClient sourceP, ref bool flag)
		{
			if (!flag) return false;
			flag = sourceP.Email.Contains(objectToFind);
			return flag;
		}
	}

	public class AddressComparator
	{
		string objectToFind;

		public AddressComparator(string value) { objectToFind = value; }

		public bool Compare(IClient sourceP, ref bool flag)
		{
			if (!flag) return false;
			flag = sourceP.Address.Contains(objectToFind);
			return flag;
		}
	}

}
