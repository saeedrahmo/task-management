using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using TaskManagement.Areas.Identity.Controllers;
using TaskManagement.Areas.Identity.Models;



namespace TaskManagement.Test
{
    public class AccountControllerTests
    {
        [Fact]
        public async Task Login_ValidCredentials_ReturnsRedirectToHome()
        {
            // Arrange
            var mockUserStore = new Mock<IUserStore<Core.Models.ApplicationUser>>(MockBehavior.Strict);
            var mockUserManager = new Mock<UserManager<Core.Models.ApplicationUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            mockUserManager.Setup(um => um.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(new Core.Models.ApplicationUser { UserName = "testuser" });
            mockUserManager.Setup(um => um.CheckPasswordAsync(It.IsAny<Core.Models.ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var mockClaimsFactory = new Mock<IUserClaimsPrincipalFactory<Core.Models.ApplicationUser>>();
            var mockIdentityOptions = new Mock<IOptions<IdentityOptions>>();
            var mockLogger = new Mock<ILogger<SignInManager<Core.Models.ApplicationUser>>>();
            var mockAuthenticationSchemeProvider = new Mock<IAuthenticationSchemeProvider>();

            var mockSignInManager = new Mock<SignInManager<Core.Models.ApplicationUser>>(
                mockUserManager.Object,
                mockHttpContextAccessor.Object,
                mockClaimsFactory.Object,
                mockIdentityOptions.Object,
                mockLogger.Object,
                mockAuthenticationSchemeProvider.Object);

            var controller = new AccountController( mockSignInManager.Object, mockUserManager.Object);

            // Act
            var result = await controller.Login(new LoginViewModel { UserName = "testuser", Password = "password" });

            // Assert
            Assert.IsType<RedirectResult>(result);
            Assert.Equal("/", ((RedirectResult)result).Url);
        }
    }
}
