using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Moq;
using System.Security.Claims;
using TaskManagement.Areas.Identity.Controllers;
using TaskManagement.Areas.Management.Controllers;
using TaskManagement.Controllers;
using TaskManagement.Core.DTOs;
using TaskManagement.Core.Mapper;
using TaskManagement.Data.RepositoryManager;
using TaskManagement.Services.IService;
using Xunit;

namespace TaskManagement.Tests
{
    public class TasksControllerTests
    {
        private readonly Mock<ITaskService> _mockTaskService;
        private readonly Mock<IUserService> _mockUserService;
        private readonly Mock<ILogger<TasksController>> _mockLogger;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly IMapper _mapper;

        public TasksControllerTests()
        {
            _mockTaskService = new Mock<ITaskService>();
            _mockUserService = new Mock<IUserService>();
            _mockLogger = new Mock<ILogger<TasksController>>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();

            // Initialize AutoMapper
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });
            _mapper = mapperConfig.CreateMapper();
        }

        [Fact]
        public async Task Index_ReturnsViewResult()
        {
            // Arrange
            var controller = new TasksController(_mockLogger.Object, _mockUnitOfWork.Object, _mapper, _mockTaskService.Object, _mockUserService.Object);

            // Mock the task service
            var searchQuery = "test";
            var sortOrder = "title";
            var pageNumber = 1;
            var pageSize = 3;

            var taskList = new List<Core.Models.Task>
        {
            new Core.Models.Task { Id = 1, Title = "Task 1",Description="Desc",DueDate=new DateTime(2000,05,20).Date, Priority=Core.Enums.Priority.High,Status=Core.Enums.Status.Completed},
            new Core.Models.Task { Id = 1, Title = "Task 2",Description="Desc",DueDate=new DateTime(2000,05,20).Date, Priority=Core.Enums.Priority.High,Status=Core.Enums.Status.Completed},
            new Core.Models.Task { Id = 1, Title = "Task 3",Description="Desc",DueDate=new DateTime(2000,05,20).Date, Priority=Core.Enums.Priority.High,Status=Core.Enums.Status.Completed}
        };

            //var pagedResult = new PagedResult<TaskModel>(taskList, taskList.Count, pageNumber, pageSize);

            _mockTaskService.Setup(x => x.GetPagedTaskList(searchQuery, sortOrder, pageNumber, pageSize))
                .ReturnsAsync((Items:taskList,Count:taskList.Count));         

            // Act
            var result = await controller.Index(searchQuery, sortOrder, pageNumber, pageSize) as ViewResult;//ViewResult

            // Assert
            Assert.NotNull(result);
            //Assert.Equal("Index", result.ViewName);

            var model = result.Model as List<PagedTaskListDto>;
            Assert.NotNull(model);
            Assert.Equal(taskList.Count, model.Count);
        }
    }

}
