using MyKnitting.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.Linq;
using MyKnitting.Services;

namespace MyKnitting.ViewModels {
    [QueryProperty(nameof(ProjectId), nameof(ProjectId))]
    public class ProjectDetailsViewModel :BaseViewModel {
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
                    yarnsTemp.Add(YarnsDataStore.GetItemAsync(id.Yarn.Id.ToString()).Result);
                }

                var needlesTemp = new List<Needle>();
                foreach (var id in needlesId) {
                    needlesTemp.Add(NeedlesDataStore.GetItemAsync(id.Needle.Id.ToString()).Result);
                }

                Yarns = yarnsTemp;
                Needles = needlesTemp;                  

            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
