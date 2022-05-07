using MyKnitting.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyKnitting.Services {
    public class NFPDataStore : IDataStore<NeedlesForProjects>{
        readonly ObservableCollection<NeedlesForProjects> collection;

        public NFPDataStore() {
            var projects = new List<Project>(new ProjectDataStore().GetItemsAsync().Result);
            var needles = new List<Needle>(new NeedleDataStore().GetItemsAsync().Result);
            collection = new ObservableCollection<NeedlesForProjects>() {
                    new NeedlesForProjects {Id = 0, Project= projects[0], Needle=needles[1] },
                    new NeedlesForProjects {Id = 1, Project= projects[0], Needle=needles[2] },
                    new NeedlesForProjects {Id = 2, Project= projects[1], Needle=needles[3] },
                    new NeedlesForProjects {Id = 3, Project= projects[1], Needle=needles[4] },
                    new NeedlesForProjects {Id = 4, Project= projects[1], Needle=needles[5] },
                    new NeedlesForProjects {Id = 5, Project= projects[1], Needle=needles[2] },
                    new NeedlesForProjects {Id = 6, Project= projects[2], Needle=needles[3] },
                    new NeedlesForProjects {Id = 7, Project= projects[2], Needle=needles[4] },
                    new NeedlesForProjects {Id = 8, Project= projects[2], Needle=needles[5] },
                    new NeedlesForProjects {Id = 9, Project= projects[2], Needle=needles[6] },
                    new NeedlesForProjects {Id = 10, Project= projects[2], Needle=needles[1] },
                    new NeedlesForProjects {Id = 11, Project= projects[3], Needle=needles[2] },
                    new NeedlesForProjects {Id = 12, Project= projects[3], Needle=needles[3] },
                    new NeedlesForProjects {Id = 13, Project= projects[3], Needle=needles[4] },
                    new NeedlesForProjects {Id = 14, Project= projects[3], Needle=needles[7] }

            };
        }

        public async Task<bool> AddItemAsync(NeedlesForProjects yfp) {
            collection.Add(yfp);
            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(NeedlesForProjects yfp) {
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
        public Task<NeedlesForProjects> GetItemAsync(string id) {
            return null;
        }

        public async Task<IEnumerable<NeedlesForProjects>> GetItemsAsync(bool forceRefresh = false) {
            //Console.WriteLine("Hello");
            return await Task.FromResult(collection);
        }
    }

}
