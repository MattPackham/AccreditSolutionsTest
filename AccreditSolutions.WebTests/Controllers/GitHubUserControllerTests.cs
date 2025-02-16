using NUnit.Framework;
using Moq;
using AccreditSolutions.Controllers;
using AccreditSolutions.Models.ViewModels;
using AccreditSolutions.Service.Abstract;
using AccreditSolutionsShared.Constants;
using AccreditSolutions.Models.Factories;
using AccreditSolutionsShared.Classes.Abstract;
using System.Net.Http;
using System.Web.Mvc;
using AccreditSolutions.Service.Models;

namespace AccreditSolutions.Web.Tests.Controllers
{
    [TestFixture]
    public class GitHubUserControllerTests
    {
        private Mock<IUsernameValidator> _mockUsernameValidator;
        private Mock<IGitHubService> _mockGitHubService;
        private GitHubUserController _controller;

        [SetUp]
        public void Setup()
        {
            _mockUsernameValidator = new Mock<IUsernameValidator>();
            _mockGitHubService = new Mock<IGitHubService>();
            _controller = new GitHubUserController(_mockUsernameValidator.Object, _mockGitHubService.Object);
        }


        [Test]
        public void RetrieveGitHubUser_PostsWithInvalidUsername_ReturnsViewWithError()
        {
            // Arrange
            var model = new RetrieveGitHubUserViewModel { Username = "" };
            _mockUsernameValidator.Setup(v => v.IsValid(It.IsAny<string>())).Returns(false);

            // Act
            var result = _controller.RetrieveGitHubUser(model) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("RetrieveGitHubUser", result.ViewName);
            Assert.IsTrue(result.ViewData.ModelState.ContainsKey("Message"));
            Assert.AreEqual(UserMessages.InvalidUsernameMessage, result.ViewData.ModelState["Message"].Errors[0].ErrorMessage);
        }

        [Test]
        public void RetrieveGitHubUser_PostsWithValidationMessage_ReturnsViewWithMessage()
        {
            // Arrange
            var model = new RetrieveGitHubUserViewModel { Username = "Invalid--User" };
            _mockUsernameValidator.Setup(v => v.IsValid(It.IsAny<string>())).Returns(false);
            _mockUsernameValidator.Setup(v => v.GetValidationMessage(It.IsAny<string>())).Returns(UserMessages.AlphaNumericValidationMessage);

            // Act
            var result = _controller.RetrieveGitHubUser(model) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("RetrieveGitHubUser", result.ViewName);
            Assert.IsTrue(result.ViewData.ModelState.ContainsKey("Message"));
            Assert.AreEqual(UserMessages.AlphaNumericValidationMessage, result.ViewData.ModelState["Message"].Errors[0].ErrorMessage);
        }

        [Test]
        public void RetrieveGitHubUser_PostsWithUserNotFound_ReturnsViewWithMessage()
        {
            // Arrange
            var model = new RetrieveGitHubUserViewModel { Username = "NonExistentUser" };
            _mockUsernameValidator.Setup(v => v.IsValid(It.IsAny<string>())).Returns(true);
            _mockUsernameValidator.Setup(v => v.GetValidationMessage(It.IsAny<string>())).Returns(string.Empty);
            _mockGitHubService.Setup(s => s.RetrieveUserWithRepositories(It.IsAny<string>())).Returns((GitHubUser)null);

            // Act
            var result = _controller.RetrieveGitHubUser(model) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("RetrieveGitHubUser", result.ViewName);
            Assert.IsTrue(result.ViewData.ModelState.ContainsKey("Message"));
            Assert.AreEqual(UserMessages.UserNotFoundMessage, result.ViewData.ModelState["Message"].Errors[0].ErrorMessage);
        }

        [Test]
        public void RetrieveGitHubUser_PostsWithHttp403Error_ReturnsViewWithMessage()
        {
            // Arrange
            var model = new RetrieveGitHubUserViewModel { Username = "ForbiddenUser" };
            _mockUsernameValidator.Setup(v => v.IsValid(It.IsAny<string>())).Returns(true);
            _mockUsernameValidator.Setup(v => v.GetValidationMessage(It.IsAny<string>())).Returns(string.Empty);
            _mockGitHubService.Setup(s => s.RetrieveUserWithRepositories(It.IsAny<string>())).Throws(new HttpRequestException("403 Forbidden"));

            // Act
            var result = _controller.RetrieveGitHubUser(model) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("RetrieveGitHubUser", result.ViewName);
            Assert.AreEqual(UserMessages.GitHubEndpointForbidden, result.ViewData.ModelState["Message"].Errors[0].ErrorMessage);
        }

        [Test]
        public void RetrieveGitHubUser_PostsWithHttp404Error_ReturnsViewWithMessage()
        {
            // Arrange
            var model = new RetrieveGitHubUserViewModel { Username = "NonExistentUser" };
            _mockUsernameValidator.Setup(v => v.IsValid(It.IsAny<string>())).Returns(true);
            _mockUsernameValidator.Setup(v => v.GetValidationMessage(It.IsAny<string>())).Returns(string.Empty);
            _mockGitHubService.Setup(s => s.RetrieveUserWithRepositories(It.IsAny<string>())).Throws(new HttpRequestException("404 Not Found"));

            // Act
            var result = _controller.RetrieveGitHubUser(model) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("RetrieveGitHubUser", result.ViewName);
            Assert.AreEqual(UserMessages.UserNotFoundMessage, result.ViewData.ModelState["Message"].Errors[0].ErrorMessage);
        }

        [Test]
        public void RetrieveGitHubUser_PostsWithGenericHttpRequestException_ReturnsViewWithMessage()
        {
            // Arrange
            var model = new RetrieveGitHubUserViewModel { Username = "SomeUser" };
            _mockUsernameValidator.Setup(v => v.IsValid(It.IsAny<string>())).Returns(true);
            _mockUsernameValidator.Setup(v => v.GetValidationMessage(It.IsAny<string>())).Returns(string.Empty);
            _mockGitHubService.Setup(s => s.RetrieveUserWithRepositories(It.IsAny<string>())).Throws(new HttpRequestException("Unknown error occurred"));

            // Act
            var result = _controller.RetrieveGitHubUser(model) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("RetrieveGitHubUser", result.ViewName);
            Assert.AreEqual("Unknown error occurred", result.ViewData.ModelState["Message"].Errors[0].ErrorMessage);
        }

        [Test]
        public void RetrieveGitHubUser_PostsWithValidUser_ReturnsGitHubUserResultsView()
        {
            // Arrange
            var model = new RetrieveGitHubUserViewModel { Username = "ValidUser" };
            var fakeGitHubUser = new GitHubUser { UserName = "ValidUser" };
            var fakeViewModel = new GitHubUserResultViewModelFactory(fakeGitHubUser).Create();

            _mockUsernameValidator.Setup(v => v.IsValid(It.IsAny<string>())).Returns(true);
            _mockUsernameValidator.Setup(v => v.GetValidationMessage(It.IsAny<string>())).Returns(string.Empty);
            _mockGitHubService.Setup(s => s.RetrieveUserWithRepositories(It.IsAny<string>())).Returns(fakeGitHubUser);

            // Act
            var result = _controller.RetrieveGitHubUser(model) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("GitHubUserResults", result.ViewName);
            Assert.IsInstanceOf<GitHubUserResultViewModel>(result.Model);
        }

    }
}
