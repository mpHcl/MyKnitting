using MyKnitting.Models;
using MyKnitting.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MyKnitting.ViewModels {
    public class NeedlesPageViewModel : BaseViewModel {
        private IEnumerable<Needle> needles;
        public Command<Needle> DeleteNeedle { get; }
        public Command AddNeedle { get; }

        public NeedlesPageViewModel() {
            Title = "Druty";
            needles = NeedlesDataStore.GetItemsAsync().Result;
            DeleteNeedle = new Command<Needle>(OnDeleteItemClick);
            AddNeedle = new Command(OnAddClick);
        }

        public IEnumerable<Needle> Needles {
            get => needles;
        }

        private async void OnDeleteItemClick(Needle needle) {
            Console.WriteLine("HELLLO");
            var answear = await Shell.Current.DisplayAlert("Uwaga", "Na pewno chcesz usunąć te druty?", "Tak", "Nie");
            if (answear == true) {
                await NeedlesDataStore.DeleteItemAsync(needle.Id.ToString());
            }           
        }
        public async void OnAddClick() {
            await Shell.Current.GoToAsync($"{nameof(AddNeedle)}");
        }
    }
}
