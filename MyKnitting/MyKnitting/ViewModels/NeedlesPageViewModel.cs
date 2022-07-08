using MyKnitting.Models;
using MyKnitting.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
            Refresh = new Command(refresh);

            Shell.Current.Navigated += (sender, e) =>
            {
                Refresh.Execute(this);
            };
        }

        public IEnumerable<Needle> Needles {
            get => needles;
        }

        private async void OnDeleteItemClick(Needle needle) {
            var answear = await Shell.Current.DisplayAlert("Uwaga", "Na pewno chcesz usunąć te druty?", "Tak", "Nie");
            if (answear == true) {
                await NeedlesDataStore.DeleteItemAsync(needle.Id.ToString());
            }
            Refresh.Execute(this);
        }
        public async void OnAddClick() {
            await Shell.Current.GoToAsync($"{nameof(AddNeedle)}");
        }

        public Command Refresh { get; }
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
            var needlesstemp = await NeedlesDataStore.GetItemsAsync();

            //Usuwanie starych elementów z kolekcji, nie można przypisać nowej kolekcji (research potrzebny)
            int numberOfItems = needles.Count();
            for (int i = 0; i < numberOfItems; i++) {
                ((ObservableCollection<Needle>)needles).Remove(((ObservableCollection<Needle>)needles)[0]);
            }

            //Wypełnianie kolekcji elementami pobranymi z bazy danych. 
            foreach (var item in ((ObservableCollection<Needle>)needlesstemp)) {
                ((ObservableCollection<Needle>)needles).Add(item);
            }

            IsRefreshing = false;
        }
    }
}
