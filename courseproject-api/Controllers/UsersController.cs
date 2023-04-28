using AutoMapper;
using courseproject_api.Dtos;
using courseproject_api.Interfaces;
using courseproject_api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace courseproject_api.Controllers
{
    [ApiController, Authorize]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UsersController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetUsers()
        {
            ICollection<User> users = _userRepository.GetUsers();

            ICollection<UserResponseDto> usersDto = _mapper.Map<ICollection<UserResponseDto>>(users);

            return Ok(usersDto);
        }
    }
}
