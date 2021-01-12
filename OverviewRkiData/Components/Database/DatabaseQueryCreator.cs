using System;
using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace OverviewRkiData.Components.Database
{
    internal class DatabaseQueryCreator
    {
        #region TABLE Create and Exist Querys

        /// <summary>
        /// Erstellt aus dem Daten Objekt den Befehl für das erstellen einer Tabelle.
        /// </summary>
        /// <typeparam name="T">Das Daten Objekt angeben.</typeparam>
        /// <param name="data">Instanz des Daten Objektes.</param>
        /// <returns>Gibt den fertige Befehl als Zeichenkette zurück.</returns>
        internal string GetCreateTableByDataObjectString<T>()
        {
            StringBuilder sb = new StringBuilder();
            Type type = ((T)Activator.CreateInstance(typeof(T))).GetType();

            // Name des Datenobjektes als Tabellenname verwenden.
            sb.Append("CREATE TABLE " + type.Name + "(");
            foreach (PropertyInfo item in type.GetProperties())
            {

                // Name der Eigenschaft verwenden für die Datenspalte.
                sb.Append(item.Name + " ");
                bool setType = false;

                if (item.Name == "Id")
                {
                    // Primary Key anlegen für das Automatische Hochzählen der Id Nummer
                    if (item.PropertyType == typeof(Int64)) { sb.Append("INTEGER PRIMARY KEY AUTOINCREMENT,"); setType = true; }
                }
                else
                {
                    // Daten Typ ermitteln und passenden Datentype für die Daten Tabelle festlegen.
                    if (item.PropertyType == typeof(int) || item.PropertyType == typeof(Int64)) { sb.Append("INT NOT NULL,"); setType = true; }
                    if (item.PropertyType == typeof(string)) { sb.Append("NVARCHAR NOT NULL,"); setType = true; }
                    if (item.PropertyType == typeof(DateTime)) { sb.Append("INTEGER NOT NULL,"); setType = true; }
                    if (item.PropertyType == typeof(decimal) || item.PropertyType == typeof(double))
                    {
                        sb.Append("DOUBLE NOT NULL,");
                        setType = true;
                    }
                    if (item.PropertyType == typeof(bool)) { sb.Append("INT,"); setType = true; }
                }

                if (!setType)
                {
                    throw new ArgumentException("Can recognize type");
                }
            }

            // Letztes Komma entfernen
            sb.Remove(sb.ToString().Length - 1, 1);
            sb.Append(")");

            return sb.ToString();
        }

        /// <summary>
        /// Stellt zu dem Typ den passenen Query zusammen um nach der Tabelle prüfen zu können.
        /// </summary>
        /// <typeparam name="T">Typ angeben.</typeparam>
        /// <returns>Gibt den fertige Abfrage String zurück.</returns>
        internal string GetTableExist<T>()
        {
            Type type = ((T)Activator.CreateInstance(typeof(T))).GetType();

            // Name des Datenobjektes als Tabellenname verwenden.
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT * ");
            sb.Append("FROM sqlite_master ");
            sb.Append("WHERE type='table' AND name='" + type.Name + "'");

            return sb.ToString();
        }

        #endregion

        #region INSERT

        /// <summary>
        /// Erstellt aus dem Daten Objekt und dessen hinterlegten Werten
        /// einen Insert Befehlt, um einen neuen Datensatz anlegen zu können.
        /// </summary>
        /// <typeparam name="T">Das Daten Objekt angeben.</typeparam>
        /// <param name="data">Fertiges Daten Objekt mit Daten übergeben.</param>
        /// <returns>Gibt den fertige Befehl als Zeichenkette zurück.</returns>
        internal string GetDataInsert<T>(T data)
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder sbValues = new StringBuilder();
            Type type = data.GetType();

            // Daten Felder benennen.
            sb.Append("INSERT INTO " + type.Name + " (");
            foreach (PropertyInfo item in type.GetProperties())
            {
                // Wenn nicht Id, und falls doch prüfe ob es ein int ist und wenn der Wert sich von -1 unterscheidet.
                if ((item.Name != "Id" && item.GetValue(data) is int &&
                    (int)item.GetValue(data) != -1) ||
                    item.GetValue(data) is string)
                {
                    sb.Append(item.Name + ",");
                    sbValues.Append("'" + item.GetValue(data).ToString() + "',");
                }
                else if (item.GetValue(data) is DateTime)
                {
                    sb.Append(item.Name + ",");

                    DateTime dt = (DateTime)item.GetValue(data);

                    sbValues.Append("'" + dt.Ticks + "',");
                }
                else if (item.GetValue(data) is decimal || item.GetValue(data) is double)
                {
                    sb.Append(item.Name + ",");
                    sbValues.Append("'" + item.GetValue(data).ToString().Replace(',', '.') + "',");
                }
                else if (item.Name != "Id" && item.GetValue(data) is Int64)
                {
                    sb.Append(item.Name + ",");
                    sbValues.Append("'" + item.GetValue(data).ToString() + "',");
                }
                else if (item.Name != "Id" && item.GetValue(data) is bool)
                {
                    sb.Append(item.Name + ",");
                    sbValues.Append("'" + item.GetValue(data).ToString() + "',");
                }
            }

            // Letztes Komma Entfernen
            sb.Remove(sb.ToString().Length - 1, 1);
            sb.Append(") ");
            // Letztes Komma entfernen
            sbValues.Remove(sbValues.ToString().Length - 1, 1);

            // Werte aus dem Daten Objekt eintragen.
            sb.Append("VALUES (");
            sb.Append(sbValues.ToString());
            sb.Append(")");

            StringBuilder sbSubList = new StringBuilder();
            // GOTO??
            // Was ist mit den Unter Klassen
            foreach (FieldInfo item in type.GetFields())
            {
                string resultSubList = string.Empty;
                if (item.GetValue(data) is IList)
                {

                }
                Debug.Print(item.Attributes.ToString());
            }

            return sb.ToString();
        }

        #endregion

        #region SELECT Query

        /// <summary>
        /// Erstellt aus dem Typ die Abfrage zusammen.
        /// </summary>
        /// <typeparam name="T">Das Daten Objekt angeben.</typeparam>
        /// <param name="data">Instanz des Daten Objektes.</param>
        /// <returns>Gibt den fertige Befehl als Zeichenkette zurück.</returns>
        internal string GetDataSelect<T>()
        {
            Type type = ((T)Activator.CreateInstance(typeof(T))).GetType();

            // Daten Felder Selektieren
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT ");
            foreach (PropertyInfo item in type.GetProperties())
            {
                sb.Append(item.Name + ",");
            }

            // Letztes Komma entfernen
            sb.Remove(sb.ToString().Length - 1, 1);

            // Aus Daten Tabelle
            sb.Append(" FROM " + type.Name);

            return sb.ToString();
        }

        /// <summary>
        /// Stellt aus dem Typ und der Id die Abfrage zusammen
        /// </summary>
        /// <typeparam name="T">Typ angeben</typeparam>
        /// <returns>Gibt die fertige Abfrage als String zurück.</returns>
        internal string GetDataSelectById<T>(Int64 id)
        {
            Type type = ((T)Activator.CreateInstance(typeof(T))).GetType();

            // Daten Felder Selektieren
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT ");
            foreach (PropertyInfo item in type.GetProperties())
            {
                sb.Append(item.Name + ",");
            }

            // Letztes Komma entfernen
            sb.Remove(sb.ToString().Length - 1, 1);

            // Aus Daten Tabelle
            sb.Append(" FROM " + type.Name + " ");

            // Datensatz mit der Id überschreiben.
            sb.Append("WHERE Id='" + id + "'");

            return sb.ToString();
        }

        #endregion

        #region UPDATE

        /// <summary>
        /// Erstellt aus dem Daten Typ den passenden Query.
        /// </summary>
        /// <typeparam name="T">Typ angeben</typeparam>
        /// <param name="data">Übergabe des daten Typ.</param>
        /// <returns>Gibt den fertigen String zurück.</returns>
        internal string GetDataUpdate<T>(T data)
        {
            Type type = data.GetType();
            string id = "-1";

            StringBuilder sb = new StringBuilder();
            sb.Append("UPDATE " + type.Name + " ");
            sb.Append("SET ");
            foreach (PropertyInfo item in type.GetProperties())
            {
                if (item.Name == "Id") { id = item.GetValue(data).ToString(); }

                if (item.GetValue(data) is bool)
                {
                    string b = (bool)item.GetValue(data) == true ? "1" : "0";
                    sb.Append(item.Name + "='" + b + "',");
                }
                else
                {
                    sb.Append(item.Name + "='" + item.GetValue(data).ToString() + "',");
                }
            }
            // Letztes Komma und dingens entfernen
            sb.Remove(sb.ToString().Length - 1, 1);
            sb.Append(" ");

            // Der zu überschreiben Datensatz mit der Id.
            sb.Append("WHERE Id='" + id + "';");

            return sb.ToString();
        }

        #endregion

        #region DELETE QUERY

        /// <summary>
        /// Stellt aus dem Typ und der Id Nummer die Lösch Abfrage ab.
        /// </summary>
        /// <typeparam name="T">Typ angeben.</typeparam>
        /// <param name="id">Id nummer des zu löschenden Datensatzes.</param>
        /// <returns></returns>
        internal string GetDataDelete<T>(Int64 id)
        {
            Type type = ((T)Activator.CreateInstance(typeof(T))).GetType();
            return this.GetDataDelete(type.Name, id);
        }

        /// <summary>
        /// Stellt aus dem Typ und dem Datenobjekt die Lösch Abfrage zusammen.
        /// </summary>
        /// <typeparam name="T">Typ angeben.</typeparam>
        /// <param name="data">Der zulöschende Datensatz.</param>
        /// <returns>Gibt die Abfrage als Zeichenkette zurück.</returns>
        internal string GetDataDelete<T>(T data)
        {
            Type type = data.GetType();
            return this.GetDataDelete(type.Name, ((ISQLiteData)data).Id);
        }

        /// <summary>
        /// Stellt aus den Tabellennamen und der Id Nummer die Lösch abfrage zusammen.
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        private string GetDataDelete(string tablename, Int64 id)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("DELETE ");
            sb.Append("FROM " + tablename + " ");
            sb.Append("WHERE Id='" + id.ToString() + "'");

            return sb.ToString();
        }

        #endregion
    }
}