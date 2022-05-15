using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Xamarin.Forms;

namespace MyKnitting.ViewModels {
    public class AddYarnViewModel : BaseViewModel {
        byte r;
        byte g;
        byte b;

        public byte Red {
            get => r;
            set => SetProperty(ref r, value, "" , SetColor);
        }

        public byte Green {
            get => g;
            set => SetProperty(ref g, value, "", SetColor);
        }
        public byte Blue {
            get => b;
            set => SetProperty(ref b, value, "", SetColor);
        }

        public string ColorSource {
            get {
                return GetHex();
            }
            set {

            }
        }

        public string ColorCode {
            get;
            set;
        }
        public AddYarnViewModel() {
            Title = "Dodaj włóczkę";
            r = 1;
            g = 1;
            b = 1;
        }

        private string GetHex() {
            string r = Convert.ToString(Red, 16);
            string g = Convert.ToString(Green, 16);
            string b = Convert.ToString(Blue, 16);

            if (r.Length == 1) {
                r = '0' + r;
            }

            if (g.Length == 1) {
                g = '0' + g;
            }

            if (b.Length == 1) {
                b = '0' + b;
            }

            return $"#{r}{g}{b}";
        }

        private void SetColor() {
            ColorSource = GetHex();
            ColorCode = GetHex();
        }   
    }
}
