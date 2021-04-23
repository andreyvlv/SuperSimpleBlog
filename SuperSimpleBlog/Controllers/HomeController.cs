using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SuperSimpleBlog.Models;
using Microsoft.EntityFrameworkCore;
using SuperSimpleBlog.ViewModels;

namespace SuperSimpleBlog.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db;
        public HomeController(ApplicationDbContext context)
        {
            db = context;
        }

        public async Task<IActionResult> Index(int page = 1)
        {           
            int pageSize = 3;   // количество элементов на странице

            IQueryable<Post> source = db.Posts
                .Where(p => p.Public == true && p.Created <= DateTime.Now && p.Deleted == false)
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

        public async Task<IActionResult> Details(string title)
        {
            if (title != null)
            {
                Post post = await db.Posts.FirstOrDefaultAsync(p => p.Title == title);
                if (post != null)
                    return View(post);
            }
            return NotFound();
        }
    }
}
