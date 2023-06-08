using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Data.RepositoryManager;

namespace TaskManagement.Controllers
{
    public class BaseController<T> : Controller
    {
        public readonly ILogger<T> _logger;
        public readonly IUnitOfWork _uow;
        public readonly IMapper _mapper;

        public BaseController(ILogger<T> logger, IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
        }
    }
}
