using System;
using System.Linq;
using ClientClasses;

namespace Search
{
	public class OrgNameComparator
	{
		string objectToFind;

		public OrgNameComparator(string value) { objectToFind = value; }

		public bool Compare(Client sourceP, ref bool flag)
		{
			if (!flag) return false;
			flag = (sourceP as ClientORG).OrgName.Contains(objectToFind);
			return flag;
		}
	}


	public class DirectorFirstNameComparator
	{
		string objectToFind;

		public DirectorFirstNameComparator(string value) { objectToFind = value; }

		public bool Compare(Client sourceP, ref bool flag)
		{
			if (!flag) return false;
			flag = (sourceP as ClientORG).DirectorFirstName.Contains(objectToFind);
			return flag;
		}
	}

	public class DirectorMiddleNameComparator
	{
		string objectToFind;

		public DirectorMiddleNameComparator(string value) { objectToFind = value; }

		public bool Compare(Client sourceP, ref bool flag)
		{
			if (!flag) return false;
			flag = (sourceP as ClientORG).DirectorMiddleName.Contains(objectToFind);
			return flag;
		}
	}

	public class DirectorLastNameComparator
	{
		string objectToFind;

		public DirectorLastNameComparator(string value) { objectToFind = value; }

		public bool Compare(Client sourceP, ref bool flag)
		{
			if (!flag) return false;
			flag = (sourceP as ClientORG).DirectorLastName.Contains(objectToFind);
			return flag;
		}
	}

	public class RegistrationStartDateComparator
	{
		DateTime objectToFind;

		public RegistrationStartDateComparator(DateTime value) { objectToFind = value; }

		public bool Compare(Client sourceP, ref bool flag)
		{
			if (!flag) return false;
			flag = objectToFind <= (sourceP as ClientORG).RegistrationDate;
			return flag;
		}
	}

	public class RegistrationEndDateComparator
	{
		DateTime objectToFind;

		public RegistrationEndDateComparator(DateTime value) { objectToFind = value; }

		public bool Compare(Client sourceP, ref bool flag)
		{
			if (!flag) return false;
			flag = (sourceP as ClientORG).RegistrationDate <= objectToFind;
			return flag;
		}
	}

	public class TINComparator
	{
		string objectToFind;

		public TINComparator(string value) { objectToFind = value; }

		public bool Compare(Client sourceP, ref bool flag)
		{
			if (!flag) return false;
			flag = (sourceP as ClientORG).TIN.Contains(objectToFind);
			return flag;
		}
	}

}
