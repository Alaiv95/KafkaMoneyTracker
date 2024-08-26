namespace Application.exceptions;

public class BudgetForCategoryAlreadyExistsException : Exception, IApiException
{
    public BudgetForCategoryAlreadyExistsException(string category)
        : base($"Budget for {category} already exists!") { }

    public int ErrorCode => 400;
}