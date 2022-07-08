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
        readonly string _tableName;

        public ProjectDataStore() {
            var basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            _tableName = "Data source=" + Path.Combine(basePath, "database.db");
        }

        public async Task<bool> AddItemAsync(Project item) {
            using (var db = new SqliteConnection(_tableName)) {
                db.Open();
                var command = db.CreateCommand();
                command.CommandText =
                    @"
                        INSERT INTO projects (name, photo, pattern, done)
                        VALUES ($name, $photo, $pattern, $done);
                    ";
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

                await command.ExecuteNonQueryAsync();
            }

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id) {
            using (var db = new SqliteConnection(_tableName)) {
                db.Open();
                var command = db.CreateCommand();
                command.CommandText = "DELETE FROM yfp WHERE projectid = $id;";
                command.Parameters.AddWithValue("$id", id);
                command.ExecuteNonQuery();

                command = db.CreateCommand();
                command.CommandText = "DELETE FROM nfp WHERE projectid = $id;";
                command.Parameters.AddWithValue("$id", id);
                command.ExecuteNonQuery();

                command = db.CreateCommand();
                command.CommandText =
                    @"
                        DELETE FROM projects WHERE id = $id;
                    ";

                command.Parameters.AddWithValue("$id", id);

                await command.ExecuteNonQueryAsync();
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
