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
using System.Collections.ObjectModel;


namespace MyKnitting.ViewModels {
    [QueryProperty(nameof(ProjectId), nameof(ProjectId))]
    public class ProjectDetailsViewModel : BaseViewModel {
        private Project project;
        private string projectId;
        private string projectName;
        private string width;
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
            get {
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


                var yarnsTemp = new ObservableCollection<Yarn>();
                foreach (var id in yarnsID) {
                    var yarn = YarnsDataStore.GetItemAsync(id.Yarn.Id.ToString()).Result;
                    if (yarn != null)
                        yarnsTemp.Add(yarn);
                }

                var needlesTemp = new ObservableCollection<Needle>();
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
            EditNeedles = new Command(EditNeedlesFunc);
            EditYarns = new Command(EditYarnsFunc);
            ChangePattern = new Command(ChangePatternFunc);
            ChangePhoto = new Command(ChangePhotoFunc);
            SeePattern = new Command(SeePatternFunc);
            DeleteProject = new Command(DeleteProjectFunc);
            SaveProject = new Command(SaveProjectFunc);
        }

        public Command EditNeedles { get; }
        public Command EditYarns { get; }
        public Command ChangePattern { get; }
        public Command ChangePhoto { get; }

        public Command SeePattern { get; }

        public Command DeleteProject { get; }
        public Command SaveProject { get; }

        private async void SeePatternFunc() {
            var openFileRequest = new OpenFileRequest(project.Pattern, new ReadOnlyFile(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData))));
            //openFileRequest.File = new ReadOnlyFile(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), project.Pattern));
            await Launcher.OpenAsync(openFileRequest);
            await ProjectsDataStore.UpdateItemAsync(project);
            
        }
        private async void EditNeedlesFunc() {
            await Shell.Current.GoToAsync($"{nameof(AddNeedlesForProject)}?{nameof(ProjectDetailsViewModel.ProjectId)}={project.Id}");
        }

        private async void EditYarnsFunc() {
            await Shell.Current.GoToAsync($"{nameof(AddYarnsForProject)}?{nameof(ProjectDetailsViewModel.ProjectId)}={project.Id}");
        }
        Tools.IImageResize resizer => DependencyService.Get<Tools.IImageResize>();

        private async void ChangePhotoFunc() {
            var answear = await Shell.Current.DisplayActionSheet("Wybierz opcje: ", "Anuluj", null, "Zmień zdjęcie");

            if (answear == "Zmień zdjęcie") {
                try {
                    var result = await FilePicker.PickAsync(PickOptions.Images);
                    var new_path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), result.FileName);
                    File.Copy(result.FullPath, new_path, true);
                    File.WriteAllBytes(new_path, resizer.Resize(File.ReadAllBytes(result.FullPath), 1600, 900));


                    //projectPhoto = new_path;
                    project.Photo = new_path;
                    ProjectPhoto = project.Photo;
                } catch (Exception ex) {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private async void ChangePatternFunc() {
            try {
                var result = await FilePicker.PickAsync();
                var new_path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), result.FileName);
                File.Copy(result.FullPath, new_path, true);
                project.Pattern = result.FileName;
                Pattern = project.Pattern;
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
        }

        private async void DeleteProjectFunc() {
            await ProjectsDataStore.DeleteItemAsync(project.Id.ToString());
            Shell.Current.SendBackButtonPressed();
        }

        private async void SaveProjectFunc() {
            await ProjectsDataStore.UpdateItemAsync(project);
            Shell.Current.SendBackButtonPressed();
        }
    }
}
