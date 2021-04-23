using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuperSimpleBlog.Models
{
    public class Model
    {
        public string ID { get; set; }

        // Will hold the original creation date of the field, 
        // the default value is set to DateTime.Now
        public DateTime Created { get; set; }

        // will hold the last updated date of the field
        ///will initially be set to DateTime.Now, but should be updated on every...update
        public DateTime Updated { get; set; }
        public bool Deleted { get; set; } = false;
    }
}
