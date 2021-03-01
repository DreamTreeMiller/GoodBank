namespace LoggingNS
{
	public enum TransactionType
	{
		OpenAccount,
		CloseAccount,
		CashDeposit,
		CashWithdrawal,
		ReceiveWireFromAccount,
		SendWireToAccount,
		InterestAccrual,
		BlockAccount,
		TransactionFailed
	}

}
