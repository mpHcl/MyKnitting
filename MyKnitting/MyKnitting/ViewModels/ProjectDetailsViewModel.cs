using MyKnitting.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.Linq;
using MyKnitting.Services;
using System.IO;
using MyKnitting.Views;

namespace MyKnitting.ViewModels {
    [QueryProperty(nameof(ProjectId), nameof(ProjectId))]
    public class ProjectDetailsViewModel : BaseViewModel {
        private Project project;
        private string projectId;
        private string projectName;
        private string width;
        private string height;
        private string pattern;
        private IEnumerable<Needle> needles;
        private IEnumerable<Yarn> yarns;

        public string ProjectId {
            get {
                return projectId;
            }
            set {
                projectId = value;
                LoadItemId(value);
            }
        }
        public string ProjectName {
            get { Console.WriteLine("TESSTTT");
                return project.Name;
            }

            set => SetProperty(ref projectName, value);
        }

        public string PhotoWidth {
            get => width;
            set => SetProperty(ref width, value);
        }


        public string ProjectPhoto {
            get => project.Photo;
            set => SetProperty(ref projectName, value);
        }

        public string Pattern {
            get => pattern;
            set => SetProperty(ref pattern, value);
        }
        
        public IEnumerable<Needle> Needles {
            get => needles; 
            set => SetProperty(ref needles, value);
        }

        public IEnumerable<Yarn> Yarns {
            get => yarns;
            set => SetProperty(ref yarns, value);
        }
  
        public async void LoadItemId(string projectId) {
            try {
                project = await ProjectsDataStore.GetItemAsync(projectId);
                ProjectName = project.Name;
                ProjectPhoto = project.Photo;
                Title = project.Name;
                width = DeviceDisplay.MainDisplayInfo.Width.ToString();
                PhotoWidth = width;
                Pattern = project.Pattern;

                IEnumerable<YarnsForProject> yarnsID = YFPDataStore.GetItemsAsync().Result.
                    Where(x => x.Project.Id == int.Parse(projectId));
                IEnumerable<NeedlesForProjects> needlesId = NFPDataStore.GetItemsAsync().Result.
                    Where(x => x.Project.Id == int.Parse(projectId));

                var yarnsTemp = new List<Yarn>();
                foreach (var id in yarnsID) {
                    var yarn = YarnsDataStore.GetItemAsync(id.Yarn.Id.ToString()).Result;
                    if (yarn != null)
                        yarnsTemp.Add(yarn);
                }

                var needlesTemp = new List<Needle>();
                foreach (var id in needlesId) {
                    var needle = NeedlesDataStore.GetItemAsync(id.Needle.Id.ToString()).Result;
                    if (needle != null)
                        needlesTemp.Add(needle);
                }

                Yarns = yarnsTemp;
                Needles = needlesTemp;                  

            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
        }


        public ProjectDetailsViewModel() {
            EditNeedles = new Command(editNeedles);
            EditYarns = new Command(editYarns);
            ChangePattern = new Command(changePattern);
            ChangePhoto = new Command(changePhoto);
            SeePattern = new Command(seePattern);
            DeleteProject = new Command(deleteProject);
        }

        public Command EditNeedles { get; }
        public Command EditYarns { get; }
        public Command ChangePattern { get; }
        public Command ChangePhoto { get; }

        public Command SeePattern { get; }

        public Command DeleteProject { get; }

        private async void seePattern() {
            Console.WriteLine("HELLLO");
            var openFileRequest = new OpenFileRequest();
            Console.WriteLine(project.Pattern);
            Console.WriteLine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), project.Pattern));
            openFileRequest.File = new ReadOnlyFile(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), project.Pattern));
            Console.WriteLine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), project.Pattern));
            await Launcher.OpenAsync(openFileRequest);
            await ProjectsDataStore.UpdateItemAsync(project);
            
        }
        private async void editNeedles() {
            await Shell.Current.DisplayActionSheet("Wybierz opcje: ", "Anuluj", null, "Dodaj druty do projektu", "Usuń druty z projektu");
        }

        private async void editYarns() {
            await Shell.Current.DisplayActionSheet("Wybierz opcje: ", "Anuluj", null, "Dodaj włókczki do projektu", "Usuń włóczki z projektu");
        }

        private async void changePhoto() {
            var answear = await Shell.Current.DisplayActionSheet("Wybierz opcje: ", "Anuluj", null, "Zmień zdjęcie");

            if (answear == "Zmień zdjęcie") {
                try {
                    var result = await FilePicker.PickAsync(PickOptions.Images);
                    var new_path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), result.FileName);
                    File.Copy(result.FullPath, new_path, true);
                    Console.WriteLine(result.FullPath);
                    project.Photo = new_path;
                    ProjectPhoto = project.Photo;
                } catch (Exception ex) {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private async void changePattern() {
            try {
                var result = await FilePicker.PickAsync();
                Console.WriteLine(result.FullPath);
                var new_path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), result.FileName);
                File.Copy(result.FullPath, new_path, true);
                project.Pattern = result.FileName;
                Pattern = project.Pattern;
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
        }

        private async void deleteProject() {
            await new ProjectDataStore().DeleteItemAsync(project.Id.ToString());
            await Shell.Current.GoToAsync($"{nameof(ProjectsPage)}");
        }
    }
}
