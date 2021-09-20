using MyBlog.Models;
using MyBlog.Models.Comments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Data.Repository
{
    public interface IRepository
    {
        Post GetPost(int id);
        List<Post> GetAllPosts();
        List<Post> GetAllPosts(string category);
        void AddPost(Post post);
        void UpdatePost(Post post);
        void RemovePost(int id);

        void AddSubComment(SubComment comment);
        Task<bool> SaveChangesAync();

    }
}
