using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;

namespace OverviewRkiData.Components.Database
{
    // TODO: need cleanup
    public class RkiDatabaseConnector
    {
        /// <summary>
        /// Provides methods to create or retrieve data from the given data object.
        /// </summary>
        private readonly DatabaseQueryCreator _querys = new DatabaseQueryCreator();

        /// <summary>
        /// Gets or sets the connection string.
        /// Normal connection string for connection to a SQL database.
        /// </summary>
        private readonly string _strCon;

        /// <summary>
        /// Gets or sets the debug switch.
        /// </summary>
        private readonly bool _debug;

        /// <summary>
        /// Standard constructor with specification about the data with the data. If the file does not exist, then it will be created.
        /// </summary>
        /// <param name="dataSourceFile">Name of the database file.</param>
        /// <param name="debug">Displays information about the operation in the outpu.</param>
        public RkiDatabaseConnector(string dataSourceFile, bool debug = false)
        {
            this._debug = debug;

            if (string.IsNullOrEmpty(dataSourceFile))
            {
                throw new NullReferenceException("The name of the dataSource file is null and that is not allowed!");
            }

            this._strCon = $"Data Source={dataSourceFile};Version=3;";
            this.DebugMessage("Connection String created.");
        }

        #region TABLE Create and Exist

        /// <summary>
        /// Creates the table from the data object.
        /// </summary>
        /// <typeparam name="T">Data object that is to be mapped as a table.</typeparam>
        /// <returns>Returns true if the table was created.</returns>
        public bool Create<T>()
        {
            var tableName = typeof(T).Name;

            var query = this._querys.GetCreateTableByDataObjectString<T>(out var subTableQueries);
            if (!this.Exist<T>())
            {
                this.DebugMessage($"created query: {query}");
                var result = this.ExecuteQuery(query);

                this.CreateSubTables(result, subTableQueries);

                return result == 0 || result == 1;
            }

            this.DebugMessage($"The table already exists: {tableName}");

            return false;
        }

        private void CreateSubTables(int result, string[] subTableQueries)
        {
            if (result == 1)
            {
                // create subTables
                foreach (var extendArrayDataToTableQuery in subTableQueries)
                {
                    var subTableResult = this.ExecuteQuery(extendArrayDataToTableQuery);

                    this.DebugMessage($"Create subTable: {subTableResult}");
                }
            }
        }

        public bool Create(Type type)
        {
            var tableName = type.Name;

            var query = this._querys.GetCreateTableByDataObjectString(type, out var subTableQueries);
            if (!this.Exist(type))
            {
                this.DebugMessage($"created query: {query}");
                var result = this.ExecuteQuery(query);
                return result == 0 || result == 1;
            }

            this.DebugMessage($"The table already exists: {tableName}");

            return false;
        }


        /// <summary>
        /// With the specification of the type, it can be queried whether the table has already been created.
        /// </summary>
        /// <typeparam name="T">Specify data object to be checked by.</typeparam>
        /// <returns>Returns true if the table exists.</returns>
        public bool Exist<T>()
        {
            var query = this._querys.GetTableExist<T>();
            return this.InternalExist(query);
        }

        public bool Exist(Type type)
        {
            var query = this._querys.GetTableExist(type);
            return this.InternalExist(query);
        }

        private bool InternalExist(string query)
        {
            this.DebugMessage($"created query: {query}");
            var result = 0;

            using var connection = new SQLiteConnection(this._strCon);
            connection.Open();
            this.DebugMessage("SQL Connection opened.");

            var command = new SQLiteCommand(query, connection);
            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                result = 1;
            }

            reader.Close();
            connection.Close();

            return result == 1;
        }

    

        #endregion

        #region INSERT

        /// <summary>
        /// Writes the record to the database.
        /// </summary>
        /// <typeparam name="T">Data object</typeparam>
        /// <param name="data">Data object with content information to save.</param>
        /// <returns>Returns true if a record could be saved.</returns>
        public bool Insert<T>(T data)
        {
            var query = this._querys.GetDataInsert(data, out var insertQueries);
            this.DebugMessage($"created query: {query}");
            var result = this.ExecuteQuery(query);
            
            return result == 1;
        }

        #endregion

        #region SELECT Select and Select by Id

        /// <summary>
        /// Gets the record of the table with indication of the data object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<T> Select<T>()
        {
            var query = this._querys.GetDataSelect<T>();
            this.DebugMessage($"Query zusammengestellt: {query}");

            var list = new List<T>();

            using var connection = new SQLiteConnection(this._strCon);
            connection.Open();
            this.DebugMessage("SQL Connection geöffnet.");

            var command = new SQLiteCommand(query, connection);
            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                // TODO: ganzen Block Auslagern und neu schreiben

                var tObject = (T)Activator.CreateInstance(typeof(T));
                if (tObject == null)
                {
                    break;
                }

                var type = tObject.GetType();
                var index = 0;
                var objectValue = reader.GetValue(index);
                
                foreach (var item in type.GetProperties())
                {
                    this.DebugTypeIdentification(objectValue);

                    // TODO: Warum habe ich das hier nochmal gemacht?
                    //if (item.PropertyType == typeof(DateTime))
                    //{
                    //    long ticks = (long)objectValue;
                    //    item.SetValue(tObject, new DateTime(ticks));
                    //}
                    //else if (item.PropertyType == typeof(decimal))
                    //{
                    //    decimal amount = Convert.ToDecimal(objectValue);
                    //    item.SetValue(tObject, amount);
                    //}
                    //else if (item.PropertyType == typeof(double))
                    //{
                    //    double amount = Convert.ToDouble(objectValue);
                    //    item.SetValue(tObject, amount);
                    //}
                    //else if (item.PropertyType == typeof(bool))
                    //{
                    //    bool b = Convert.ToBoolean(objectValue);
                    //    item.SetValue(tObject, b);
                    //}
                    ////else if(item.PropertyType == typeof(Int64))
                    ////{
                    ////    long l = Convert.ToInt64(objectValue);
                    ////    item.SetValue(tObject, l);
                    ////}
                    ////else if (item.PropertyType == typeof(string))
                    ////{
                    ////    item.SetValue(tObject, objectValue.ToString());
                    ////}
                    //else
                    //{
                    //    item.SetValue(tObject, objectValue);
                    //}
                    item.SetValue(tObject, reader.GetValue(index));

                    index++;
                }

                list.Add(tObject);
            }

            reader.Close();
            connection.Close();

            return list;
        }

        /// <summary>
        /// Get Data by id number.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public T Select<T>(long id)
        {
            var query = this._querys.GetDataSelectById<T>(id);
            this.DebugMessage($"created Query : {query}");

            var data = (T)Activator.CreateInstance(typeof(T));

            using var connection = new SQLiteConnection(this._strCon);
            connection.Open();
            this.DebugMessage("SQL Connection opened.");

            var command = new SQLiteCommand(query, connection);
            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                // TODO: ganzen block neu schreiben.
                if (data == null)
                {
                    continue;
                }

                var type = data.GetType();
                var index = 0;
                foreach (var item in type.GetProperties())
                {
                    object obj = reader.GetValue(index); 
                    this.DebugTypeIdentification(obj);
                    item.SetValue(data, reader.GetValue(index));

                    index++;
                }
            }

            reader.Close();
            connection.Close();

            return data;
        }

        #endregion

        #region Update

        /// <summary>
        /// Updates the passed data set
        /// </summary>
        /// <typeparam name="T">Set type</typeparam>
        /// <param name="data">Passing the data type</param>
        /// <returns>Returns true if the record was updated</returns>
        public bool Update<T>(T data)
        {
            var query = this._querys.CreateQueryUpdateData(data);
            this.DebugMessage($"created query: {query}");

            var result = this.ExecuteQuery(query);
            return result == 1;
        }

        #endregion

        #region DELETE

        /// <summary>
        /// Delete a data
        /// </summary>
        /// <typeparam name="T">Type that the record concerns.</typeparam>
        /// <param name="data">The record to be deleted.</param>
        /// <returns>Returns true if the record was deleted.</returns>
        public bool Delete<T>(T data)
        {
            var query = this._querys.GetDataDelete(data);
            this.DebugMessage($"created query: {query}");

            var result = this.ExecuteQuery(query);
            return result == 1;
        }

        #endregion

        #region SQL Excecutes

        /// <summary>
        /// Establishes a connection, executes the query and then closes the connection.
        /// </summary>
        /// <param name="query">Query to be executed</param>
        /// <returns>Returns the number of rows created, modified or deleted for the query.</returns>
        private int ExecuteQuery(string query)
        {
            using var connection = new SQLiteConnection(this._strCon);
            connection.Open();
            this.DebugMessage("SQL Connection geöffnet.");

            var command = new SQLiteCommand(query, connection);
            var result = command.ExecuteNonQuery();
            
            connection.Close();

            return result;
        }

        #endregion

        #region Debug and Check

        /// <summary>
        /// Checks the debug switch and outputs the message in the output if it is on.
        /// </summary>
        /// <param name="message">Message to be specified for the job.</param>
        private void DebugMessage(string message)
        {
            if (this._debug)
            {
                Debug.Print(message);
            }
        }

        /// <summary>
        /// Returns the type via the output, from the passed object.
        /// </summary>
        /// <param name="obj">set object for debug information.</param>
        private void DebugTypeIdentification(object obj)
        {
            if (!this._debug)
            {
                return;
            }

            if (obj == null)
            {
                Debug.WriteLine($"The object is null.");
                return;
            }

            Debug.WriteLine($"The type object is a {obj.GetType()}");
        }


        ///// <summary>
        ///// Prüft den Typ, ob das Interface implementiert wurde.
        ///// </summary>
        ///// <typeparam name="T">Typ angeben.</typeparam>
        ///// <returns>Gibt true zurück, wenn das Interface im Typ implementiert wurde.</returns>
        //private void CheckInterface<T>()
        //{
        //    bool result = (T)Activator.CreateInstance(typeof(T)) is ISQLiteData;

        //    if (!result) { throw new NotImplementedException("Für den Typ wurde das Interface ISQLiteData nicht implementiert."); }
        //}

        #endregion

    }
}
