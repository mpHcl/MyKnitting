using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Media;
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
        public byte[] Resize(byte[] imageData, float width, float height, string path) {
            // Load the bitmap
            Bitmap originalImage = BitmapFactory.DecodeByteArray(imageData, 0, imageData.Length);
            var newWidth = (900.0/ originalImage.Height) * originalImage.Width;
            Console.WriteLine("!!!!!!!!!!!" + newWidth);
            ExifInterface exif = new ExifInterface(path);
            var orientation = exif.GetAttributeInt(ExifInterface.TagOrientation, 1);
            Console.WriteLine(orientation);
            Matrix matrix = new Matrix();
            //matrix.PostRotate(0);
              //8 - bez zmian
            switch (orientation) {
                case 0:
                    break;
                case 6:
                    matrix.SetRotate(90);
                    newWidth = (900.0 / originalImage.Width) * originalImage.Height;
                    break;
                case 7:
                    matrix.SetRotate(-90);
                    break;
                case 8: matrix.SetRotate(270);
                        newWidth = (900.0 / originalImage.Width) * originalImage.Height;
                    break;
            }
        

        originalImage = Bitmap.CreateBitmap(originalImage, 0, 0, originalImage.Width, originalImage.Height, matrix, true);

            Bitmap resizedImage = Bitmap.CreateScaledBitmap(originalImage, (int) newWidth, 900, false);
            using (MemoryStream ms = new MemoryStream()) {
                resizedImage.Compress(Bitmap.CompressFormat.Jpeg, 100, ms);
                return ms.ToArray();
            }
        }
    }
}