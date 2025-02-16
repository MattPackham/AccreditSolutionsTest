using Moq;
using NUnit.Framework;
using AccreditSolutions.Service.Abstract;
using AccreditSolutions.Service.Concrete;
using System;
using System.Net.Http;
using Newtonsoft.Json;

namespace Service.Tests
{
    [TestFixture]
    public class GitHubServiceTests
    {
        private Mock<IWebClient> _mockWebClient;
        private Mock<IEnvironmentVariables> _mockEnvironmentVariables;
        private IGitHubService _gitHubService;

        [SetUp]
        public void SetUp()
        {
            _mockWebClient = new Mock<IWebClient>();
            _mockEnvironmentVariables = new Mock<IEnvironmentVariables>();
            _gitHubService = new GitHubService(_mockWebClient.Object, _mockEnvironmentVariables.Object);
        }

        [Test]
        public void RetrieveUserWithRepositories_ShouldThrowArgumentNullException_WhenUserNotProvided()
        { 
            // Assert
            Assert.Throws<ArgumentNullException>(() => _gitHubService.RetrieveUserWithRepositories(null));
        }


        [Test]
        public void RetrieveUserWithRepositories_ShouldThrowWebException_WhenUserNotFound()
        {      
            // Arrange
            var username = "nonexistentuser";
            var url = "https://api.github.com/users/nonexistentuser";
            _mockEnvironmentVariables.Setup(x => x.GitHubUrl).Returns("https://api.github.com/users/");
            _mockWebClient.Setup(x => x.DownloadString(url)).Throws(new HttpRequestException());

            // Assert
            Assert.Throws<HttpRequestException>(() => _gitHubService.RetrieveUserWithRepositories(username));
        }

        [Test]
        public void RetrieveUserWithRepositories_ShouldThrowJsonReaderException_WhenJsonInvalid()
        {
            // Arrange
            var username = "nonexistentuser";
            var url = "https://api.github.com/users/nonexistentuser";
            _mockEnvironmentVariables.Setup(x => x.GitHubUrl).Returns("https://api.github.com/users/");
            _mockWebClient.Setup(x => x.DownloadString(url)).Returns("InvalidJson");

            // Assert
            Assert.Throws<JsonReaderException>(() => _gitHubService.RetrieveUserWithRepositories(username));
        }

        [Test]
        public void RetrieveUserWithRepositories_ShouldReturnNull_WhenUserNotFound()
        {
            // Arrange
            var username = "nonexistentuser";
            var url = "https://api.github.com/users/nonexistentuser";
            _mockEnvironmentVariables.Setup(x => x.GitHubUrl).Returns("https://api.github.com/users/");
            _mockWebClient.Setup(x => x.DownloadString(url)).Returns(string.Empty);

            // Act
            var result = _gitHubService.RetrieveUserWithRepositories(username);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void RetrieveUserWithRepositories_ShouldReturnGitHubUserWithoNoRepos_WhenUserButHasNoRepos()
        {
            // Arrange
            var username = "validuser";
            var url = "https://api.github.com/users/validuser";
            var jsonResponse = "{\"login\":\"validuser\",\"location\":\"Location1\",\"avatar_url\":\"http://example.com/avatar\",\"repos_url\":\"https://api.github.com/users/validuser/repos\"}";

            _mockEnvironmentVariables.Setup(x => x.GitHubUrl).Returns("https://api.github.com/users/");
            _mockWebClient.Setup(x => x.DownloadString(url)).Returns(jsonResponse);

            var userUrl = "https://api.github.com/users/validuser/repos";
            _mockWebClient.Setup(x => x.DownloadString(userUrl)).Returns("");

            _mockEnvironmentVariables.Setup(x => x.NumberOfRepositories).Returns(5);

            // Act
            var result = _gitHubService.RetrieveUserWithRepositories(username);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.UserName, Is.EqualTo("validuser"));
            Assert.That(result.Location, Is.EqualTo("Location1"));
            Assert.That(result.AvatarUrl, Is.EqualTo("http://example.com/avatar"));
            Assert.That(result.GitHubRepositories, Is.Null);
        }


        [Test]
        public void RetrieveUserWithRepositories_ShouldReturnGitHubUserWithoutRepos_WhenReposUrlIsEmpty()
        {
            // Arrange
            var username = "userwithnorepos";
            var url = "https://api.github.com/users/userwithnorepos";
            var jsonResponse = "{\"login\":\"userwithnorepos\",\"location\":\"Location2\",\"avatar_url\":\"http://example.com/avatar\",\"repos_url\":\"\"}";

            _mockEnvironmentVariables.Setup(x => x.GitHubUrl).Returns("https://api.github.com/users/");
            _mockWebClient.Setup(x => x.DownloadString(url)).Returns(jsonResponse);

            // Act
            var result = _gitHubService.RetrieveUserWithRepositories(username);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.UserName, Is.EqualTo("userwithnorepos"));
            Assert.That(result.Location, Is.EqualTo("Location2"));
            Assert.That(result.AvatarUrl, Is.EqualTo("http://example.com/avatar"));
            Assert.That(result.GitHubRepositories, Is.Null);
        }


        [Test]
        public void RetrieveUserWithRepositories_ShouldReturnGitHubUser_WhenUserFound()
        {
            // Arrange
            var username = "validuser";
            var url = "https://api.github.com/users/validuser";
            var jsonResponse = "{\"login\":\"validuser\",\"location\":\"Location1\",\"avatar_url\":\"http://example.com/avatar\",\"repos_url\":\"https://api.github.com/users/validuser/repos\"}";

            _mockEnvironmentVariables.Setup(x => x.GitHubUrl).Returns("https://api.github.com/users/");
            _mockWebClient.Setup(x => x.DownloadString(url)).Returns(jsonResponse);

            var userUrl = "https://api.github.com/users/validuser/repos";
            _mockWebClient.Setup(x => x.DownloadString(userUrl)).Returns("[{\"name\": \"BRepo1\", \"stargazers_count\": 5}, {\"name\": \"ARepo2\", \"stargazers_count\": 10}]");

            _mockEnvironmentVariables.Setup(x => x.NumberOfRepositories).Returns(5);

            // Act
            var result = _gitHubService.RetrieveUserWithRepositories(username);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.UserName, Is.EqualTo("validuser"));
            Assert.That(result.Location, Is.EqualTo("Location1"));
            Assert.That(result.AvatarUrl, Is.EqualTo("http://example.com/avatar"));
            Assert.That(result.GitHubRepositories, Has.Count.EqualTo(2));
            Assert.That(result.GitHubRepositories[0].RepositoryName, Is.EqualTo("ARepo2"));
            Assert.That(result.GitHubRepositories[1].RepositoryName, Is.EqualTo("BRepo1"));
        }

        [Test]
        public void RetrieveRepositories_ShouldThrowArgumentNullException_WhenUrlNotProvided()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => _gitHubService.RetrieveRepositories(null));
        }

        [Test]
        public void RetrieveRepositories_ShouldReturnNull_WhenJsonResultIsEmpty()
        {
            // Arrange
            var url = "https://api.github.com/users/someuser/repos";
            _mockWebClient.Setup(x => x.DownloadString(url)).Returns(string.Empty);

            // Act
            var result = _gitHubService.RetrieveRepositories(url);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void RetrieveRepositories_ShouldReturnListOfRepositories_WhenJsonResultIsValid()
        {
            // Arrange
            var url = "https://api.github.com/users/someuser/repos";
            var jsonResponse = "[{\"name\": \"Repo1\", \"stargazers_count\": 5}, {\"name\": \"Repo2\", \"stargazers_count\": 10}]";
            _mockWebClient.Setup(x => x.DownloadString(url)).Returns(jsonResponse);

            // Act
            var result = _gitHubService.RetrieveRepositories(url);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Has.Count.EqualTo(2));
            Assert.That(result[0].RepositoryName, Is.EqualTo("Repo1"));
            Assert.That(result[1].RepositoryName, Is.EqualTo("Repo2"));
        }

















        [Test]
        public void RetrieveUser_ShouldThrowArgumentNullException_WhenUserNotProvided()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => _gitHubService.RetrieveUser(null));
        }


        [Test]
        public void RetrieveUser_ShouldThrowWebException_WhenUserNotFound()
        {
            // Arrange
            var username = "nonexistentuser";
            var url = "https://api.github.com/users/nonexistentuser";
            _mockEnvironmentVariables.Setup(x => x.GitHubUrl).Returns("https://api.github.com/users/");
            _mockWebClient.Setup(x => x.DownloadString(url)).Throws(new HttpRequestException());

            // Assert
            Assert.Throws<HttpRequestException>(() => _gitHubService.RetrieveUser(username));
        }

        [Test]
        public void RetrieveUser_ShouldThrowJsonReaderException_WhenJsonInvalid()
        {
            // Arrange
            var username = "nonexistentuser";
            var url = "https://api.github.com/users/nonexistentuser";
            _mockEnvironmentVariables.Setup(x => x.GitHubUrl).Returns("https://api.github.com/users/");
            _mockWebClient.Setup(x => x.DownloadString(url)).Returns("InvalidJson");

            // Assert
            Assert.Throws<JsonReaderException>(() => _gitHubService.RetrieveUser(username));
        }

        [Test]
        public void RetrieveUser_ShouldReturnNull_WhenUserNotFound()
        {
            // Arrange
            var username = "nonexistentuser";
            var url = "https://api.github.com/users/nonexistentuser";
            _mockEnvironmentVariables.Setup(x => x.GitHubUrl).Returns("https://api.github.com/users/");
            _mockWebClient.Setup(x => x.DownloadString(url)).Returns(string.Empty);

            // Act
            var result = _gitHubService.RetrieveUser(username);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void RetrieveUser_ShouldReturnGitHubUser_WhenUserFound()
        {
            // Arrange
            var username = "validuser";
            var url = "https://api.github.com/users/validuser";
            var jsonResponse = "{\"login\":\"validuser\",\"location\":\"Location1\",\"avatar_url\":\"http://example.com/avatar\",\"repos_url\":\"https://api.github.com/users/validuser/repos\"}";

            _mockEnvironmentVariables.Setup(x => x.GitHubUrl).Returns("https://api.github.com/users/");
            _mockWebClient.Setup(x => x.DownloadString(url)).Returns(jsonResponse);

            _mockEnvironmentVariables.Setup(x => x.NumberOfRepositories).Returns(5);

            // Act
            var result = _gitHubService.RetrieveUser(username);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.UserName, Is.EqualTo("validuser"));
            Assert.That(result.Location, Is.EqualTo("Location1"));
            Assert.That(result.AvatarUrl, Is.EqualTo("http://example.com/avatar"));
            Assert.That(result.GitHubRepositories, Is.Null);
        }
    }
}
