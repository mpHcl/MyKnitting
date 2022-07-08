using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace MyKnitting.Models {
    public class Project {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Photo { get; set; }
        public string Pattern { get; set; }
        public string Done { get; set; }

    }
}
