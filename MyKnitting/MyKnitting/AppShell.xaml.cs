using MyKnitting.ViewModels;
using MyKnitting.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace MyKnitting {
    public partial class AppShell : Xamarin.Forms.Shell {
        public AppShell() {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ProjectDetailsPage), typeof(ProjectDetailsPage));
            Routing.RegisterRoute(nameof(AddProject), typeof(AddProject));
            Routing.RegisterRoute(nameof(AddNeedle), typeof(AddNeedle));
            Routing.RegisterRoute(nameof(AddYarn), typeof(AddYarn));
        }
    }
}
