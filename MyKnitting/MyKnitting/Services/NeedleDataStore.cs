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
                command.CommandText = "DELETE FROM nfp WHERE needleid = $id;";
                command.Parameters.AddWithValue("$id", id);
                command.ExecuteNonQuery();
                command = db.CreateCommand();
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
                        string _owned = reader.GetString(4);

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

                using (var reader = await command.ExecuteReaderAsync()) {
                    while (reader.Read()) {
                        int _id = reader.GetInt32(0);
                        string _type = reader.GetString(1);
                        int _size = reader.GetInt32(2);
                        int _length = reader.GetInt32(3);
                        string _owned = reader.GetString(4);

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
