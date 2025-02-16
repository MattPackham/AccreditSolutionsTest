using AccreditSolutionsShared.Classes.Abstract;
using AccreditSolutionsShared.Constants;
using System.Text.RegularExpressions;

namespace AccreditSolutionsShared.Classes.Concrete
{
    public class UsernameValidator : IUsernameValidator
    {
        public string GetValidationMessage(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return UserMessages.NoUserNameProvidedMessage;
            }

            if (username.StartsWith("-") || username.EndsWith("-"))
            {
                return UserMessages.AlphaNumericValidationMessage;
            }

            if (username.Contains("--"))
            {
                return UserMessages.SeqentialDashesValidationMessage;
            }

            var validUsernameRegex = new Regex(RegexPatterns.ValidUsernameRegex);

            if (!validUsernameRegex.IsMatch(username))
            {
                return UserMessages.AlphaNumericValidationMessage;
            }

            return null;
        }

        public bool IsValid(string username)
        { 
            if (string.IsNullOrWhiteSpace(username)) return false; 

            var validUsernameRegex = new Regex(RegexPatterns.ValidUsernameRegex);

            if (!validUsernameRegex.IsMatch(username)) return false;

            if (username.StartsWith("-") || username.EndsWith("-")) return false;

            if (username.Contains("--")) return false;

            return true;
        }
    }
}
