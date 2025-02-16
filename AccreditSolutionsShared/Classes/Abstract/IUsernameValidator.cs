namespace AccreditSolutionsShared.Classes.Abstract
{
    public interface IUsernameValidator
    {
        bool IsValid(string username);
        string GetValidationMessage(string username);
    }
}
