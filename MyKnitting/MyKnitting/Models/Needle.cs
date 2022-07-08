﻿using System;
using System.Collections.Generic;
using System.Text;
using SQLite;


namespace MyKnitting.Models {
    public class Needle {
        public int Id { get; set; }
        public string Type { get; set; }
        public int Size { get; set; }
        public int Length { get; set; }
        public string Owned { get; set; } 
    }
}
