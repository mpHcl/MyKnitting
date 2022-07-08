using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Xamarin.Forms;
using MyKnitting.Models;
using MyKnitting.Services;
using MyKnitting.Views;

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
            SaveYarn = new Command(saveYarn);
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

        public Command SaveYarn { get; }

        private int amount = 1;
        public int Amount {
            get => amount;
            set => SetProperty(ref amount, value);
        }

        private string material = "bawełna";
        public string Material {
            get => material;
            set => SetProperty(ref material, value);
        }
        
        private string name = "name";
        public string Name {
            get => name;
            set => SetProperty(ref name, value);
        }

        private int size;
        public int Size {
            get => size;
            set => SetProperty(ref size, value);
        }
        private async void saveYarn() {
            Yarn yarn = new Yarn() { 
                Color = GetHex(), ColorCode = GetHex(), Amount = amount, Material = material, Name = name, Owned = "true", Size = size, Id = 0 
            };
            await YarnsDataStore.AddItemAsync(yarn);
            Shell.Current.SendBackButtonPressed();
        }
    }
}
