using System;
using System.IO;
using System.Linq;
using System.Windows.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OverviewRkiData.Components.Database;

namespace OverviewRkiData.Test
{
    [DoNotParallelize]
    [TestClass]
    public class DatabaseConnectorSubTablesTest
    {
        private readonly string _connectionString = "Data Source=databaseTest{0}db.db";

        public void Init(int threadNumber)
        {
            var files = Directory.GetFiles($"{Environment.CurrentDirectory}");

            foreach (var file in files)
            {
                if (file.EndsWith(string.Format(this._connectionString, threadNumber)))
                {
                    File.Delete(file);
                }
            }
        }

        [TestMethod]
        public void CreateNewTable()
        {
            // arrange
            this.Init(1);
            var connector = new RkiDatabaseConnector(string.Format(this._connectionString, 1));

            // act
            var result = connector.Create<MyTestDataSubData>();

            // assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void InsertDataWithCollection()
        {
            // arrange
            this.Init(4);
            var connector = new RkiDatabaseConnector(string.Format(this._connectionString, 4));
            connector.Create<MyTestDataSubData>();

            // act
            connector.Insert(new MyTestDataSubData { CollectionOfInteger = new [] {12, 34, 56}});
            var result = connector.Select<MyTestDataSubData>();

            // assert
            Assert.IsNotNull(result);
            var data = result.First();
            Assert.IsNotNull(data.CollectionOfInteger);
            Assert.IsInstanceOfType(data.CollectionOfInteger, typeof(int[]));
            Assert.AreEqual(3, data.CollectionOfInteger.Length);
        }
    }
}