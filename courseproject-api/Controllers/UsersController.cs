using AutoMapper;
using courseproject_api.Dtos;
using courseproject_api.Interfaces;
using courseproject_api.Models;
using courseproject_api.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace courseproject_api.Controllers
{
    [ApiController, Authorize]
    [Route("blurb-api/[controller]")]
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
        public IActionResult GetUsers(string? search)
        {
            var identity = (ClaimsIdentity)User.Identity;
            var id = identity.Claims
                .Where(c => c.Type == ClaimTypes.NameIdentifier)
                .Select(c => c.Value)
                .FirstOrDefault();

            var users = _userRepository.GetUsers();

            var usersDto = _mapper.Map<ICollection<UserDto>>(users);

            if (!search.IsNullOrEmpty())
            {
                usersDto = usersDto.Where(u => u.Username.Contains(search)).ToList();
            }

            var currentUser = _userRepository.GetUser(Convert.ToInt32(id));

            foreach (var user in usersDto)
            {
                user.IsSubscribed = _userRepository.IsUserSubscribed(currentUser.Id, (int)user.Id);
                if (currentUser.Role == "ADMIN" || currentUser.Role == "MODERATOR")
                {
                    user.CanBan = true;
                }
                else
                {
                    user.CanBan = false;    
                }
            }

            return Ok(usersDto);
        }

        [HttpGet("{id}"), Authorize]
        public IActionResult GetUser(int id)
        {
            var identity = (ClaimsIdentity)User.Identity;
            var userId = identity.Claims
                .Where(c => c.Type == ClaimTypes.NameIdentifier)
                .Select(c => c.Value)
                .FirstOrDefault();

            if (!_userRepository.UserExists(Convert.ToInt32(userId)))
            { 
                return NotFound("User not found.");
            }

            var user = _userRepository.GetUser(id);

            var userDto = _mapper.Map<UserDto>(user);

            userDto.IsSubscribed = _userRepository.IsUserSubscribed(Convert.ToInt32(userId), (int)user.Id);

            return Ok(userDto);
        }
        
        [HttpGet("{id}/posts"), Authorize]
        public IActionResult GetUserPosts(int id)
        {
            var identity = (ClaimsIdentity)User.Identity;
            var email = identity.Claims
                .Where(c => c.Type == ClaimTypes.Email)
                .Select(c => c.Value)
                .FirstOrDefault();
            var userId = identity.Claims
                .Where(c => c.Type == ClaimTypes.NameIdentifier)
                .Select(c => c.Value)
                .FirstOrDefault();

            if (!_userRepository.UserExists(email))
            {
                return NotFound("User not found.");
            }

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
                post.AuthorAvatar = keyValuePair.Key.Avatar;
                post.AuthorProfileColor = keyValuePair.Key.ProfileColor;
                post.LikeCount = _postRepository.GetPostLikeCount(keyValuePair.Value.Id);
                post.ShareCount = _postRepository.GetPostShareCount(keyValuePair.Value.Id);
                post.CommentCount = _postRepository.GetPostCommentCount(keyValuePair.Value.Id);
                post.ReportCount = _postRepository.GetPostReportCount(keyValuePair.Value.Id);
                post.IsLiked = _postRepository.IsPostLiked(keyValuePair.Value.Id, Convert.ToInt32(userId));

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

            var currentUser = _userRepository.GetUser(Convert.ToInt32(id));

            foreach (var user in subscriptions)
            {
                user.IsSubscribed = _userRepository.IsUserSubscribed(currentUser.Id, (int)user.Id);
                if (currentUser.Role == "ADMIN" || currentUser.Role == "MODERATOR")
                {
                    user.CanBan = true;
                }
                else
                {
                    user.CanBan = false;
                }
            }

            return Ok(subscriptions);
        }

        [HttpGet("me/posts"), Authorize]
        public IActionResult GetCurrentUserPosts()
        {
            var identity = (ClaimsIdentity)User.Identity;
            var id = identity.Claims
                .Where(c => c.Type == ClaimTypes.NameIdentifier)
                .Select(c => c.Value)
                .FirstOrDefault();

            if (!_userRepository.UserExists(Convert.ToInt32(id)))
            {
                return NotFound("User not found.");
            }

            var postsAndUsers = _postRepository.GetPosts(Convert.ToInt32(id));

            var posts = new List<PostDto>();

            foreach (var keyValuePair in postsAndUsers)
            {
                var post = _mapper.Map<PostDto>(keyValuePair.Value);

                post.AuthorId = keyValuePair.Key.Id;
                post.AuthorUsername = keyValuePair.Key.Username;
                post.AuthorProfileColor = keyValuePair.Key.ProfileColor;
                post.AuthorAvatar = keyValuePair.Key.Avatar; 
                post.AuthorStatus = keyValuePair.Key.Status;
                post.Tags = keyValuePair.Value.Tags.Select(t => t.Text).ToList();
                post.LikeCount = _postRepository.GetPostLikeCount(keyValuePair.Value.Id);
                post.ShareCount = _postRepository.GetPostShareCount(keyValuePair.Value.Id);
                post.CommentCount = _postRepository.GetPostCommentCount(keyValuePair.Value.Id);
                post.ReportCount = _postRepository.GetPostReportCount(keyValuePair.Value.Id);
                post.IsLiked = _postRepository.IsPostLiked(keyValuePair.Value.Id, keyValuePair.Key.Id);

                posts.Add(post);
            }

            return Ok(posts);
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

        [HttpGet("me/subscriptions/posts"), Authorize]
        public IActionResult GetSubscriptionPosts(int? page)
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

            if (page is null)
            {
                page = 0;
            }

            var postsAndUsers = _postRepository.GetSubscriptionPosts((int)page, Convert.ToInt32(id));

            var posts = new List<PostDto>();

            foreach (var keyValuePair in postsAndUsers)
            {
                var post = _mapper.Map<PostDto>(keyValuePair.Value);

                post.AuthorId = keyValuePair.Key.Id;
                post.AuthorUsername = keyValuePair.Key.Username;
                post.AuthorAvatar = keyValuePair.Key.Avatar;
                post.AuthorProfileColor = keyValuePair.Key.ProfileColor;
                post.AuthorStatus = keyValuePair.Key.Status;
                post.Tags = keyValuePair.Value.Tags.Select(t => t.Text).ToList();
                post.LikeCount = _postRepository.GetPostLikeCount(keyValuePair.Value.Id);
                post.ShareCount = _postRepository.GetPostShareCount(keyValuePair.Value.Id);
                post.CommentCount = _postRepository.GetPostCommentCount(keyValuePair.Value.Id);
                post.ReportCount = _postRepository.GetPostReportCount(keyValuePair.Value.Id);
                post.IsLiked = _postRepository.IsPostLiked(keyValuePair.Value.Id, Convert.ToInt32(id));

                posts.Add(post);
            }

            return Ok(posts);
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

            if (_userRepository.GetUser(Convert.ToInt32(id)).Status == "BANNED")
            {
                return BadRequest("You are blocked and can't change your profile.");
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
