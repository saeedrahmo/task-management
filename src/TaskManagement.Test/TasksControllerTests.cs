using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using TaskManagement.Areas.Management.Controllers;

namespace TaskManagement.Test
{
    public class TasksControllerTests
    {
        [Fact]
        public async Task Index_ReturnsViewResult_ForAuthorizedUser()
        {
            // Arrange
            //var user = new Core.Models.ApplicationUser { UserName = "admin" };

            var mockUserManager = new Mock<UserManager<Core.Models.ApplicationUser>>(Mock.Of<IUserStore<Core.Models.ApplicationUser>>(), null, null, null, null, null, null, null, null);
            var mockSignInManager = new Mock<SignInManager<Core.Models.ApplicationUser>>(mockUserManager.Object, Mock.Of<IHttpContextAccessor>(), Mock.Of<IUserClaimsPrincipalFactory<Core.Models.ApplicationUser>>(), null, null, null, null);
            

            mockSignInManager.Setup(sm => sm.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), false, false))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

            //var controller = new TasksController(mockUserManager.Object, mockSignInManager.Object);


            // Arrange
            //var orders = new List<Order> { /* initialize your test orders */ };

            //var mockDbContext = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
            //mockDbContext.Setup(db => db.Orders).ReturnsDbSet(orders);

            //var controller = new OrderController(mockDbContext.Object);

            //// Act
            //var result = controller.GetOrders();


            // Act
            //var result = await controller.Index();

            // Assert
            //Assert.IsType<ViewResult>(result);
        }
    }
}
