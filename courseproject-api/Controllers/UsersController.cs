using AutoMapper;
using courseproject_api.Dtos;
using courseproject_api.Interfaces;
using courseproject_api.Models;
using courseproject_api.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace courseproject_api.Controllers
{
    [ApiController, Authorize]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;

        public UsersController(IUserRepository userRepository, IPostRepository postRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _postRepository = postRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetUsers()
        {
            var users = _userRepository.GetUsers();

            var usersDto = _mapper.Map<ICollection<UserDto>>(users);

            return Ok(usersDto);
        }

        [HttpGet("{id}")]
        public IActionResult GetUser(int id)
        {
            if (!_userRepository.UserExists(id))
            { 
                return NotFound("User not found.");
            }

            var user = _userRepository.GetUser(id);

            var userDto = _mapper.Map<UserDto>(user);

            return Ok(userDto);
        }
        
        [HttpGet("{id}/posts")]
        public IActionResult GetUserPosts(int id)
        {
            if (!_userRepository.UserExists(id))
            {
                return NotFound("User not found.");
            }

            var postsAndUsers = _postRepository.GetPosts(id);

            var posts = new List<PostDto>();

            foreach (var keyValuePair in postsAndUsers)
            {
                var post = _mapper.Map<PostDto>(keyValuePair.Value);

                post.AuthorId = keyValuePair.Key.Id;
                post.AuthorUsername = keyValuePair.Key.Username;
                post.LikeCount = _postRepository.GetPostLikeCount(keyValuePair.Value.Id);
                post.ShareCount = _postRepository.GetPostShareCount(keyValuePair.Value.Id);
                post.CommentCount = _postRepository.GetPostCommentCount(keyValuePair.Value.Id);
                post.ReportCount = _postRepository.GetPostReportCount(keyValuePair.Value.Id);

                posts.Add(post);
            }

            return Ok(posts);
        }

        [HttpGet("me")]
        public IActionResult GetCurrentUser()
        {
            var identity = (ClaimsIdentity)User.Identity;
            var email = identity.Claims
                .Where(c => c.Type == ClaimTypes.Email)
                .Select(c => c.Value)
                .FirstOrDefault();

            if (!_userRepository.UserExists(email))
            {
                return NotFound("User not found.");
            }

            var user = _userRepository.GetUser(email);

            var userDto = _mapper.Map<UserDto>(user);

            return Ok(userDto);
        }

        [HttpGet("me/subscriptions")]
        public IActionResult GetCurrentUserSubscriptions()
        {
            var identity = (ClaimsIdentity)User.Identity;
            var email = identity.Claims
                .Where(c => c.Type == ClaimTypes.Email)
                .Select(c => c.Value)
                .FirstOrDefault();
            var id = identity.Claims
                .Where(c => c.Type == ClaimTypes.NameIdentifier)
                .Select(c => c.Value)
                .FirstOrDefault();

            if (!_userRepository.UserExists(email))
            {
                return NotFound("User not found.");
            }

            var subscriptions = 
                _mapper.Map<ICollection<UserDto>>(
                    _userRepository.GetUserSubscriptions(Convert.ToInt32(id)));

            return Ok(subscriptions);
        }

        [HttpPost("me/subscriptions")]
        public IActionResult AddSubscription(SubscriptionDto request)
        {
            var identity = (ClaimsIdentity)User.Identity;
            var email = identity.Claims
                .Where(c => c.Type == ClaimTypes.Email)
                .Select(c => c.Value)
                .FirstOrDefault();
            var id = identity.Claims
                .Where(c => c.Type == ClaimTypes.NameIdentifier)
                .Select(c => c.Value)
                .FirstOrDefault();

            if (!_userRepository.UserExists(email))
            {
                return NotFound("User not found.");
            }

            if (!_userRepository.UserExists(request.PublisherId))
            {
                return NotFound("User not found.");
            }

            if (Convert.ToInt32(id) == request.PublisherId)
            {
                return BadRequest("Can't subscribe to yourself.");
            }

            _userRepository.AddUserSubscription(Convert.ToInt32(id), request.PublisherId);

            var subscriptions =
                _mapper.Map<ICollection<UserDto>>(
                    _userRepository.GetUserSubscriptions(Convert.ToInt32(id)));

            return Ok(subscriptions);
        }

        [HttpDelete("me/subscriptions")]
        public IActionResult RemoveSubscription(SubscriptionDto request)
        {
            var identity = (ClaimsIdentity)User.Identity;
            var email = identity.Claims
                .Where(c => c.Type == ClaimTypes.Email)
                .Select(c => c.Value)
                .FirstOrDefault();
            var id = identity.Claims
                .Where(c => c.Type == ClaimTypes.NameIdentifier)
                .Select(c => c.Value)
                .FirstOrDefault();

            if (!_userRepository.UserExists(email))
            {
                return NotFound("User not found.");
            }

            if (!_userRepository.UserExists(request.PublisherId))
            {
                return NotFound("User not found.");
            }

            if (Convert.ToInt32(id) == request.PublisherId)
            {
                return BadRequest("Can't subscribe to yourself.");
            }

            _userRepository.RemoveUserSubscription(Convert.ToInt32(id), request.PublisherId);

            var subscriptions =
                _mapper.Map<ICollection<UserDto>>(
                    _userRepository.GetUserSubscriptions(Convert.ToInt32(id)));

            return Ok(subscriptions);
        }

        [HttpPut("me/update")]
        public IActionResult UpdateCurrentUser(UserDto request)
        {
            var identity = (ClaimsIdentity)User.Identity;
            var email = identity.Claims
                .Where(c => c.Type == ClaimTypes.Email)
                .Select(c => c.Value)
                .FirstOrDefault();
            var id = identity.Claims
                .Where(c => c.Type == ClaimTypes.NameIdentifier)
                .Select(c => c.Value)
                .FirstOrDefault();

            if (!_userRepository.UserExists(email))
            {
                return NotFound("User not found.");
            }

            request.Id = Convert.ToInt32(id);

            var user = _userRepository.UpdateUser(request);

            var userDto = _mapper.Map<UserDto>(user);

            return Ok(userDto);
        }

        [HttpPut("{id}/status"), Authorize(Roles = "ADMIN,MODERATOR")]
        public IActionResult ChangeUserStatus(int id, StatusDto request) 
        {
            if (!_userRepository.UserExists(id))
            {
                return NotFound("User not found.");
            }

            var user = _userRepository.ChangeUserStatus(id, request.Status);

            var userDto = _mapper.Map<UserDto>(user);

            return Ok(userDto);
        }

        [HttpDelete("{id}"), Authorize(Roles = "ADMIN,MODERATOR")]
        public IActionResult DeleteUser(int id)
        {
            if (!_userRepository.UserExists(id))
            {
                return NotFound("User not found.");
            }

            _userRepository.DeleteUser(id);

            return NoContent();
        }
    }
}
