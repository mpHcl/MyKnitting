using MyKnitting.Models;
using MyKnitting.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace MyKnitting.ViewModels {
    [QueryProperty(nameof(ProjectId), nameof(ProjectId))]
    public class AddYFPViewModel :BaseViewModel {
        private Project project;

        private IEnumerable<Yarn> yarns; 
        private IEnumerable<Yarn> allYarns;
        //private IEnumerable<Yarn> usableYarns;
        public IEnumerable<Yarn> Yarns {
            get => yarns;
            set => SetProperty(ref yarns, value);
        }

        public IEnumerable<Yarn> UsableYarns {
            get => allYarns;
            set => SetProperty(ref allYarns, value);
        }

        public AddYFPViewModel() {
            Cancel = new Command(CancelFunc);
            Save = new Command(SaveFunc);
        }

        private string projectId;
        public string ProjectId {
            get {
                return projectId;
            }
            set {
                projectId = value;
                LoadItemId(value);
            }
        }

        public async void LoadItemId(string projectId) {
            try {
                project = await ProjectsDataStore.GetItemAsync(projectId);

                var allYarns = await YarnsDataStore.GetItemsAsync();
                IEnumerable<YarnsForProject> yarnsIDTemp = YFPDataStore.GetItemsAsync().Result.
                    Where(x => x.Project.Id == int.Parse(projectId));

                ObservableCollection<Yarn> yarnsID = new ObservableCollection<Yarn>();
                
                foreach (var item in yarnsIDTemp) {
                    var yarn = item.Yarn;
                    if (yarn != null) {
                        yarnsID.Add(yarn);
                    }
                }

                var yarnsTempUsed = new ObservableCollection<Yarn>(); 
                var yarnsTempUsable = new ObservableCollection<Yarn>();

                foreach (var item in allYarns) {
                        
                        if (yarnsID.Select(x => x.Id).Contains(item.Id)) {
                            yarnsTempUsed.Add(item);
                        }
                        else {
                            yarnsTempUsable.Add(item);
                        }
                    
                }

                UsableYarns = yarnsTempUsable;
                Yarns = yarnsTempUsed;

            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
        }

        public Command Save { get; }
        public Command Cancel { get; }
        private void CancelFunc() {
            Shell.Current.SendBackButtonPressed();
        }

        private async void SaveFunc() {
            var yarnsTemp = YFPDataStore.GetItemsAsync().Result.
                    Where(x => x.Project.Id == int.Parse(projectId));

            foreach (var item in yarnsTemp) {
                await YFPDataStore.DeleteItemAsync(item.Id.ToString());
            }
            
            foreach (var item in yarns) {
                await YFPDataStore.AddItemAsync(new YarnsForProject() { Project = project, Yarn = item });
            }

            Shell.Current.SendBackButtonPressed();
        }

        public void AddSelected(Yarn yarn) {
            ((ObservableCollection<Yarn>)allYarns).Remove(yarn);
            ((ObservableCollection<Yarn>)yarns).Add(yarn);
        }
        public void RemoveSelected(Yarn yarn) {
            ((ObservableCollection<Yarn>)yarns).Remove(yarn);
            ((ObservableCollection<Yarn>)allYarns).Add(yarn);
        }


    }
}
