namespace FinanceTracker.Exceptions;

public class TransactionBadRequestException : Exception
{
    public TransactionBadRequestException(string message) : base(message)
    {
        
    }
}