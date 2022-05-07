using MyKnitting.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyKnitting.Services {
    public class YFPDataStore : IDataStore<YarnsForProject> {
        readonly ObservableCollection<YarnsForProject> collection;

        public YFPDataStore() {
            var projects = new List<Project>(new ProjectDataStore().GetItemsAsync().Result);
            var yarns = new List<Yarn>(new YarnDataStore().GetItemsAsync().Result);
            collection = new ObservableCollection<YarnsForProject>() {
                    new YarnsForProject {Id = 0, Project= projects[0], Yarn=yarns[1] },
                    new YarnsForProject {Id = 1, Project= projects[0], Yarn=yarns[2] },
                    new YarnsForProject {Id = 2, Project= projects[1], Yarn=yarns[3] },
                    new YarnsForProject {Id = 3, Project= projects[1], Yarn=yarns[4] },
                    new YarnsForProject {Id = 4, Project= projects[1], Yarn=yarns[5] },
                    new YarnsForProject {Id = 5, Project= projects[1], Yarn=yarns[2] },
                    new YarnsForProject {Id = 6, Project= projects[2], Yarn=yarns[3] },
                    new YarnsForProject {Id = 7, Project= projects[2], Yarn=yarns[4] },
                    new YarnsForProject {Id = 8, Project= projects[2], Yarn=yarns[5] },
                    new YarnsForProject {Id = 9, Project= projects[2], Yarn=yarns[6] },
                    new YarnsForProject {Id = 10, Project= projects[2], Yarn=yarns[1] },
                    new YarnsForProject {Id = 11, Project= projects[3], Yarn=yarns[2] },
                    new YarnsForProject {Id = 12, Project= projects[3], Yarn=yarns[3] },
                    new YarnsForProject {Id = 13, Project= projects[3], Yarn=yarns[4] },
                    new YarnsForProject {Id = 14, Project= projects[3], Yarn=yarns[7] }

            };
        }

        public async Task<bool> AddItemAsync(YarnsForProject yfp) {
            collection.Add(yfp);
            return await Task.FromResult(true);
        } 

        public async Task<bool> UpdateItemAsync(YarnsForProject yfp) {
            return await Task.FromResult(false);
        }

        public async Task<bool> DeleteItemAsync(string id) {
            try {
                var intId = int.Parse(id);
                collection.Remove(collection.Where(x => x.Id == intId).FirstOrDefault());
                return await Task.FromResult(true);

            } catch (Exception) {
                return await Task.FromResult(false);
            }
        }
        public Task<YarnsForProject> GetItemAsync(string id) {
            return null;
        }

        public async Task<IEnumerable<YarnsForProject>> GetItemsAsync(bool forceRefresh = false) {
            //Console.WriteLine("Hello");
            return await Task.FromResult(collection);
        }
    }
}
