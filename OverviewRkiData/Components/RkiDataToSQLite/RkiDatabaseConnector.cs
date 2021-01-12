using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace OverviewRkiData.Components.RkiDataToSQLite
{
    public class RkiDatabaseConnector
    {

        public RkiDatabaseConnector()
        {
            CreateTable();
            WriteHello();
            ReadHello();
        }

        private void CreateTable()
        {
            using var connection = new SqliteConnection("Data Source=hello.db");
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"CREATE TABLE IF NOT EXISTS user (name VARCHAR(60) NOT NULL)";

            command.ExecuteNonQuery();
        }

        private void WriteHello()
        {
            using var connection = new SqliteConnection("Data Source=hello.db");
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"INSERT INTO user (name) VALUES ('Tiggel')";

            command.ExecuteNonQuery();
        }

        private static void ReadHello()
        {
            using (var connection = new SqliteConnection("Data Source=hello.db"))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText =
                    @"
        SELECT name
        FROM user
    ";
               // command.Parameters.AddWithValue("$id", 1);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var name = reader.GetString(0);

                        Debug.WriteLine($"Hello, {name}!");
                    }
                }
            }
        }


        // private SqliteConnection _connection;

        //public void Init()
        //{
        //    string path = $"{Environment.CurrentDirectory}\\rkiDatabase.db";
        //    this._connection = new SQLiteConnection(path);
        //    this._connection.CreateTable(typeof(RkiDataDb));
        //    this._connection.CreateTable<LandkreisDb>();
        //    this._connection.CreateTable<DataValuesDb>();
        //}

        //public RkiDataDb Load(DateTime from) => this._connection
        //    .Table<RkiDataDb>()
        //        .First(w => w
        //            .StateTime.ToShortDateString()
        //            .Equals(from.ToShortDateString()));

        //public IEnumerable<DataValuesDb> GetHistorie(string landkreis)
        //{
        //    var table = this._connection.Table<LandkreisDb>();
        //    var landkreisId = table.FirstOrDefault(f => f.Name.Equals(landkreis));
        //    var tableValues = this._connection.Table<DataValuesDb>();
        //    return tableValues.Select(s => s);
        //}

        //internal IEnumerable<RkiDataDb> LoadAll() => this._connection
        //    .Table<RkiDataDb>();
        //internal IEnumerable<LandkreisDb> AllLandkreise() => this._connection
        //    .Table<LandkreisDb>();
        //public void Add(RkiDataDb rkiDataDb) => this._connection.Insert(rkiDataDb);
        //public int Add(DataValuesDb dataValuesDb)
        //{
        //    var result = this._connection.Insert(dataValuesDb);
        //    return result == 1 ? dataValuesDb.Id : 0;
        //}
        
    }
}
