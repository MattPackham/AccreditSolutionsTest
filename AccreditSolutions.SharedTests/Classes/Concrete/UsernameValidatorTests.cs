using AccreditSolutionsShared.Classes.Abstract;
using AccreditSolutionsShared.Classes.Concrete;
using AccreditSolutionsShared.Constants;
using NUnit.Framework;

namespace Service.Tests
{
    [TestFixture]
    public class UserValidationTests
    {
        private IUsernameValidator _usernameValidator;

        [SetUp]
        public void SetUp()
        {
            _usernameValidator = new UsernameValidator();
        }

        [Test]
        public void GetValidationMessage_ShouldReturnNoUserNameProvidedMessage_WhenUsernameIsNull()
        {
            // Arrange
            string username = null;
            var expectedMessage = UserMessages.NoUserNameProvidedMessage;

            // Act
            var result = _usernameValidator.GetValidationMessage(username);

            // Assert
            Assert.That(result, Is.EqualTo(expectedMessage));
        }

        [Test]
        public void GetValidationMessage_ShouldReturnNoUserNameProvidedMessage_WhenUsernameIsEmpty()
        {
            // Arrange
            var username = string.Empty;
            var expectedMessage = UserMessages.NoUserNameProvidedMessage;

            // Act
            var result = _usernameValidator.GetValidationMessage(username);

            // Assert
            Assert.That(result, Is.EqualTo(expectedMessage));
        }

        [Test]
        public void GetValidationMessage_ShouldReturnAlphaNumericValidationMessage_WhenUsernameContainsNonAlphaNumericChars()
        {
            // Arrange
            var username = "invalid@username";
            var expectedMessage = UserMessages.AlphaNumericValidationMessage;

            // Act
            var result = _usernameValidator.GetValidationMessage(username);

            // Assert
            Assert.That(result, Is.EqualTo(expectedMessage));
        }

        [Test]
        public void GetValidationMessage_ShouldReturnAlphaNumericValidationMessage_WhenUsernameStartsWithDash()
        {
            // Arrange
            var username = "-username";
            var expectedMessage = UserMessages.AlphaNumericValidationMessage;

            // Act
            var result = _usernameValidator.GetValidationMessage(username);

            // Assert
            Assert.That(result, Is.EqualTo(expectedMessage));
        }

        [Test]
        public void GetValidationMessage_ShouldReturnAlphaNumericValidationMessage_WhenUsernameEndsWithDash()
        {
            // Arrange
            var username = "username-";
            var expectedMessage = UserMessages.AlphaNumericValidationMessage;

            // Act
            var result = _usernameValidator.GetValidationMessage(username);

            // Assert
            Assert.That(result, Is.EqualTo(expectedMessage));
        }

        [Test]
        public void GetValidationMessage_ShouldReturnSeqentialDashesValidationMessage_WhenUsernameContainsSequentialDashes()
        {
            // Arrange
            var username = "user--name";
            var expectedMessage = UserMessages.SeqentialDashesValidationMessage;

            // Act
            var result = _usernameValidator.GetValidationMessage(username);

            // Assert
            Assert.That(result, Is.EqualTo(expectedMessage));
        }

        [Test]
        public void GetValidationMessage_ShouldReturnNull_WhenUsernameIsValid()
        {
            // Arrange
            var username = "validusername";

            // Act
            var result = _usernameValidator.GetValidationMessage(username);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void IsValid_ShouldReturnFalse_WhenUsernameIsNull()
        {
            // Arrange
            string username = null;

            // Act
            var result = _usernameValidator.IsValid(username);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void IsValid_ShouldReturnFalse_WhenUsernameIsEmpty()
        {
            // Arrange
            var username = string.Empty;

            // Act
            var result = _usernameValidator.IsValid(username);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void IsValid_ShouldReturnFalse_WhenUsernameContainsNonAlphaNumericChars()
        {
            // Arrange
            var username = "invalid@username";

            // Act
            var result = _usernameValidator.IsValid(username);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void IsValid_ShouldReturnFalse_WhenUsernameStartsWithDash()
        {
            // Arrange
            var username = "-username";

            // Act
            var result = _usernameValidator.IsValid(username);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void IsValid_ShouldReturnFalse_WhenUsernameEndsWithDash()
        {
            // Arrange
            var username = "username-";

            // Act
            var result = _usernameValidator.IsValid(username);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void IsValid_ShouldReturnFalse_WhenUsernameContainsSequentialDashes()
        {
            // Arrange
            var username = "user--name";

            // Act
            var result = _usernameValidator.IsValid(username);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void IsValid_ShouldReturnTrue_WhenUsernameIsValid()
        {
            // Arrange
            var username = "validusername";

            // Act
            var result = _usernameValidator.IsValid(username);

            // Assert
            Assert.That(result, Is.True);
        }
    }
}
