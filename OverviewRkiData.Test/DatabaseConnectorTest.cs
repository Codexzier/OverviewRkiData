using Microsoft.VisualStudio.TestTools.UnitTesting;
using OverviewRkiData.Components.Database;
using System;
using System.IO;
using System.Linq;

namespace OverviewRkiData.Test
{
    [DoNotParallelize]
    [TestClass]
    public class DatabaseConnectorTest
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
            var result = connector.Create<MyTestData>();

            // assert
            Assert.IsTrue(result);
        }


        [TestMethod]
        public void TableNotExist()
        {
            // arrange
            this.Init(2);
            var connector = new RkiDatabaseConnector(string.Format(this._connectionString, 2));

            // act
            var result = connector.Exist<MyTestData>();

            // assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TableExist()
        {
            // arrange
            this.Init(3);
            var connector = new RkiDatabaseConnector(string.Format(this._connectionString, 3));
            connector.Create<MyTestData>();

            // act
            var result = connector.Exist<MyTestData>();

            // assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void InsertData()
        {
            // arrange
            this.Init(4);
            var connector = new RkiDatabaseConnector(string.Format(this._connectionString, 4));
            connector.Create<MyTestData>();

            // act
            var result = connector.Insert(new MyTestData { MyText = "Hallo" });

            // assert
            Assert.IsTrue(result);
        }


        [TestMethod]
        public void SelectEmptyData()
        {
            // arrange
            this.Init(5);
            var connector = new RkiDatabaseConnector(string.Format(this._connectionString, 5));
            connector.Create<MyTestData>();

            // act
            var result = connector.Select<MyTestData>();

            // assert
            Assert.IsTrue(!result.Any());
        }

        [TestMethod]
        public void SelectOneData()
        {
            // arrange
            this.Init(6);
            var connector = new RkiDatabaseConnector(string.Format(this._connectionString, 6));
            connector.Create<MyTestData>();
            connector.Insert(new MyTestData { MyText = "Test data" });

            // act
            var result = connector.Select<MyTestData>();

            // assert
            Assert.IsTrue(result.Any());
        }


        [TestMethod]
        public void SelectDataById()
        {
            // arrange
            this.Init(7);
            var connector = new RkiDatabaseConnector(string.Format(this._connectionString, 7));
            connector.Create<MyTestData>();
            connector.Insert(new MyTestData { MyText = "Test data 1" });
            var insertData = new MyTestData { MyText = "Test data 2" };
            connector.Insert(insertData);

            // act
            var result = connector.Select<MyTestData>(2L);

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(MyTestData), result.GetType());
            Assert.AreEqual(insertData.MyText, result.MyText);
        }

        [TestMethod]
        public void UpdateData()
        {
            // arrange
            this.Init(8);
            var connector = new RkiDatabaseConnector(string.Format(this._connectionString, 8));
            connector.Create<MyTestData>();
            connector.Insert(new MyTestData { MyText = "Test data 1" });
            connector.Insert(new MyTestData { MyText = "Test data 2" });
            var updateData = connector.Select<MyTestData>(2L);

            // act
            updateData.MyText = "new text";
            var resultUpdate = connector.Update(updateData);
            var result = connector.Select<MyTestData>(2L);

            // assert
            Assert.IsTrue(resultUpdate);
            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(MyTestData), result.GetType());
            Assert.AreEqual(updateData.MyText, result.MyText);
        }


        [TestMethod]
        public void DeleteData()
        {
            // arrange
            this.Init(9);
            var connector = new RkiDatabaseConnector(string.Format(this._connectionString, 9));
            connector.Create<MyTestData>();
            connector.Insert(new MyTestData { MyText = "Test data 1" });
            var deleteData = new MyTestData { MyText = "Test data 2" };
            connector.Insert(deleteData);
            var updateData = connector.Select<MyTestData>(2L);

            // act
            var resultDelete = connector.Delete(updateData);
            var result = connector.Select<MyTestData>();

            // assert
            Assert.IsTrue(resultDelete);
            Assert.IsNotNull(result);

            Assert.IsFalse(result.Any(a => a.MyText.Equals(deleteData.MyText)));
        }
    }
}
