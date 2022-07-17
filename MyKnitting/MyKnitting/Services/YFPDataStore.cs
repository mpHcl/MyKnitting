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
                        INSERT INTO yfp (projectid, yarnid)
                        VALUES ($projectid, $yarnid);
                    ";
                
                Console.WriteLine(item.Project.Id);
                Console.WriteLine(item.Yarn.Id);
                command.Parameters.AddWithValue("$projectid", item.Project.Id);
                command.Parameters.AddWithValue("$yarnid", item.Yarn.Id);


                await command.ExecuteNonQueryAsync();
                Console.WriteLine("ZAPISANO");
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
                        FROM yfp;
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
