using AutoMapper;
using courseproject_api.Dtos;
using courseproject_api.Interfaces;
using courseproject_api.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;

namespace courseproject_api.Controllers
{
    [ApiController, Authorize]
    [Route("api/[controller]")]
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

        [HttpGet]
        public IActionResult GetPosts()
        {
            var postsAndUsers = _postRepository.GetPosts();

            var posts = new List<PostDto>();

            foreach (var keyValuePair in postsAndUsers)
            {
                var post = _mapper.Map<PostDto>(keyValuePair.Value);

                post.AuthorId = keyValuePair.Key.Id;
                post.AuthorUsername = keyValuePair.Key.Username;
                post.LikeCount = _postRepository.GetPostLikeCount(keyValuePair.Value.Id);
                post.ShareCount= _postRepository.GetPostShareCount(keyValuePair.Value.Id);
                post.CommentCount = _postRepository.GetPostCommentCount(keyValuePair.Value.Id);
                post.ReportCount = _postRepository.GetPostReportCount(keyValuePair.Value.Id);

                posts.Add(post);
            }

            return Ok(posts);
        }

        [HttpGet("{id}")]
        public IActionResult GetPost(int id)
        {
            if (!_postRepository.PostExists(id))
            {
                return NotFound("Post not found.");
            }

            var postAndUser = _postRepository.GetPost(id);

            var post = _mapper.Map<PostDto>(postAndUser.Value);

            post.AuthorId = postAndUser.Key.Id;
            post.AuthorUsername = postAndUser.Key.Username;
            post.LikeCount = _postRepository.GetPostLikeCount(postAndUser.Value.Id);
            post.ShareCount = _postRepository.GetPostShareCount(postAndUser.Value.Id);
            post.CommentCount = _postRepository.GetPostCommentCount(postAndUser.Value.Id);
            post.ReportCount = _postRepository.GetPostReportCount(postAndUser.Value.Id);

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

            request.AuthorId = Convert.ToInt32(userId);
            request.PostId = id;

            var comment = _postRepository.AddCommentToPost(id, request);

            var commentDto = _mapper.Map<CommentDto>(comment);

            commentDto.AuthorId = comment.User.Id;
            commentDto.AuthorUsername = comment.User.Username;

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
