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
                        INSERT INTO yarns (name, color, colorcode, size, material, amount, owned)
                        VALUES ($name, $color, $colorcode, $size, $material, $amount, $owned);
                    ";

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
                command.CommandText = "DELETE FROM yfp WHERE yarnid = $id;";
                command.Parameters.AddWithValue("$id", id);
                command.ExecuteNonQuery();

                command = db.CreateCommand();

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
                        string _owned = reader.GetString(7);
                        

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
                        string _owned = reader.GetString(7);

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
