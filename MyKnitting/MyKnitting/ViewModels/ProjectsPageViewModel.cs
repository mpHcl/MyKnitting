using MyKnitting.Models;
using MyKnitting.Views;
using System;
using System.Collections.Generic;

using Xamarin.Forms;


namespace MyKnitting.ViewModels {
    public class ProjectsPageViewModel : BaseViewModel {
        readonly IEnumerable<Project> projects;
        public Command<Project> ViewProjectDetails { get;}
        public Command AddProject { get; }
        
        public ProjectsPageViewModel() {
            Title = "Projekty";
            projects = ProjectsDataStore.GetItemsAsync().Result;
            Console.WriteLine("Tutaj chyba już nie działa...");
            ViewProjectDetails = new Command<Project>(OnProjectClicked);
            AddProject = new Command(OnAddClick);
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