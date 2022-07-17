using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MyKnitting.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MyKnitting.Droid {
    public class ImgResize : IImageResize {
        public byte[] Resize(byte[] imageData, float width, float height) {
            // Load the bitmap
            Bitmap originalImage = BitmapFactory.DecodeByteArray(imageData, 0, imageData.Length);
            var newWidth = (1200.0 / originalImage.Height) * originalImage.Width;
            Console.WriteLine("!!!!!!!!!!!" + newWidth);
            Bitmap resizedImage = Bitmap.CreateScaledBitmap(originalImage, (int) newWidth, 1200, false);
            using (MemoryStream ms = new MemoryStream()) {
                resizedImage.Compress(Bitmap.CompressFormat.Jpeg, 100, ms);
                return ms.ToArray();
            }
        }
    }
}