using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuperSimpleBlog.Models
{
    public class Post : Model
    {
        public string Title { get; set; }
        public int Views { get; set; } = 0;

        // Text of post
        public string Content { get; set; }

        // Post summary
        public string Excerpt { get; set; }

        public string CoverImagePath { get; set; }
        public bool Public { get; set; }
    }
}
