using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace MyKnitting.Models {
    public class YarnsForProject {
        public int Id { get; set; }

        public Project Project { get; set; }
        public Yarn Yarn { get; set; }
    }
}
