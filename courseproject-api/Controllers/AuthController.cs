using AutoMapper;
using courseproject_api.Dtos;
using courseproject_api.Interfaces;
using courseproject_api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace courseproject_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public AuthController(IUserRepository userRepository, IConfiguration configuration, IMapper mapper)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _mapper = mapper;
        }

        [HttpPost("signup")]
        public IActionResult SignUp(UserRequestDto request)
        {
            if (_userRepository.UserExists(request.Email))
            {
                return BadRequest("User with this E-Mail already exists.");
            }

            if (request.Username is null)
            {
                return BadRequest("Username is required.");
            }

            var user = _userRepository.AddUser(request);

            var response = _mapper.Map<UserDto>(user);

            return Created($"api/users/{response.Id}", response);
        }

        [HttpPost("login")]
        public IActionResult LogIn(UserRequestDto request)
        {
            if (!_userRepository.UserExists(request.Email))
            {
                return BadRequest("User with this E-Mail doesn't exist.");
            }

            var user = _userRepository.GetUser(request.Email);

            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return BadRequest("Incorrect password.");
            }

            var token = CreateToken(user);

            return Ok(token);
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role),
            }; 

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("Jwt:Token").Value    
            ));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: credentials,
                expires: DateTime.Now.AddDays(1)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
