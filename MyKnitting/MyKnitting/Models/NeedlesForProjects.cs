using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace MyKnitting.Models {
    public class NeedlesForProjects {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public Project Project { get; set; }
        public Needle Needle { get; set; }
    }
}
