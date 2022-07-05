using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyKnitting.Views {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProjectDetailsPage : ContentPage {
        public ProjectDetailsPage() {
            InitializeComponent();
            Console.WriteLine("test");
            BindingContext = new ViewModels.ProjectDetailsViewModel(); 
            
        }
    }
}