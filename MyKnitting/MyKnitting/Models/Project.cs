using System;
using System.Collections.Generic;
using System.Text;

namespace MyKnitting.Models {
    public class Project {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Photo { get; set; }
        public string Pattern { get; set; }
        public bool Done { get; set; }

    }
}
