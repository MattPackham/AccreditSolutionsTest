namespace AccreditSolutionsShared.Constants
{
    public class UserMessages
    {
        public const string InvalidUsernameMessage = "The provided username is invalid.";
        public const string NoUserNameProvidedMessage = "Please enter a username.";
        public const string AlphaNumericValidationMessage = "Username can only contain alphanumeric characters and dashes.";
        public const string SeqentialDashesValidationMessage = "Username cannot contain consecutive dashes.";
        public const string UserNotFoundMessage = "A user with the username provided could not be found.";
        public const string GitHubEndpointForbidden = "GitHub API access is forbidden.You may need to authenticate or wait due to rate limits.";
    }
}