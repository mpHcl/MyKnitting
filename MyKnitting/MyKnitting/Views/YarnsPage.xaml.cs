﻿using MyKnitting.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyKnitting.Views {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class YarnsPage : ContentPage {
        public YarnsPage() {
            InitializeComponent();
            BindingContext = new YarnsPageViewModel();
        }
    }
}