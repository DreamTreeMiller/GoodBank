using System;
using System.Linq;
using ClientClasses;

namespace Search
{
	public class FirstNameComparator
	{
		string objectToFind;

		public FirstNameComparator(string value) { objectToFind = value; }

		public bool Compare(Client sourceP, ref bool flag)
		{
			if (!flag) return false;
			if (sourceP is ClientVIP)
				flag = (sourceP as ClientVIP).FirstName.Contains(objectToFind);
			if (sourceP is ClientSIM)
				flag = (sourceP as ClientSIM).FirstName.Contains(objectToFind);
			return flag;
		}
	}

	public class MiddleNameComparator
	{
		string objectToFind;

		public MiddleNameComparator(string value) { objectToFind = value; }

		public bool Compare(Client sourceP, ref bool flag)
		{
			if (!flag) return false;
			if (sourceP is ClientVIP)
				flag = (sourceP as ClientVIP).MiddleName.Contains(objectToFind);
			if (sourceP is ClientSIM)
				flag = (sourceP as ClientSIM).MiddleName.Contains(objectToFind);
			return flag;
		}
	}

	public class LastNameComparator
	{
		string objectToFind;

		public LastNameComparator(string value) { objectToFind = value; }

		public bool Compare(Client sourceP, ref bool flag)
		{
			if (!flag) return false;
			if (sourceP is ClientVIP)
				flag = (sourceP as ClientVIP).LastName.Contains(objectToFind);
			if (sourceP is ClientSIM)
				flag = (sourceP as ClientSIM).LastName.Contains(objectToFind);
			return flag;
		}
	}

	public class StartDateComparator
	{
		DateTime objectToFind;

		public StartDateComparator(DateTime value) { objectToFind = value; }

		public bool Compare(Client sourceP, ref bool flag)
		{
			if (!flag) return false;
			flag = objectToFind <= (sourceP as ClientSIM).BirthDate;
			return flag;
		}
	}

	public class EndDateComparator
	{
		DateTime objectToFind;

		public EndDateComparator(DateTime value) { objectToFind = value; }

		public bool Compare(Client sourceP, ref bool flag)
		{
			if (!flag) return false;
			if (sourceP is ClientVIP)
				flag = (sourceP as ClientVIP).BirthDate <= objectToFind;
			if (sourceP is ClientSIM)
				flag = (sourceP as ClientSIM).BirthDate <= objectToFind;
			return flag;
		}
	}

	public class PassportNumberComparator
	{
		string objectToFind;

		public PassportNumberComparator(string value) { objectToFind = value; }

		public bool Compare(Client sourceP, ref bool flag)
		{
			if (!flag) return false;
			if (sourceP is ClientVIP)
				flag = (sourceP as ClientVIP).PassportNumber.Contains(objectToFind);
			if (sourceP is ClientSIM)
				flag = (sourceP as ClientSIM).PassportNumber.Contains(objectToFind);
			return flag;
		}
	}
}
