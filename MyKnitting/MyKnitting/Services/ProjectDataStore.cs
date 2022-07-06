using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyKnitting.Models;
using Microsoft.Data.Sqlite;


namespace MyKnitting.Services {
    public class ProjectDataStore : IDataStore<Project> {
        /*
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
        }*/

        //To implement...
        //Only thing to do is configure app database in other place, probably somewhere in app.xml.cs 
        //and uncommnet this section.
        //There might be problems with id so make sure to take care of this, probably all you need to do 
        //is make a primary key in model, and give cotnrol of it to the database, and this will be the simpliest 
        //solution.
        //Similarities can be found in other DataStore files. 
        
        readonly string _tableName;

        public ProjectDataStore() {
            var basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            _tableName = "Data source=" + Path.Combine(basePath, "database.db");
        }

        public async Task<bool> AddItemAsync(Project item) {
            using (var db = new SqliteConnection(_tableName)) {
                db.Open();
                Console.WriteLine("HELLLLLLLLLLO");
                var command = db.CreateCommand();
                Console.WriteLine("HELLLLLLLLLLO");
                command.CommandText =
                    @"
                        INSERT INTO projects (name, photo, pattern, done)
                        VALUES ($name, $photo, $pattern, $done);
                    ";
                Console.WriteLine("HELLLLLLLLLLO");
                //command.Parameters.AddWithValue("$id", item.Id);
                command.Parameters.AddWithValue("$name", item.Name);
                command.Parameters.AddWithValue("$photo", item.Photo);
                command.Parameters.AddWithValue("$pattern", item.Pattern);
                command.Parameters.AddWithValue("$done", item.Done);

                await command.ExecuteNonQueryAsync();
            }

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Project item) {
            using (var db = new SqliteConnection(_tableName)) {
                db.Open();

                var command = db.CreateCommand();
                command.CommandText =
                    @"
                        UPDATE projects
                        SET name = $name, photo = $photo, pattern = $pattern, done = $done
                        WHERE id = $id;
                    ";

                Console.WriteLine(item.Id);

                command.Parameters.AddWithValue("$id", item.Id);
                command.Parameters.AddWithValue("$name", item.Name);
                command.Parameters.AddWithValue("$photo", item.Photo);
                command.Parameters.AddWithValue("$pattern", item.Pattern);
                command.Parameters.AddWithValue("$done", item.Done);

                Console.WriteLine("tutaj jest błąd");
                await command.ExecuteNonQueryAsync();
                Console.WriteLine("Tutaj dokładnie");
            }

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id) {
            using (var db = new SqliteConnection(_tableName)) {
                db.Open();
                Console.WriteLine("Tutaj jest bład.....");
                var command = db.CreateCommand();
                command.CommandText =
                    @"
                        DELETE FROM projects WHERE id = $id;
                    ";

                command.Parameters.AddWithValue("$id", id);

                await command.ExecuteNonQueryAsync();
                Console.WriteLine("Tutaj jest bład.....");
            }

            return await Task.FromResult(true);
        }

        public async Task<Project> GetItemAsync(string id) {
            
            using (var db = new SqliteConnection(_tableName)) {
                db.Open();

                var command = db.CreateCommand();
                command.CommandText =
                    @"
                        SELECT *
                        FROM projects
                        WHERE id = $id;
                    ";

                command.Parameters.AddWithValue("$id", id);

                using (var reader = await command.ExecuteReaderAsync()) {
                    if (reader.Read()) {
                        int _id = reader.GetInt32(0);
                        string _name = reader.GetString(1);
                        string _photo = reader.GetString(2);
                        string _pattern = reader.GetString(3);
                        string _done = reader.GetString(4);

                        return await Task.FromResult(new Project
                        {
                            Id = _id,
                            Name = _name,
                            Photo = _photo,
                            Pattern = _pattern,
                            Done = _done
                        });
                    } else {
                        throw new SqliteException("Item not found", 0);
                    }
                    
                }

            }
        }

        public async Task<IEnumerable<Project>> GetItemsAsync(bool forceRefresh = false) {
            var result = new ObservableCollection<Project>();
            
            using (var db = new SqliteConnection(_tableName)) {
                db.Open();
                
                var command = db.CreateCommand();
                command.CommandText =
                    @"
                        SELECT *
                        FROM projects;
                    ";

                using (var reader =  command.ExecuteReader()) {                  
                    while (reader.Read()) {
                        int _id = reader.GetInt32(0);
                        string _name = reader.GetString(1);
                        string _photo = reader.GetString(2);
                        string _pattern = reader.GetString(3);
                        string _done = reader.GetString(4);

                        result.Add(new Project {
                            Id = _id,
                            Name = _name,
                            Photo = _photo,
                            Pattern = _pattern,
                            Done = _done
                        });
                    }
                    
                }

                return await Task.FromResult(result);
            }
        }
    }
}
