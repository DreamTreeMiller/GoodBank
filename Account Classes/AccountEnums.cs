namespace AccountClasses
{
	public enum AccountType
	{
		Current,
		Deposit,
		Credit,
		Total
	}

	public enum AccountStatus
	{
		Opened,
		Closed
	}

	public enum RecalcPeriod
	{
		Monthly,
		Annually,
		AtTheEnd,
		NoRecalc
	}
}
