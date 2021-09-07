using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace OverviewRkiData.Components.Database.MsSql
{
    public class DatabaseConnector
    {
        private readonly string _connectionString;

        public DatabaseConnector(string connectionString)
        {
            this._connectionString = connectionString;
        }

        public int ConnectAndDo(Func<SqlConnection, int> func)
        {
            var connection = new SqlConnection(this._connectionString);
            connection.Open();

            var count = func(connection);
            connection.Close();

            return count;
        }

        public int WriteData(SqlConnection connection, string query)
        {
            var adapter = new SqlDataAdapter();
            var command = new SqlCommand(query, connection);
            adapter.InsertCommand = command;
            adapter.InsertCommand.ExecuteNonQuery();
            command.Dispose();

            return 0;
        }

        public SqlDataReader ReadData(SqlConnection connection, string query)
        {
            var commmand = new SqlCommand(query, connection);
            return commmand.ExecuteReader();
        }

        public int UpdateData(SqlConnection connection, string query)
        {
            var adapter = new SqlDataAdapter();
            var commmand = new SqlCommand(query, connection);

            adapter.UpdateCommand = commmand;
            adapter.UpdateCommand.ExecuteNonQuery();
            commmand.Dispose();

            return 0;
        }


        public string[][] ReadDataFromQuery(SqlConnection connection, string query)
        {
            var commmand = new SqlCommand(query, connection);
            var reader = commmand.ExecuteReader();

            int columns = reader.GetColumnSchema().Count;
            var result = new List<string[]>();
            while (reader.Read())
            {
                var rowResult = new List<string>();
                for (int index = 0; index < columns; index++)
                {
                    rowResult.Add($"{reader.GetValue(index)}");
                }

                result.Add(rowResult.ToArray());
            }
            return result.ToArray();
        }
    }
}
