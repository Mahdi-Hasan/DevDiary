using Microsoft.AspNetCore.Mvc;
using MyBlog.Data.FileManager;
using MyBlog.Data.Repository;
using MyBlog.Models;
using MyBlog.Models.Comments;
using MyBlog.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyBlog.Controllers
{
    public class HomeController : Controller
    {
        private IRepository _repo;
        private IFileManager _fileManager;

        public HomeController(IRepository repo, IFileManager fileManager)
        {
            _repo = repo;
            _fileManager = fileManager;
        }
        public IActionResult Index(string category)
        {
            var posts = string.IsNullOrEmpty(category) ? _repo.GetAllPosts() : _repo.GetAllPosts(category);
            return View(posts);
        }
        public IActionResult Post(int id)
        {
            var post = _repo.GetPost(id);
            return View(post);
        }
        [HttpGet("/Image/{image}")]
        public IActionResult Image(string image) =>
             new FileStreamResult(
                 _fileManager.ImageStream(image),
                 $"image/{image.Substring(image.LastIndexOf('.') + 1)}");

        [HttpPost]
        public async Task<IActionResult> Comment(CommentViewModel vm)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Post", new { id = vm.PostId });

            var post = _repo.GetPost(vm.PostId);
            if (vm.MainCommentId == 0)
            {
                post.MainComments = post.MainComments ?? new List<MainComment>();
                post.MainComments.Add(new MainComment
                {
                    Message = vm.Message,
                    Created = DateTime.Now,
                });

                _repo.UpdatePost(post);
            }
            else
            {
                var comment = new SubComment
                {
                    MainCommentId = vm.MainCommentId,
                    Message = vm.Message,
                    Created = DateTime.Now,
                };
                _repo.AddSubComment(comment);
            }
            await _repo.SaveChangesAync();
            return RedirectToAction("Post", new { id = vm.PostId });
        }
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null)
                return View(new Post());
            else
            {
                var post = _repo.GetPost((int)id);
                return View(post);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Post post)
        {
            if (post.Id > 0)
                _repo.UpdatePost(post);
            else
                _repo.AddPost(post);
            if (await _repo.SaveChangesAync())
                return RedirectToAction("Index");
            else
                return View(post);
        }
        [HttpGet]
        public async Task<IActionResult> Remove(int id)
        {
            _repo.RemovePost(id);
            await _repo.SaveChangesAync();
            return RedirectToAction("Index");
        }
    }
}
