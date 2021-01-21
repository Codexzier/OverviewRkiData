using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace OverviewRkiData.Components.Database
{
    public class DatabaseQueryCreator
    {
        #region TABLE Create and Exist Querys

        /// <summary>
        /// Creates the command for creating a table from the data object.
        /// </summary>
        /// <typeparam name="T">Specify the data object.</typeparam>
        /// <returns>Returns the finished command as a string.</returns>
        public string GetCreateTableByDataObjectString<T>(out string[] extendArrayDataToTableQueries)
        {
            var type = typeof(T);

            return this.GetCreateTableByDataObjectString(type, out extendArrayDataToTableQueries);
        }
        
        public string GetCreateTableByDataObjectString(Type type, out string[] extendArrayDataToTableQueries)
        {
            extendArrayDataToTableQueries = null;
            if (type == null)
            {
                throw new DatabaseQueryCreatorException("Can not create table. The type is not valid for create a query.");
            }

            var listExtendDataToTablesQueries = new List<string>();

            var sb = new StringBuilder();
            // Use the name of the data object as the table name.
            sb.Append($"CREATE TABLE {type.Name}(");
            foreach (var item in type.GetProperties())
            {
                // Use property name for the column.
                sb.Append($"{item.Name}");
                bool setType = false;

                // TODO: need refactor this :D
                if (IsPrimaryKeyAndAutoIncrement(item.CustomAttributes))
                {
                    // Create primary key for automatic incrementing of the Id number
                    if (item.PropertyType == typeof(Int64)) { sb.Append(" INTEGER PRIMARY KEY AUTOINCREMENT,"); setType = true; }
                }
                else
                {
                    // Determine data type and set the appropriate data type for the data table.
                    if (item.PropertyType == typeof(int) || item.PropertyType == typeof(Int64)) { sb.Append(" INT NOT NULL,"); setType = true; }
                    if (item.PropertyType == typeof(string)) { sb.Append(" NVARCHAR NOT NULL,"); setType = true; }
                    if (item.PropertyType == typeof(DateTime)) { sb.Append(" INTEGER NOT NULL,"); setType = true; }
                    if (item.PropertyType == typeof(decimal) || item.PropertyType == typeof(double))
                    {
                        sb.Append("DOUBLE NOT NULL,");
                        setType = true;
                    }
                    if (item.PropertyType == typeof(bool)) { sb.Append(" INT,"); setType = true; }

                    if (item.PropertyType == typeof(Int64[]) ||
                        item.PropertyType == typeof(Int32[]) ||
                        item.PropertyType == typeof(long[]))
                    {
                        // create subTable for the array
                        listExtendDataToTablesQueries.Add(
                            this.CreateTableByDataObjectString(
                                type.Name, 
                                item.Name, 
                                typeof(long)));
                        sb.Append($"Id INTEGER NOT NULL,");
                        setType = true;
                    }
                }

                if (!setType)
                {
                    throw new ArgumentException("Can recognize type");
                }
            }

            sb.Remove(sb.ToString().Length - 1, 1);
            sb.Append(")");

            // query for subTables
            extendArrayDataToTableQueries = listExtendDataToTablesQueries.ToArray();

            return sb.ToString();
        }

        public string CreateTableByDataObjectString(string tableName, string itemName, Type type)
        {
            var sb = new StringBuilder();
            sb.Append($"CREATE TABLE {tableName}_{itemName}(");
            sb.Append($"{tableName}_id, ");
            sb.Append($"{itemName}_item INTEGER NOT NULL");
            sb.Append(")");

            return sb.ToString();
        }

        private static bool IsPrimaryKeyAndAutoIncrement(IEnumerable<CustomAttributeData> itemCustomAttributes) =>
            itemCustomAttributes
                .Count(w => w.AttributeType == typeof(PrimaryKeyAttribute) ||
                            w.AttributeType == typeof(AutoIncrementAttribute)) == 2;

        /// <summary>
        /// Assembles the appropriate query for the type to be able to check for the table.
        /// </summary>
        /// <typeparam name="T">Set type object.</typeparam>
        /// <returns>Returns the finished query string.</returns>
        public string GetTableExist<T>()
        {
            var type = typeof(T);

            return this.GetTableExist(type);
        }

        public string GetTableExist(Type type)
        {
            if (type == null)
            {
                throw new DatabaseQueryCreatorException("Can not check table exist. The type is not valid for create a query.");
            }

            var sb = new StringBuilder();
            sb.Append("SELECT * ");
            sb.Append("FROM sqlite_master ");
            sb.Append($"WHERE type='table' AND name='{type.Name}'");

            return sb.ToString();
        }

        #endregion

        #region INSERT

        /// <summary>
        /// Creates an insert command from the data object and
        /// its stored values in order to create a new data set.
        /// </summary>
        /// <typeparam name="T">Specify the data object.</typeparam>
        /// <param name="data">Pass finished data object with data.</param>
        /// <returns>Returns the finished command as a string.</returns>
        public string GetDataInsert<T>(T data, out string[] insertQueries)
        {
            insertQueries = null;
            if (data == null)
            {
                throw new DatabaseQueryCreatorException("Data can not be null. The type is not valid for create a query.");
            }

            var sb = new StringBuilder();
            var sbValues = new StringBuilder();
            var type = typeof(T);
            if (type == null)
            {
                throw new DatabaseQueryCreatorException("Can not insert query. The type is not valid for create a query.");
            }

            var collectionQuerySubTable = new List<string>();

            sb.Append($"INSERT INTO {type.Name} (");
            foreach (var item in type.GetProperties())
            {
                // TODO: Need refactor this :D
                // Wenn nicht Id, und falls doch prüfe ob es ein int
                // ist und wenn der Wert sich von -1 unterscheidet.
                if (IsPrimaryKeyAndAutoIncrement(item.CustomAttributes))
                {
                    continue;
                }
                else if (item.GetValue(data) is DateTime)
                {

                    var dt = (DateTime)item.GetValue(data);

                    sb.Append(item.Name + ",");
                    sbValues.Append($"'{dt.Ticks}',");
                }
                else if (item.GetValue(data) is decimal || item.GetValue(data) is double)
                {
                    sb.Append(item.Name + ",");
                    // TODO: Format by culture missing
                    sbValues.Append($"'{item.GetValue(data).ToString().Replace(',', '.')}',");
                }
                else if (item.PropertyType == typeof(Int64[]) ||
                         item.PropertyType == typeof(Int32[]) ||
                         item.PropertyType == typeof(long[]))
                {
                    // create subTable for the array
                    collectionQuerySubTable.AddRange(
                        this.GetInsertSubTableByDataObjectString(
                            type.Name,
                            item.Name,
                            (Int32[])item.GetValue(data)));
                    //sb.Append($"{item.Name},");
                    //sbValues.Append($"1,");
                }
                else
                {
                    sb.Append(item.Name + ",");
                    sbValues.Append($"'{item.GetValue(data)}',");
                }
            }

            if (sb.ToString().EndsWith(","))
            {
                sb.Remove(sb.ToString().Length - 1, 1);
            }
            sb.Append(") ");

            if (sbValues.Length > 0)
            {
                sbValues.Remove(sbValues.ToString().Length - 1, 1);
            }

            sb.Append("VALUES (");
            sb.Append(sbValues.ToString());
            sb.Append(")");
            
            // insert queries for subTable
            insertQueries = collectionQuerySubTable.ToArray();

            return sb.ToString();
        }

        private string[] GetInsertSubTableByDataObjectString(
            string tableName, 
            string itemName,
            Int32[] values)
        {
            var queries = new List<string>();
            // TODO: need optimized to the right sql statement for insert multiple data
            foreach (var value in values)
            {
                var sb = new StringBuilder();
                sb.Append($"INSERT INTO {tableName}_{itemName}({tableName}_id) ");
                sb.Append("VALUES (");
                sb.Append($"@{tableName}_id@, ");
                sb.Append($"{value}");
                sb.Append(")");
                queries.Add(sb.ToString());
            }
            
            return queries.ToArray();
        }

        #endregion

        #region SELECT Query

        /// <summary>
        /// Creates the query from the type.
        /// </summary>
        /// <typeparam name="T">Specify the data object.</typeparam>
        /// <returns>Returns the finished command as a string.</returns>
        public string GetDataSelect<T>()
        {
            var type = typeof(T);

            if (type == null)
            {
                throw new DatabaseQueryCreatorException("Can not select on null type. The type is not valid for create a query.");
            }

            var sb = new StringBuilder();
            sb.Append("SELECT ");
            foreach (var item in type.GetProperties())
            {
                sb.Append(item.Name + ",");
            }

            sb.Remove(sb.ToString().Length - 1, 1);
            sb.Append(" FROM " + type.Name);

            return sb.ToString();
        }

        /// <summary>
        /// Assembles the query from the type and the id
        /// </summary>
        /// <typeparam name="T">Setup type</typeparam>
        /// <returns>Returns the completed query as a string.</returns>
        public string GetDataSelectById<T>(long id)
        {
            var type = typeof(T);
            if (type == null)
            {
                throw new DatabaseQueryCreatorException("Can not select on null type. The type is not valid for create a query.");
            }

            var sb = new StringBuilder();
            sb.Append("SELECT ");
            foreach (var item in type.GetProperties())
            {
                sb.Append(item.Name + ",");
            }

            sb.Remove(sb.ToString().Length - 1, 1);
            sb.Append(" FROM " + type.Name + " ");
            sb.Append("WHERE Id='" + id + "'");

            return sb.ToString();
        }

        #endregion

        #region UPDATE

        /// <summary>
        /// Creates the appropriate query from the data type.
        /// </summary>
        /// <typeparam name="T">Setup Type</typeparam>
        /// <param name="data">Passing the data type.</param>
        /// <returns>Returns the finished string.</returns>
        public string CreateQueryUpdateData<T>(T data)
        {
            if (data == null)
            {
                throw new DatabaseQueryCreatorException("Data can not be null. The type is not valid for create a query.");
            }

            var type = typeof(T);
            var primaryKeyAndAutoIncrementName = string.Empty;
            var primaryKeyAndAutoIncrement = "-1";

            var sb = new StringBuilder();
            sb.Append($"UPDATE {type.Name} ");
            sb.Append("SET ");
            foreach (var item in type.GetProperties())
            {
                if (IsPrimaryKeyAndAutoIncrement(item.CustomAttributes))
                {
                    primaryKeyAndAutoIncrement = item.GetValue(data)?.ToString();
                    primaryKeyAndAutoIncrementName = item.Name;
                }

                if (item.GetValue(data) is bool bResult)
                {
                    var bStr = bResult ? "1" : "0";
                    sb.Append($"{item.Name }=='{bStr}',");
                }
                else
                {
                    sb.Append($"{item.Name }='{item.GetValue(data)}',");
                }
            }

            sb.Remove(sb.ToString().Length - 1, 1);
            sb.Append(" ");

            sb.Append($"WHERE {primaryKeyAndAutoIncrementName}='{primaryKeyAndAutoIncrement}';");
            return sb.ToString();
        }

        #endregion

        #region DELETE QUERY

        /// <summary>
        /// Creates the delete query from the type and the Id number.
        /// </summary>
        /// <typeparam name="T">Set Type.</typeparam>
        /// <param name="id">Id number of the record to be deleted.</param>
        /// <returns>Returns the query as a string.</returns>
        public string GetDataDelete<T>(long id)
        {
            var type = typeof(T);
            return this.GetDataDelete(type.Name, id);
        }

        /// <summary>
        /// Assembles the delete query from the type and the data object.
        /// </summary>
        /// <typeparam name="T">Set Type.</typeparam>
        /// <param name="data">The record to be deleted.</param>
        /// <returns>Returns the query as a string.</returns>
        public string GetDataDelete<T>(T data)
        {
            var type = typeof(T);

            // TODO: can only handle with one of them
            var hasPrimaryKeyAndAutoIncrementValue = type
                .GetProperties()
                .FirstOrDefault(w => IsPrimaryKeyAndAutoIncrement(w.CustomAttributes));

            if (hasPrimaryKeyAndAutoIncrementValue == null)
            {
                throw new DatabaseQueryCreatorException("");
            }

            // TODO: is only long not other type for primary key.
            return this.GetDataDelete(type.Name, (long)hasPrimaryKeyAndAutoIncrementValue.GetValue(data));
        }

        /// <summary>
        /// Assembles the delete query from the table names and the Id number.
        /// </summary>
        /// <param name="tableName">Set table name.</param>
        /// <param name="id">primary key of the data set.</param>
        /// <returns>Returns the query as a string.</returns>
        private string GetDataDelete(string tableName, Int64 id)
        {
            var sb = new StringBuilder();
            sb.Append("DELETE ");
            sb.Append($"FROM {tableName} ");
            sb.Append($"WHERE Id='{id}'");

            return sb.ToString();
        }

        #endregion
    }
}