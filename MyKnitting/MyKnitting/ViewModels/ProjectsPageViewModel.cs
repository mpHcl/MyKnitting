using MyKnitting.Models;
using MyKnitting.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;

#if __ANDROID__
    using Android.Graphics;
#endif


namespace MyKnitting.ViewModels {
    public class ProjectsPageViewModel : BaseViewModel {
        readonly IEnumerable<Project> projects;
        public Command<Project> ViewProjectDetails { get;}
        public Command AddProject { get; }
        public Command Refresh { get; }

        public ProjectsPageViewModel() {
            Title = "Projekty";
            projects = ProjectsDataStore.GetItemsAsync().Result;
            ViewProjectDetails = new Command<Project>(OnProjectClicked);
            AddProject = new Command(OnAddClick);
            Refresh = new Command(refresh);
            
            Shell.Current.Navigated += (sender, e) =>
            {
                Refresh.Execute(this);
            };
            
        }

        private bool isRefreshing = false;
        public bool IsRefreshing {
            get { return isRefreshing; }
            set {
                isRefreshing = value;
                OnPropertyChanged();
            }
        }

        public async void refresh() {
            IsRefreshing = true;
            var projectstemp = await ProjectsDataStore.GetItemsAsync();

            //Usuwanie starych elementów z kolekcji, nie można przypisać nowej kolekcji (research potrzebny)
            int numberOfItems = projects.Count();
            for (int i = 0; i < numberOfItems; i++) {
                ((ObservableCollection<Project>)projects).Remove(((ObservableCollection<Project>)projects)[0]);
            }

            
            //Wypełnianie kolekcji elementami pobranymi z bazy danych. 
            foreach (var item in ((ObservableCollection<Project>)projectstemp)) {
                ((ObservableCollection<Project>)projects).Add(item);
            }
            
            IsRefreshing = false;

        }

        public IEnumerable<Project> Projects {
            get => projects;
        }

        public async void OnProjectClicked(Project project) {
            await Shell.Current.GoToAsync($"{nameof(ProjectDetailsPage)}?{nameof(ProjectDetailsViewModel.ProjectId)}={project.Id}");
        }

        public async void OnAddClick() {
            await Shell.Current.GoToAsync($"{nameof(AddProject)}");
        }
    }
}