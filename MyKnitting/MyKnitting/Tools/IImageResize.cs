using System;
using System.Collections.Generic;
using System.Text;

namespace MyKnitting.Tools {
    public interface IImageResize {
         byte[] Resize(byte[] imageData, float width, float height);
    }
}
