using System;
using System.Collections.Generic;
using System.Text;

namespace MyKnitting.Models {
    public class NeedlesForProjects {
        public int Id { get; set; }
        public Project Project { get; set; }
        public Needle Needle { get; set; }
    }
}
