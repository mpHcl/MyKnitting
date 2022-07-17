using MyKnitting.Models;
using MyKnitting.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MyKnitting.ViewModels {
    public class AddNeedlesViewModel : BaseViewModel {
        private string type = "typ";
        public string Type {
            get => type;
            set => SetProperty(ref type, value);
        }

        private int size = 1;
        public int Size {
            get => size;
            set => SetProperty(ref size, value);
        }


        private int length = 60;
        public int Length {
            get => length;
            set => SetProperty(ref length, value);
        }

        public Command SaveNeedles { get; }
        private async void SaveNeedlesFunc() {
            Needle needle = new Needle() { Id = 0, Length = length, Size = size, Type = type, Owned = "true"};
            await NeedlesDataStore.AddItemAsync(needle);
            Shell.Current.SendBackButtonPressed();
        }

        public AddNeedlesViewModel() {
            SaveNeedles = new Command(SaveNeedlesFunc);
        }

    }
}
