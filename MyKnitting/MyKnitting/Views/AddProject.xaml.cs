﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using MyKnitting.ViewModels;

namespace MyKnitting.Views {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddProject : ContentPage {
        public AddProject() {
            InitializeComponent();
            BindingContext = new AddProjectViewModel();
        }
    }
}