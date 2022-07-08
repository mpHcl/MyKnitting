using MyKnitting.Models;
using MyKnitting.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyKnitting.Views {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddYarnsForProject : ContentPage {
        public AddYarnsForProject() {
            InitializeComponent();
            BindingContext = new AddYFPViewModel();
        }

        async void SelectedAdd(object sender, SelectionChangedEventArgs e ) {
            if (((CollectionView)sender).SelectedItem != null) {
                var answear = await Shell.Current.DisplayAlert(null, "Czy chcesz dodać tę włóczkę do projektu: ", "Anuluj", "Dodaj");
                if (answear == false) {
                    ((AddYFPViewModel)BindingContext).AddSelected(e.CurrentSelection.FirstOrDefault() as Yarn);
                }
                ((CollectionView)sender).SelectedItem = null;
            }
        }

        async void SelectedRemove(object sender, SelectionChangedEventArgs e) {
            if (((CollectionView)sender).SelectedItem != null) {
                var answear = await Shell.Current.DisplayAlert(null, "Czy chcesz usunąć tę włóczkę z tego projektu: ", "Anuluj", "Usuń");
                if (answear == false) {
                    ((AddYFPViewModel)BindingContext).RemoveSelected(e.CurrentSelection.FirstOrDefault() as Yarn);  
                }
                ((CollectionView)sender).SelectedItem = null;
            }
        }
    }
}