using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using TaskManagement.Areas.Identity.Controllers;
using TaskManagement.Areas.Identity.Models;
using Xunit;

namespace TaskManagement.Tests
{
    public class AccountControllerTests
    {
        private readonly Mock<SignInManager<Core.Models.ApplicationUser>> _mockSignInManager;
        private readonly Mock<UserManager<Core.Models.ApplicationUser>> _mockUserManager;
        private readonly Mock<ILogger<AccountController>> _mockLogger;

        public AccountControllerTests()
        {
            //_mockSignInManager = new Mock<SignInManager<Core.Models.ApplicationUser>>(Mock.Of<UserManager<Core.Models.ApplicationUser>>(),
            //    new Mock<IHttpContextAccessor>().Object,
            //    new Mock<IUserClaimsPrincipalFactory<Core.Models.ApplicationUser>>().Object,
            //    null,
            //    null,
            //    null,
            //    null);

            //_mockUserManager = new Mock<UserManager<Core.Models.ApplicationUser>>(Mock.Of<IUserStore<Core.Models.ApplicationUser>>(), null, null, null, null, null, null, null, null);

            _mockUserManager = new Mock<UserManager<Core.Models.ApplicationUser>>(
    /* IUserStore<TUser> store */Mock.Of<IUserStore<Core.Models.ApplicationUser>>(),
    /* IOptions<IdentityOptions> optionsAccessor */null,
    /* IPasswordHasher<TUser> passwordHasher */null,
    /* IEnumerable<IUserValidator<TUser>> userValidators */null,
    /* IEnumerable<IPasswordValidator<TUser>> passwordValidators */null,
    /* ILookupNormalizer keyNormalizer */null,
    /* IdentityErrorDescriber errors */null,
    /* IServiceProvider services */null,
    /* ILogger<UserManager<TUser>> logger */null);

            _mockSignInManager = new Mock<SignInManager<Core.Models.ApplicationUser>>(
                _mockUserManager.Object,
                /* IHttpContextAccessor contextAccessor */Mock.Of<IHttpContextAccessor>(),
                /* IUserClaimsPrincipalFactory<TUser> claimsFactory */Mock.Of<IUserClaimsPrincipalFactory<Core.Models.ApplicationUser>>(),
                /* IOptions<IdentityOptions> optionsAccessor */null,
                /* ILogger<SignInManager<TUser>> logger */null,
                /* IAuthenticationSchemeProvider schemes */null,
                /* IUserConfirmation<TUser> confirmation */null);

            _mockLogger = new Mock<ILogger<AccountController>>();
        }

        [Fact]
        public async Task Login_ValidCredentials_ReturnsRedirectToHome()
        {
            // Arrange
            //var mockHttpContext = new Mock<HttpContext>();
            //var mockHttpResponse = new Mock<HttpResponse>();

            // Set up HttpContext mocks
            //mockHttpContext.SetupGet(x => x.Response).Returns(mockHttpResponse.Object);
            //mockHttpContext.Setup(x => x.SignOutAsync(IdentityConstants.ExternalScheme)).Returns(Task.CompletedTask);          

            var controller = new AccountController(_mockSignInManager.Object, _mockUserManager.Object, _mockLogger.Object);
            var returnUrl = ""; // Set the desired return URL

            var loginViewModel = new LoginViewModel
            {
                UserName = "testuser",
                Password = "password",
                RememberMe = false                
            };

            _mockSignInManager.Setup(x => x.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

            //var mockHttpContext = new Mock<HttpContext>();
            //var mockHttpResponse = new Mock<HttpResponse>();

            //mockHttpContext.SetupGet(x => x.Response).Returns(mockHttpResponse.Object);
            //mockHttpContext.Setup(x => x.SignOutAsync(IdentityConstants.ExternalScheme))
            //    .Returns(Task.CompletedTask);

            //controller.ControllerContext = new ControllerContext
            //{
            //    HttpContext = mockHttpContext.Object
            //};

            // Act
            var result = await controller.Login(loginViewModel, returnUrl) as RedirectToActionResult; //RedirectToActionResult

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            Assert.Equal("Home", result.ControllerName);
            //Assert.Null(result.RouteValues["area"]);
        }

        // Add more test methods for different scenarios
    }

}
