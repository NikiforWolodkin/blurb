using AutoMapper;
using courseproject_api.Dtos;
using courseproject_api.Interfaces;
using courseproject_api.Models;
using courseproject_api.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Security.Claims;

namespace courseproject_api.Controllers
{
    [ApiController, Authorize]
    [Route("blurb-api/[controller]")]
    public class PostsController : Controller
    {
        private readonly IPostRepository _postRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public PostsController(IPostRepository postRepository, IUserRepository userRepository,IMapper mapper)
        {
            _postRepository = postRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpGet("/blurb-api/stats")]
        public IActionResult GetStats() 
        {
            return Ok(_postRepository.GetStats());
        }

        [HttpGet]
        public IActionResult GetPosts(string search)
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

            var postsAndUsers = _postRepository
                .GetPosts()
                .Where(k => k.Value.Text.Contains(search) || k.Value.Tags.Select(t => t.Text).Contains(search));

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
                post.IsLiked = _postRepository.IsPostLiked(keyValuePair.Value.Id, Convert.ToInt32(userId));

                posts.Add(post);
            }

            return Ok(posts);
        }

        [HttpGet("reported")]
        public IActionResult GetReportedPosts()
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

            var postsAndUsers = _postRepository.GetReportedPosts();

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
                post.IsLiked = _postRepository.IsPostLiked(keyValuePair.Value.Id, Convert.ToInt32(userId));

                posts.Add(post);
            }

            return Ok(posts);
        }

        [HttpGet("trending"), Authorize]
        public IActionResult GetTrendingPosts(int? page)
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

            if (page is null)
            {
                page = 0;
            }

            var postsAndUsers = _postRepository.GetTrendingPosts((int)page);

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
                post.IsLiked = _postRepository.IsPostLiked(keyValuePair.Value.Id, Convert.ToInt32(userId));

                posts.Add(post);
            }

            return Ok(posts);
        }

        [HttpGet("{id}"), Authorize]
        public IActionResult GetPost(int id)
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

            if (!_postRepository.PostExists(id))
            {
                return NotFound("Post not found.");
            }

            var postAndUser = _postRepository.GetPost(id);

            var post = _mapper.Map<PostDto>(postAndUser.Value);

            post.AuthorId = postAndUser.Key.Id;
            post.AuthorUsername = postAndUser.Key.Username;
            post.AuthorAvatar = postAndUser.Key.Avatar;
            post.AuthorProfileColor = postAndUser.Key.ProfileColor;
            post.AuthorStatus = postAndUser.Key.Status;
            post.Tags = postAndUser.Value.Tags.Select(t => t.Text).ToList();
            post.LikeCount = _postRepository.GetPostLikeCount(postAndUser.Value.Id);
            post.ShareCount = _postRepository.GetPostShareCount(postAndUser.Value.Id);
            post.CommentCount = _postRepository.GetPostCommentCount(postAndUser.Value.Id);
            post.ReportCount = _postRepository.GetPostReportCount(postAndUser.Value.Id);
            post.IsLiked = _postRepository.IsPostLiked(postAndUser.Value.Id, Convert.ToInt32(userId));

            return Ok(post);
        }

        [HttpPost("{id}/like")]
        public IActionResult AddLikeToPost(int id)
        {
            if (!_postRepository.PostExists(id))
            {
                return NotFound("Post not found.");
            }

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

            _postRepository.AddLikeToPost(id, Convert.ToInt32(userId));

            return Ok(_postRepository.GetPostLikeCount(id));
        }

        [HttpDelete("{id}/like")]
        public IActionResult RemoveLikeFromPost(int id)
        {
            if (!_postRepository.PostExists(id))
            {
                return NotFound("Post not found.");
            }

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

            _postRepository.RemoveLikeFromPost(id, Convert.ToInt32(userId));

            return Ok(_postRepository.GetPostLikeCount(id));
        }

        [HttpPost("{id}/share")]
        public IActionResult AddShareToPost(int id)
        {
            if (!_postRepository.PostExists(id))
            {
                return NotFound("Post not found.");
            }

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

            _postRepository.AddShareToPost(id, Convert.ToInt32(userId));

            return Ok(_postRepository.GetPostShareCount(id));
        }

        [HttpPost("{id}/report")]
        public IActionResult AddReportToPost(int id)
        {
            if (!_postRepository.PostExists(id))
            {
                return NotFound("Post not found.");
            }

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

            _postRepository.AddReportToPost(id, Convert.ToInt32(userId));

            return Ok(_postRepository.GetPostReportCount(id));
        }

        [HttpDelete("{id}/reports")]
        public IActionResult DeleteReports(int id)
        {
            _postRepository.DeleteReports(id);

            return NoContent();
        }

        [HttpGet("{id}/comments")]
        public IActionResult GetComments(int id)
        {
            if (!_postRepository.PostExists(id))
            {
                return NotFound("Post not found.");
            }

            var comments = _postRepository.GetPostComments(id).ToList();

            var commentsDto = new List<CommentDto>();

            for (int i = 0; i < comments.Count; i++)
            {
                commentsDto.Add(_mapper.Map<CommentDto>(comments[i]));

                commentsDto[i].AuthorId = comments[i].User.Id;
                commentsDto[i].AuthorUsername = comments[i].User.Username;
                commentsDto[i].AuthorProfileColor = comments[i].User.ProfileColor;
                commentsDto[i].AuthorAvatar = comments[i].User.Avatar;
                commentsDto[i].AuthorStatus = comments[i].User.Status;
            }

            return Ok(commentsDto);
        }

        [HttpPost("{id}/comments")]
        public IActionResult AddComment(int id, CommentDto request)
        {
            if (!_postRepository.PostExists(id))
            {
                return NotFound("Post not found.");
            }

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

            if (_userRepository.GetUser(Convert.ToInt32(userId)).Status == "BANNED")
            {
                return BadRequest("You are blocked and can't create comments.");
            }

            request.AuthorId = Convert.ToInt32(userId);
            request.PostId = id;

            var comment = _postRepository.AddCommentToPost(id, request);

            var commentDto = _mapper.Map<CommentDto>(comment);

            commentDto.AuthorId = comment.User.Id;
            commentDto.AuthorUsername = comment.User.Username;
            commentDto.AuthorProfileColor = comment.User.ProfileColor;
            commentDto.AuthorAvatar = comment.User.Avatar;

            return Ok(commentDto);
        }

        [HttpPost]
        public IActionResult AddPost(PostDto request)
        {
            if (request.Text is null)
            {
                return BadRequest("Text is required.");
            }

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
                return BadRequest("You are blocked and can't create posts.");
            }

            request.AuthorId = Convert.ToInt32(id);

            var postAndUser = _postRepository.AddPost(request);

            var post = _mapper.Map<PostDto>(postAndUser.Value);

            post.AuthorId = postAndUser.Key.Id;
            post.AuthorUsername = postAndUser.Key.Username;
            post.LikeCount = _postRepository.GetPostLikeCount(postAndUser.Value.Id);
            post.ShareCount = _postRepository.GetPostShareCount(postAndUser.Value.Id);
            post.CommentCount = _postRepository.GetPostCommentCount(postAndUser.Value.Id);
            post.ReportCount = _postRepository.GetPostReportCount(postAndUser.Value.Id);

            return Created($"api/posts/{post.Id}", post);
        }

        [HttpDelete("{id}"), Authorize(Roles = "ADMIN,MODERATOR")]
        public IActionResult DeletePost(int id)
        {
            if (!_postRepository.PostExists(id))
            {
                return NotFound("Post not found.");
            }

            _postRepository.DeletePost(id);

            return NoContent();
        }
    }
}
