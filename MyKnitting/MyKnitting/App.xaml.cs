using MyKnitting.Services;
using MyKnitting.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyKnitting {
    public partial class App : Application {

        public App() {
            InitializeComponent();

            DependencyService.Register<ProjectDataStore>();
            DependencyService.Register<NeedleDataStore>();
            DependencyService.Register<YarnDataStore>();
            DependencyService.Register<NFPDataStore>();
            DependencyService.Register<YFPDataStore>();
            MainPage = new AppShell();
        }

        protected override void OnStart() {
        }

        protected override void OnSleep() {
        }

        protected override void OnResume() {
        }
    }
}
