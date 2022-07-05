using MyKnitting.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;


namespace MyKnitting.Services {
    public class YFPDataStore : IDataStore<YarnsForProject> {
        /*
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
        
        */
        //To implement...
        //Only thing to do is configure app database in other place, probably somewhere in app.xml.cs 
        //and uncommnet this section.
        //There might be problems with id so make sure to take care of this, probably all you need to do 
        //is make a primary key in model, and give cotnrol of it to the database, and this will be the simpliest 
        //solution.
        //Similarities can be found in other DataStore files. 

        readonly string _tableName;

        public YFPDataStore() {
            var basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            _tableName = "Data source=" + Path.Combine(basePath, "database.db");
        }

        public async Task<bool> AddItemAsync(YarnsForProject item) {
            using (var db = new SqliteConnection(_tableName)) {
                db.Open();

                var command = db.CreateCommand();
                command.CommandText =
                    @"
                        INSERT INTO yfp
                        VALUES ($id, $projectid, $yarnid);
                    ";

                command.Parameters.AddWithValue("$id", item.Id);
                command.Parameters.AddWithValue("$projectid", item.Project.Id);
                command.Parameters.AddWithValue("$yarnid", item.Yarn.Id);


                await command.ExecuteNonQueryAsync();
            }

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(YarnsForProject item) {
            using (var db = new SqliteConnection(_tableName)) {
                db.Open();

                var command = db.CreateCommand();
                command.CommandText =
                    @"
                        UPDATE yfp
                        SET projectid = $projectid, yarnid = $yarnid
                        WHERE id = $id;
                    ";

                command.Parameters.AddWithValue("$id", item.Id);
                command.Parameters.AddWithValue("$projectid", item.Project.Id);
                command.Parameters.AddWithValue("$neeldeid", item.Yarn.Id);

                await command.ExecuteNonQueryAsync();
            }

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id) {
            using (var db = new SqliteConnection(_tableName)) {
                db.Open();

                var command = db.CreateCommand();
                command.CommandText =
                    @"
                        DELETE FROM yfp WHERE id = $id;
                    ";

                command.Parameters.AddWithValue("$id", id);

                await command.ExecuteNonQueryAsync();
            }

            return await Task.FromResult(true);
        }

        public async Task<YarnsForProject> GetItemAsync(string id) {
            using (var db = new SqliteConnection(_tableName)) {
                db.Open();

                var command = db.CreateCommand();
                command.CommandText =
                    @"
                        SELECT *
                        FROM yfp
                        WHERE id = $id;
                    ";

                command.Parameters.AddWithValue("$id", id);

                using (var reader = await command.ExecuteReaderAsync()) {
                    if (reader.Read()) {
                        int _id = reader.GetInt32(0);
                        string _projectid = reader.GetString(1);
                        string _yarnid = reader.GetString(2);


                        return await Task.FromResult(new YarnsForProject
                        {
                            Id = _id,
                            Project = new ProjectDataStore().GetItemAsync(_projectid).Result,
                            Yarn = new YarnDataStore().GetItemAsync(_yarnid).Result
                        });
                    }
                    else {
                        throw new SqliteException("Item not found", 0);
                    }

                }

            }
        }

        public async Task<IEnumerable<YarnsForProject>> GetItemsAsync(bool forceRefresh = false) {
            var result = new ObservableCollection<YarnsForProject>();
            using (var db = new SqliteConnection(_tableName)) {
                db.Open();

                var command = db.CreateCommand();
                command.CommandText =
                    @"
                        SELECT *
                        FROM nfp;
                    ";

                using (var reader = await command.ExecuteReaderAsync()) {
                    while (reader.Read()) {
                        int _id = reader.GetInt32(0);
                        string _projectid = reader.GetString(1);
                        string _yarnid = reader.GetString(2);


                        result.Add(new YarnsForProject
                        {
                            Id = _id,
                            Project = new ProjectDataStore().GetItemAsync(_projectid).Result,
                            Yarn = new YarnDataStore().GetItemAsync(_yarnid).Result
                        });
                    }
                }

                return await Task.FromResult(result);
            }
        }
    }
}
