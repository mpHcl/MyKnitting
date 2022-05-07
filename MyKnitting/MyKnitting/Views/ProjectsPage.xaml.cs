using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using MyKnitting.ViewModels;

namespace MyKnitting.Views {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProjectsPage : ContentPage {
        public ProjectsPage() {
            InitializeComponent(); 
            BindingContext = new ProjectsPageViewModel();
        }
    }
}