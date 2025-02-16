using AccreditSolutionsShared.Classes.Abstract;
using AccreditSolutionsShared.Constants;
using AccreditSolutions.Models.ViewModels;
using Service.Abstract;
using System.Web.Mvc;
using AccreditSolutions.Models.Factories;
using System.Net.Http;

namespace AccreditSolutions.Controllers
{
    public class GitHubUserController : Controller
    {
        private IUsernameValidator _iUserNameValidator;
        private IGitHubService _iGitHubService;

        public GitHubUserController(IUsernameValidator iUserNameValidator, IGitHubService iGitHubService)
        {
            _iUserNameValidator = iUserNameValidator;
            _iGitHubService = iGitHubService;
        }

        public ActionResult RetrieveGitHubUser()
        {
            ModelState.Clear();
            return View();
        }

        [HttpPost]
        public ActionResult RetrieveGitHubUser(RetrieveGitHubUserViewModel model)
        {
            try
            {
                if (_iUserNameValidator.IsValid(model.Username))
                {
                    var validationMessage = _iUserNameValidator.GetValidationMessage(model.Username);

                    if (string.IsNullOrWhiteSpace(validationMessage))
                    {
                        ModelState.AddModelError("Message", UserMessages.InvalidUsernameMessage);
                        return View("RetrieveGitHubUser", model);
                    }

                    ModelState.AddModelError("Message", validationMessage);
                    return View("RetrieveGitHubUser", model);
                }

                var gitHubUser = _iGitHubService.RetrieveUser(model.Username);

                if (gitHubUser == null)
                {
                    ModelState.AddModelError("Message", UserMessages.UserNotFoundMessage);
                    return View("RetrieveGitHubUser", model);
                }

                var displayGitHubUserViewModel = new GitHubUserResultViewModelFactory(gitHubUser).Create();

                return View("GitHubUserResults", displayGitHubUserViewModel);
            }
            catch (HttpRequestException ex)
            {
                if (ex.Message.Contains("403"))
                {
                    ModelState.AddModelError("Message", UserMessages.GitHubEndpointForbidden);
                    return View("RetrieveGitHubUser", model);
                }

                if (ex.Message.Contains("404"))
                {
                    ModelState.AddModelError("Message", UserMessages.UserNotFoundMessage);
                    return View("RetrieveGitHubUser", model);
                }

                ModelState.AddModelError("Message", ex.Message);
                return View("RetrieveGitHubUser", model);
            }
        }
    }
}