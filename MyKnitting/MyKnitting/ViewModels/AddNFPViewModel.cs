using MyKnitting.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace MyKnitting.ViewModels {
    [QueryProperty(nameof(ProjectId), nameof(ProjectId))]
    public class AddNFPViewModel : BaseViewModel {
        private Project project;

        private IEnumerable<Needle> needles;
        private IEnumerable<Needle> allNeedles;
        //private IEnumerable<Needle> usableYarns;
        public IEnumerable<Needle> Needles {
            get => needles;
            set => SetProperty(ref needles, value);
        }

        public IEnumerable<Needle> UsableNeedles {
            get => allNeedles;
            set => SetProperty(ref allNeedles, value);
        }

        public AddNFPViewModel() {
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

                /*
                //AllYarns = await YarnsDataStore.GetItemsAsync();

                IEnumerable<YarnsForProject> yarnsID = YFPDataStore.GetItemsAsync().Result.
                    Where(x => x.Project.Id == int.Parse(projectId));

                var yarnsTemp = new ObservableCollection<Yarn>();
                foreach (var id in yarnsID) {
                    var yarn = YarnsDataStore.GetItemAsync(id.Yarn.Id.ToString()).Result;
                    if (yarn != null)
                        yarnsTemp.Add(yarn);
                }

                Yarns = yarnsTemp;
                */

                var allYarns = await NeedlesDataStore.GetItemsAsync();
                IEnumerable<NeedlesForProjects> yarnsIDTemp = NFPDataStore.GetItemsAsync().Result.
                    Where(x => x.Project.Id == int.Parse(projectId));

                ObservableCollection<Needle> yarnsID = new ObservableCollection<Needle>();

                foreach (var item in yarnsIDTemp) {
                    var yarn = item.Needle;
                    if (yarn != null) {
                        yarnsID.Add(yarn);
                    }
                }

                Console.WriteLine("!!!!!!!!!!!!!!");
                Console.WriteLine(yarnsID.Count());
                var yarnsTempUsed = new ObservableCollection<Needle>();
                var yarnsTempUsable = new ObservableCollection<Needle>();

                foreach (var item in allYarns) {

                    if (yarnsID.Select(x => x.Id).Contains(item.Id)) {
                        yarnsTempUsed.Add(item);
                    }
                    else {
                        yarnsTempUsable.Add(item);
                    }

                }

                UsableNeedles = yarnsTempUsable;
                Needles = yarnsTempUsed;

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
            var yarnsTemp = NFPDataStore.GetItemsAsync().Result.
                    Where(x => x.Project.Id == int.Parse(projectId));

            foreach (var item in yarnsTemp) {
                await NFPDataStore.DeleteItemAsync(item.Id.ToString());
            }

            foreach (var item in needles) {
                Console.WriteLine(item.Type);
                await NFPDataStore.AddItemAsync(new NeedlesForProjects() { Project = project, Needle = item });
            }

            Shell.Current.SendBackButtonPressed();
        }

        public void AddSelected(Needle yarn) {
            ((ObservableCollection<Needle>)allNeedles).Remove(yarn);
            ((ObservableCollection<Needle>)needles).Add(yarn);
        }
        public void RemoveSelected(Needle yarn) {
            ((ObservableCollection<Needle>)needles).Remove(yarn);
            ((ObservableCollection<Needle>)allNeedles).Add(yarn);
        }
    }
}
