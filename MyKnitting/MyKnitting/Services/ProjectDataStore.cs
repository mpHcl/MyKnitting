using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyKnitting.Models;


namespace MyKnitting.Services {
    public class ProjectDataStore : IDataStore<Project> {
        readonly ObservableCollection<Project> projects;

        public ProjectDataStore() {
            projects = new ObservableCollection<Project>() {
                new Project {Id =0, Done = false, Name = "Mój pierwszy projekt.", Pattern="hello.pdf", Photo="Images/asdvas.jpg"},
                new Project {Id = 1, Done = false, Name = "Nice", Pattern="hello.pdf", Photo="Images/asdasdvas.jpg"},
                new Project {Id = 2, Done = false, Name ="Coś już tu się dzieje", Pattern = "ten_sam.pdf", Photo = "Images/Hmmm.png"},
                new Project {Id = 3, Done = false, Name = "Mój pierwszy projekt.", Pattern="hello.pdf", Photo="Images/asdvas.jpg"},
                new Project {Id = 4, Done = false, Name = "Nice", Pattern="hello.pdf", Photo="Images/asdasdvas.jpg"},
                new Project {Id = 5, Done = false, Name ="Coś już tu się dzieje", Pattern = "ten_sam.pdf", Photo = "Images/Hmmm.png"},
                new Project {Id = 6, Done = false, Name = "Mój pierwszy projekt.", Pattern="hello.pdf", Photo="Images/asdvas.jpg"},
                new Project {Id = 7, Done = false, Name = "Nice", Pattern="hello.pdf", Photo="Images/asdasdvas.jpg"},
                new Project {Id = 8, Done = false, Name ="Coś już tu się dzieje", Pattern = "ten_sam.pdf", Photo = "Images/Hmmm.png"},
                new Project {Id = 9, Done = false, Name = "Mój pierwszy projekt.", Pattern="hello.pdf", Photo="Images/asdvas.jpg"},
                new Project {Id = 10, Done = false, Name = "Nice", Pattern="hello.pdf", Photo="Images/asdasdvas.jpg"},
                new Project {Id = 11, Done = false, Name ="Coś już tu się dzieje", Pattern = "ten_sam.pdf", Photo = "Images/Hmmm.png"},
                new Project {Id = 12, Done = false, Name = "Mój pierwszy projekt.", Pattern="hello.pdf", Photo="Images/asdvas.jpg"},

            };
        }

        public Task<bool> AddItemAsync(Project item) {
            projects.Add(item);
            return Task.FromResult(true);
        }
        public Task<bool> UpdateItemAsync(Project item) {
            var itemToUpdate = projects.Where(x => x.Id == item.Id).First();
            itemToUpdate.Name = item.Name;
            itemToUpdate.Pattern = item.Pattern;
            itemToUpdate.Photo = item.Photo;
            
            return Task.FromResult(true);
        }
        public Task<bool> DeleteItemAsync(string id) {
            return null; 
        }
        public async Task<Project> GetItemAsync(string id) {
            var intId = int.Parse(id);
            return await Task.FromResult(projects.FirstOrDefault(item => item.Id == intId));
        }
        public async Task<IEnumerable<Project>> GetItemsAsync(bool forceRefresh = false) {
            return await Task.FromResult(projects);
        }
    }
}
