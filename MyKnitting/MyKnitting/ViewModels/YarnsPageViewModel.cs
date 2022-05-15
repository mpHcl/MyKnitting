using MyKnitting.Models;
using MyKnitting.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MyKnitting.ViewModels {
    public class YarnsPageViewModel : BaseViewModel {
        readonly IEnumerable<Yarn> yarns;

        public Command<Yarn> DeleteYarn { get; }
        public Command AddYarn { get; }


        public YarnsPageViewModel() {
            Title = "Włóczki";
            yarns = YarnsDataStore.GetItemsAsync().Result;
            DeleteYarn = new Command<Yarn>(OnDeleteItemClick);
            AddYarn = new Command(OnAddClick);
        }

        public IEnumerable<Yarn> Yarns {
            get => yarns;
        }

        private async void OnDeleteItemClick(Yarn yarn) {
            var answear = await Shell.Current.DisplayAlert("Uwaga", "Na pewno chcesz usunąć tę włóczkę?", "Tak", "Nie");
            if (answear == true) {
                Console.WriteLine("HELLLO");
                Console.WriteLine(yarn.Id);
                await YarnsDataStore.DeleteItemAsync(yarn.Id.ToString());
            }
        }

        public async void OnAddClick() {
            await Shell.Current.GoToAsync($"{nameof(AddYarn)}");
        }
    }
}
