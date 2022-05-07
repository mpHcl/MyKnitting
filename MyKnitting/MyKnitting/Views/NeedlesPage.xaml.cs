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
    public partial class NeedlesPage : ContentPage {
        public NeedlesPage() {
            InitializeComponent();
            BindingContext = new NeedlesPageViewModel();
        }
    }
}