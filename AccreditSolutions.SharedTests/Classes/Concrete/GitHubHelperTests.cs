using AccreditSolutionsShared.Classes.Concrete;
using NUnit.Framework;
using System;

namespace AccreditSolutions.SharedTests.Classes.Concrete
{
    [TestFixture]
    public class GitHubHelperTests
    {
        [Test]
        public void BuildUserRetrievalUrl_ShouldReturnCorrectUrl_WhenValidInputs()
        {
            // Arrange
            var baseUrl = "https://api.github.com/users/";
            var userName = "validuser";

            // Act
            var result = GitHubHelper.BuildUserRetrievalUrl(baseUrl, userName);

            // Assert
            Assert.That(result, Is.EqualTo("https://api.github.com/users/validuser"));
        }

        [Test]
        public void BuildUserRetrievalUrl_ShouldThrowArgumentNullException_WhenBaseUrlIsNull()
        {
            // Arrange
            string baseUrl = null;
            var userName = "validuser";

            // Assert
            Assert.Throws<ArgumentNullException>(() => GitHubHelper.BuildUserRetrievalUrl(baseUrl, userName));
        }

        [Test]
        public void BuildUserRetrievalUrl_ShouldThrowArgumentNullException_WhenUserNameIsNull()
        {
            // Arrange
            var baseUrl = "https://api.github.com/users/";
            string userName = null;

            // Assert
            Assert.Throws<ArgumentNullException>(() => GitHubHelper.BuildUserRetrievalUrl(baseUrl, userName));
        }

        [Test]
        public void BuildUserRetrievalUrl_ShouldThrowArgumentNullException_WhenBothBaseUrlAndUserNameAreNull()
        {
            // Arrange
            string baseUrl = null;
            string userName = null;

            // Assert
            Assert.Throws<ArgumentNullException>(() => GitHubHelper.BuildUserRetrievalUrl(baseUrl, userName));
        }

        [Test]
        public void BuildUserRetrievalUrl_ShouldThrowArgumentNullException_WhenBaseUrlIsEmpty()
        {
            // Arrange
            var baseUrl = string.Empty;
            var userName = "validuser";

            // Assert
            Assert.Throws<ArgumentNullException>(() => GitHubHelper.BuildUserRetrievalUrl(baseUrl, userName));
        }

        [Test]
        public void BuildUserRetrievalUrl_ShouldThrowArgumentNullException_WhenUserNameIsEmpty()
        {
            // Arrange
            var baseUrl = "https://api.github.com/users/";
            var userName = "";

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => GitHubHelper.BuildUserRetrievalUrl(baseUrl, userName));
        }

        [Test]
        public void BuildUserRetrievalUrl_ShouldReturnCorrectUrl_WhenUserNameHasSpecialCharacters()
        {
            // Arrange
            var baseUrl = "https://api.github.com/users/";
            var userName = "valid@user123";

            // Act
            var result = GitHubHelper.BuildUserRetrievalUrl(baseUrl, userName);

            // Assert
            Assert.That(result, Is.EqualTo("https://api.github.com/users/valid@user123"));
        }

        [Test]
        public void BuildUserRetrievalUrl_ShouldReturnCorrectUrl_WhenBaseUrlHasTrailingSlash()
        {
            // Arrange
            var baseUrl = "https://api.github.com/users/";
            var userName = "validuser";

            // Act
            var result = GitHubHelper.BuildUserRetrievalUrl(baseUrl, userName);

            // Assert
            Assert.That(result, Is.EqualTo("https://api.github.com/users/validuser"));
        }
    }
}