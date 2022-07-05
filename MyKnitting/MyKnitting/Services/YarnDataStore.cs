using MyKnitting.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.ObjectModel;
using Microsoft.Data.Sqlite;
using System.IO;


namespace MyKnitting.Services {
    public class YarnDataStore : IDataStore<Yarn> {
        /*
        readonly ObservableCollection<Yarn> yarns;
        public YarnDataStore() {
            yarns = new ObservableCollection<Yarn>() {
                new Yarn {Id = 0, Amount=20, Color = "Blue", ColorCode="#221aa2", Material="Silk", Name="Knitting for Olive", Owned = false, Size = 4},
                new Yarn {Id = 1, Amount=10, Color = "Blue", ColorCode="BLUE", Material="Wool", Name="Knitting for Olive", Owned = false, Size = 4},
                new Yarn {Id = 2, Amount=30, Color = "Cyan",ColorCode="Cyan", Material="Cotton", Name="Knitting for Olive", Owned = false, Size = 4},
                new Yarn {Id = 3, Amount=10, Color = "Blue", ColorCode="#221ab1", Material="Cotton", Name="Knitting for Olive", Owned = false, Size = 4},
                new Yarn {Id = 4, Amount=2, Color = "Black", ColorCode="#111111", Material="Cotton+Silk", Name="Knitting for Olive", Owned = false, Size = 4},
                new Yarn {Id = 5, Amount=5, Color="Purple", ColorCode="Purple", Material="Moher", Name="Knitting for Olive", Owned = false, Size = 4},
                new Yarn {Id = 6, Amount=6, Color="Navy blue", ColorCode="#020859", Material="Silk", Name="Knitting for Olive", Owned = false, Size = 4},
                new Yarn {Id = 7, Amount=12, Color = "Aquamarine", ColorCode="Aquamarine", Material="Wool", Name="Knitting for Olive", Owned = false, Size = 4},
                new Yarn {Id = 8, Amount=1,  Color = "Gray",  ColorCode="#aabbcc", Material="Silk", Name="Knitting for Olive", Owned = false, Size = 4},
                new Yarn {Id = 9, Amount=2, Color="White", ColorCode="#FFFFFF", Material="Wool", Name="Knitting for Olive", Owned = false, Size = 4},
                new Yarn {Id = 10, Amount=2, Color ="Gray", ColorCode="#AAAAAA", Material="Cotton", Name="Knitting for Olive", Owned = false, Size = 4},
            };
        }
        
        public Task<bool> AddItemAsync(Yarn item) {
            yarns.Add(item);
            return Task.FromResult(true);
        }
        public Task<bool> UpdateItemAsync(Yarn item) {
            return null;
        }
        public Task<bool> DeleteItemAsync(string id) {
            yarns.Remove(yarns.Where(x => x.Id.ToString() == id).First());
            return Task.FromResult(true);
        }
        public async Task<Yarn> GetItemAsync(string id) {
            var intId = int.Parse(id);
            return await Task.FromResult(yarns.FirstOrDefault(item => item.Id == intId));
        }
        public async Task<IEnumerable<Yarn>> GetItemsAsync(bool forceRefresh = false) {
            return await Task.FromResult(yarns);
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

        public YarnDataStore() {
            var basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            _tableName = "Data source=" + Path.Combine(basePath, "database.db");
        }

        public async Task<bool> AddItemAsync(Yarn item) {
            using (var db = new SqliteConnection(_tableName)) {
                db.Open();

                var command = db.CreateCommand();
                command.CommandText =
                    @"
                        INSERT INTO yarns
                        VALUES ( $name, $color, $colorcode, $size, $material, $amount, $owned);
                    ";

                command.Parameters.AddWithValue("$id", item.Id);
                command.Parameters.AddWithValue("$name", item.Name);
                command.Parameters.AddWithValue("$color", item.Color);
                command.Parameters.AddWithValue("$colorcode", item.ColorCode);
                command.Parameters.AddWithValue("$size", item.Size);
                command.Parameters.AddWithValue("$material", item.Material);
                command.Parameters.AddWithValue("$amount", item.Amount);
                command.Parameters.AddWithValue("$owned", item.Owned);

                await command.ExecuteNonQueryAsync();
            }

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Yarn item) {
            using (var db = new SqliteConnection(_tableName)) {
                db.Open();

                var command = db.CreateCommand();
                command.CommandText =
                    @"
                        UPDATE yarns
                        SET name = $name, color = $color, colorcode = $colorcode, size = $size,
                            material = &material, amount = $amount, owned = $owned
                        WHERE id = $id;
                    ";

                command.Parameters.AddWithValue("$id", item.Id);
                command.Parameters.AddWithValue("$name", item.Name);
                command.Parameters.AddWithValue("$color", item.Color);
                command.Parameters.AddWithValue("$colorcode", item.ColorCode);
                command.Parameters.AddWithValue("$size", item.Size);
                command.Parameters.AddWithValue("$material", item.Material);
                command.Parameters.AddWithValue("$amount", item.Amount);
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
                        DELETE FROM yarns WHERE id = $id;
                    ";

                command.Parameters.AddWithValue("$id", id);

                await command.ExecuteNonQueryAsync();
            }

            return await Task.FromResult(true);
        }

        public async Task<Yarn> GetItemAsync(string id) {
            using (var db = new SqliteConnection(_tableName)) {
                db.Open();

                var command = db.CreateCommand();
                command.CommandText =
                    @"
                        SELECT *
                        FROM yarns
                        WHERE id = $id;
                    ";

                command.Parameters.AddWithValue("$id", id);

                using (var reader = await command.ExecuteReaderAsync()) {
                    if (reader.Read()) {
                        int _id = reader.GetInt32(0);
                        string _name = reader.GetString(1);
                        string _color = reader.GetString(2);
                        string _colorcode = reader.GetString(3);
                        int _size = reader.GetInt32(4);
                        string _material = reader.GetString(5);
                        int _amount = reader.GetInt32(6);
                        bool _owned = reader.GetBoolean(7);
                        

                        return await Task.FromResult(new Yarn {
                            Id = _id,
                            Name = _name,
                            Color = _color,
                            ColorCode = _colorcode,
                            Size = _size,
                            Material = _material,
                            Amount = _amount,
                            Owned = _owned
                        });
                    } else {
                        throw new SqliteException("Item not found", 0);
                    }
                    
                }

            }
        }

        public async Task<IEnumerable<Yarn>> GetItemsAsync(bool forceRefresh = false) {
            var result = new ObservableCollection<Yarn>();
            using (var db = new SqliteConnection(_tableName)) {
                db.Open();

                var command = db.CreateCommand();
                command.CommandText =
                    @"
                        SELECT *
                        FROM yarns;
                    ";

                using (var reader = await command.ExecuteReaderAsync()) {
                    while (reader.Read()) {
                        int _id = reader.GetInt32(0);
                        string _name = reader.GetString(1);
                        string _color = reader.GetString(2);
                        string _colorcode = reader.GetString(3);
                        int _size = reader.GetInt32(4);
                        string _material = reader.GetString(5);
                        int _amount = reader.GetInt32(6);
                        bool _owned = reader.GetBoolean(7);

                        result.Add(new Yarn {
                            Id = _id,
                            Name = _name,
                            Color = _color,
                            ColorCode = _colorcode,
                            Size = _size,
                            Material = _material,
                            Amount = _amount,
                            Owned = _owned
                        });
                    }    
                }

                return await Task.FromResult(result);
            }
        }
    }
}
