using MyKnitting.Models;
using MyKnitting.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Linq;

namespace MyKnitting.ViewModels {
    public class YarnsPageViewModel : BaseViewModel {
        private IEnumerable<Yarn> yarns;

        public Command<Yarn> DeleteYarn { get; }
        public Command AddYarn { get; }
        public Command Refresh { get; }

        public YarnsPageViewModel() {
            IsRefreshing = false;
            Title = "Włóczki";
            yarns = YarnsDataStore.GetItemsAsync().Result;
            DeleteYarn = new Command<Yarn>(OnDeleteItemClick);
            AddYarn = new Command(OnAddClick);
            Refresh = new Command(refresh);

            Shell.Current.Navigated += (sender, e) =>
            {
                Refresh.Execute(this);
            };
        }

        public IEnumerable<Yarn> Yarns {
            get => yarns;
        }

        private async void OnDeleteItemClick(Yarn yarn) {
            var answear = await Shell.Current.DisplayAlert("Uwaga", "Na pewno chcesz usunąć tę włóczkę?", "Tak", "Nie");
            if (answear == true) {
                await YarnsDataStore.DeleteItemAsync(yarn.Id.ToString());
            }
            Refresh.Execute(this);
        }

        public async void OnAddClick() {
            await Shell.Current.GoToAsync($"{nameof(AddYarn)}");
        }

        private bool isRefreshing;
        public bool IsRefreshing {
            get { return isRefreshing; }
            set {
                isRefreshing = value;
                OnPropertyChanged();
            }
        }
        public async void refresh() {
            IsRefreshing = true;
            var yarnstemp = await YarnsDataStore.GetItemsAsync();
            
            //Usuwanie starych elementów z kolekcji, nie można przypisać nowej kolekcji (research potrzebny)
            int numberOfItems = yarns.Count();
            for (int i = 0; i < numberOfItems; i++) {
                ((ObservableCollection<Yarn>)yarns).Remove(((ObservableCollection<Yarn>)yarns)[0]);
            }

            //Wypełnianie kolekcji elementami pobranymi z bazy danych. 
            foreach (var item in ((ObservableCollection<Yarn>)yarnstemp)) {
                  ((ObservableCollection<Yarn>)yarns).Add(item);
            }

            IsRefreshing = false;
        }
    }
}
