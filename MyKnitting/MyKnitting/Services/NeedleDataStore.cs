using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyKnitting.Models;
using Microsoft.Data.Sqlite;
using System.IO;


namespace MyKnitting.Services {
    public class NeedleDataStore : IDataStore<Needle> {
        /*
        readonly ObservableCollection<Needle> needles;
        //readonly List<Project> projects; 
        public NeedleDataStore() {
            //projects = new List<Project>(new ProjectDataStore().GetItemsAsync().Result);
            needles = new ObservableCollection<Needle>() {
                new Needle { Id = 0, Owned = false, Length = 80, Size = 5, Type = "Proste" },
                new Needle { Id = 1, Owned = true, Length = 120, Size = 5, Type = "Do kabli" },
                new Needle { Id = 2, Owned = true, Length = 120, Size = 3, Type = "Do kabli" },
                new Needle { Id = 3, Owned = false, Length = 40, Size = 4, Type = "Proste" },
                new Needle { Id = 4, Owned = false, Length = 60, Size = 4, Type = "Proste" },
                new Needle { Id = 5, Owned = false, Length = 80, Size = 5, Type = "Proste" },
                new Needle { Id = 6, Owned = true, Length = 120, Size = 5, Type = "Do kabli" },
                new Needle { Id = 7, Owned = true, Length = 120, Size = 3, Type = "Do kabli" },
                new Needle { Id = 8, Owned = false, Length = 40, Size = 4, Type = "Proste" },
                new Needle { Id = 9, Owned = false, Length = 60, Size = 4,  Type = "Proste" },
                new Needle { Id = 10, Owned = false, Length = 80, Size = 5, Type = "Proste" },
                new Needle { Id = 11, Owned = true, Length = 120, Size = 5, Type = "Do kabli" },
                new Needle { Id = 12, Owned = true, Length = 120, Size = 3, Type = "Do kabli" },
                new Needle { Id = 13, Owned = false, Length = 40, Size = 4, Type = "Proste" },
                new Needle { Id = 14, Owned = false, Length = 60, Size = 4, Type = "Proste" },
                new Needle { Id = 15, Owned = false, Length = 80, Size = 5, Type = "Proste" },
                new Needle { Id = 16, Owned = true, Length = 120, Size = 5, Type = "Do kabli" },
                new Needle { Id = 17, Owned = true, Length = 120, Size = 3, Type = "Do kabli" },
                new Needle { Id = 18, Owned = false, Length = 40, Size = 4, Type = "Proste" },
                new Needle { Id = 19, Owned = false, Length = 60, Size = 4, Type = "Proste" },
                new Needle { Id = 20, Owned = false, Length = 80, Size = 5, Type = "Proste" },
                new Needle { Id = 21, Owned = true, Length = 120, Size = 5, Type = "Do kabli" },
                new Needle { Id = 22, Owned = true, Length = 120, Size = 3, Type = "Do kabli" },
                new Needle { Id = 23, Owned = false, Length = 40, Size = 4, Type = "Proste" },
                new Needle { Id = 24, Owned = false, Length = 60, Size = 4, Type = "Proste" },
                new Needle { Id = 25, Owned = false, Length = 80, Size = 5, Type = "Proste" },
                new Needle { Id = 26, Owned = true, Length = 120, Size = 5, Type = "Do kabli" },
                new Needle { Id = 27, Owned = true, Length = 120, Size = 3, Type = "Do kabli" },
                new Needle { Id = 28, Owned = false, Length = 40, Size = 4, Type = "Proste" },
                new Needle { Id = 29, Owned = false, Length = 60, Size = 4, Type = "Proste" }
            };
        }

        public async Task<bool> AddItemAsync(Needle item) {
            needles.Add(item);
            return await Task.FromResult(true);
        }
        public async Task<bool> UpdateItemAsync(Needle item) {
            var oldItem = (from newItem in needles
                          where newItem.Id == item.Id
                          select newItem).First();

            oldItem.Size = item.Size;
            oldItem.Type = item.Type;
            oldItem.Length = item.Length;
            oldItem.Owned = item.Owned;

            return await Task.FromResult(true);
        }
        public async Task<bool> DeleteItemAsync(string id) {
            try {
                var intId = int.Parse(id);
                needles.Remove(needles.Where(x => x.Id == intId).FirstOrDefault());
                return await Task.FromResult(true);

            } catch (Exception) {
                return await Task.FromResult(false);
            }
        }
        public async Task<Needle> GetItemAsync(string id) {
            var intId = int.Parse(id);
            return await Task.FromResult(needles.FirstOrDefault(item => item.Id == intId));
        }
        public async Task<IEnumerable<Needle>> GetItemsAsync(bool forceRefresh = false) {
           //Console.WriteLine("Hello");
            return await Task.FromResult(needles);
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

        public NeedleDataStore() {
            var basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            _tableName = "Data source=" + Path.Combine(basePath, "database.db");
        }
        
        public async Task<bool> AddItemAsync(Needle item) {
            using (var db = new SqliteConnection(_tableName)) {
                db.Open();

                var command = db.CreateCommand();
                command.CommandText =
                    @"
                        INSERT INTO needles (type, size, length, owned)
                        VALUES ($type, $size, $length, $owned);
                    ";

                command.Parameters.AddWithValue("$id", item.Id);
                command.Parameters.AddWithValue("$type", item.Type);
                command.Parameters.AddWithValue("$size", item.Size);
                command.Parameters.AddWithValue("$length", item.Length);
                command.Parameters.AddWithValue("$owned", item.Owned);

                await command.ExecuteNonQueryAsync();
            }

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Needle item) {
            using (var db = new SqliteConnection(_tableName)) {
                db.Open();

                var command = db.CreateCommand();
                command.CommandText =
                    @"
                        UPDATE needles
                        SET type = $type, size = $size, length = $length, owned = $owned
                        WHERE id = $id;
                    ";

                command.Parameters.AddWithValue("$id", item.Id);
                command.Parameters.AddWithValue("$type", item.Type);
                command.Parameters.AddWithValue("$size", item.Size);
                command.Parameters.AddWithValue("$length", item.Length);
                command.Parameters.AddWithValue("$owned", item.Owned);

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
                        DELETE FROM needles WHERE id = $id;
                    ";

                command.Parameters.AddWithValue("$id", id);

                await command.ExecuteNonQueryAsync();
            }

            return await Task.FromResult(true);
        }

        public async Task<Needle> GetItemAsync(string id) {
            using (var db = new SqliteConnection(_tableName)) {
                db.Open();

                var command = db.CreateCommand();
                command.CommandText =
                    @"
                        SELECT *
                        FROM needles
                        WHERE id = $id;
                    ";

                command.Parameters.AddWithValue("$id", id);

                using (var reader = await command.ExecuteReaderAsync()) {
                    if (reader.Read()) {
                        int _id = reader.GetInt32(0);
                        string _type = reader.GetString(1);
                        int _size = reader.GetInt32(2);
                        int _length = reader.GetInt32(3);
                        bool _owned = reader.GetBoolean(4);

                        return await Task.FromResult(new Needle
                        {
                            Id = _id,
                            Type = _type,
                            Size = _size,
                            Length = _length,
                            Owned = _owned
                        });
                    } else {
                        throw new SqliteException("Item not found", 0);
                    }
                    
                }

            }
        }

        public async Task<IEnumerable<Needle>> GetItemsAsync(bool forceRefresh = false) {
            var result = new ObservableCollection<Needle>();
            using (var db = new SqliteConnection(_tableName)) {
                db.Open();

                var command = db.CreateCommand();
                command.CommandText =
                    @"
                        SELECT *
                        FROM needles;
                    ";
                Console.WriteLine("Błąd w tym miejscu jest prawie pewny");
                using (var reader = await command.ExecuteReaderAsync()) {
                    while (reader.Read()) {
                        int _id = reader.GetInt32(0);
                        string _type = reader.GetString(1);
                        int _size = reader.GetInt32(2);
                        int _length = reader.GetInt32(3);
                        bool _owned = reader.GetBoolean(4);

                        result.Add(new Needle {
                            Id = _id,
                            Type = _type,
                            Size = _size,
                            Length = _length,
                            Owned = _owned
                        });
                    }
                    
                }

                return await Task.FromResult(result);
            }
        }
    }
}
