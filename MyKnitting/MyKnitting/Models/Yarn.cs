using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace MyKnitting.Models {
    public class Yarn {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public string ColorCode { get; set; }
        public int Size { get; set; }
        public string Material { get; set; }
        public int Amount { get; set; }
        public string Owned { get; set; }

    }
}
