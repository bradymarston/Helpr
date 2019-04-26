using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Helpr.Server.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShadySoft.EntityPersistence;
using ShadySoft.EntityPersistence.Extensions.Controller;

namespace Helpr.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IRepository<Post> _postRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PostsController(IRepository<Post> postRepository, IUnitOfWork unitOfWork)
        {
            _postRepository = postRepository;
            _unitOfWork = unitOfWork;
        }

        // GET: api/Posts
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var posts = await _postRepository.GetAsync();
            return Ok(posts);
        }

        // GET: api/Posts/5
        [HttpGet("{id}", Name = "Get")]
        [ServiceFilter(typeof(FindEntityFilter<Post>))]
        public IActionResult Get(int id)
        {
            return Ok(HttpContext.GetFoundEntity<Post>());
        }

        // POST: api/Posts
        [HttpPost]
        public async Task<IActionResult> Post(Post post)
        {
            _postRepository.Add(post);
            await _unitOfWork.CompleteAsync();

            return Ok(post);
        }

        // PUT: api/Posts/5
        [HttpPut("{id}")]
        [ServiceFilter(typeof(FindEntityFilter<Post>))]
        public async Task<IActionResult> Put(int id, Post post)
        {
            var postInDb = HttpContext.GetFoundEntity<Post>();
            postInDb.Content = post.Content;
            postInDb.LikeCount = post.LikeCount;
            postInDb.Title = post.Title;
            await _unitOfWork.CompleteAsync();

            return Ok(postInDb);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        [ServiceFilter(typeof(FindEntityFilter<Post>))]
        public async Task<IActionResult> Delete(int id)
        {
            var postInDb = HttpContext.GetFoundEntity<Post>();
            _postRepository.Remove(postInDb);
            await _unitOfWork.CompleteAsync();

            return Ok();
        }
    }
}
