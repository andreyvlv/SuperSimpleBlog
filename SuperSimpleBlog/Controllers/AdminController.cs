using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SuperSimpleBlog.Models;
using SuperSimpleBlog.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SuperSimpleBlog.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private ApplicationDbContext db;
        public AdminController(ApplicationDbContext context)
        {
            db = context;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            //var list = await db.Posts.OrderBy(p => p.Created).ToListAsync();
            //return View(list);

            int pageSize = 3;   // количество элементов на странице

            IQueryable<Post> source = db.Posts
                .OrderByDescending(p => p.Created);

            var count = await source.CountAsync();
            var items = await source.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            IndexViewModel viewModel = new IndexViewModel
            {
                PageViewModel = pageViewModel,
                Posts = items
            };
            return View(viewModel);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Post post)
        {
            post.Created = DateTime.Now;
            post.Updated = post.Created;
            post.Public = true;
            post.ID = Guid.NewGuid().ToString();
            db.Posts.Add(post);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id != null)
            {
                Post post = await db.Posts.FirstOrDefaultAsync(p => p.ID == id);
                if (post != null)
                    return View(post);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Post post)
        {
            post.Public = true;
            post.Updated = DateTime.Now;
            db.Posts.Update(post);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            if (id != null)
            {
                Post post = await db.Posts.FirstOrDefaultAsync(p => p.ID == id);
                if (post != null)
                {
                    db.Posts.Remove(post);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            return NotFound();
        }
    }
}
