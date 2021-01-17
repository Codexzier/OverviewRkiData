using Microsoft.VisualStudio.TestTools.UnitTesting;
using OverviewRkiData.Components.Database;

namespace OverviewRkiData.Test
{
    [TestClass]
    public class DatabaseQueryCreatorTest
    {
        [TestMethod]
        public void CreateQuery_CreateTable_Test()
        {
            // arrange
            var component = new DatabaseQueryCreator();

            // act
            var query = component.GetCreateTableByDataObjectString<MyTestData>();

            // assert
            Assert.AreEqual("CREATE TABLE MyTestData(Id INTEGER PRIMARY KEY AUTOINCREMENT,MyText NVARCHAR NOT NULL)", query);
        }

        [TestMethod]
        public void CreateQuery_GetTableExist_Test()
        {
            // arrange
            var component = new DatabaseQueryCreator();

            // act
            var query = component.GetTableExist<MyTestData>();

            // assert
            Assert.AreEqual("SELECT * FROM sqlite_master WHERE type='table' AND name='MyTestData'", query);
        }

        [TestMethod]
        public void CreateQuery_GetDataInsert_Test()
        {
            // arrange
            var component = new DatabaseQueryCreator();
            var testData = new MyTestData {MyText = "Hello"};

            // act
            var query = component.GetDataInsert(testData);

            // assert
            Assert.AreEqual("INSERT INTO MyTestData (MyText) VALUES ('Hello')", query);
        }

        [TestMethod]
        public void CreateQuery_GetDataSelect_Test()
        {
            // arrange
            var component = new DatabaseQueryCreator();

            // act
            var query = component.GetDataSelect<MyTestData>();

            // assert
            Assert.AreEqual("SELECT Id,MyText FROM MyTestData", query);
        }

        [TestMethod]
        public void CreateQuery_GetDataSelectById_Test()
        {
            // arrange
            var component = new DatabaseQueryCreator();

            // act
            var query = component.GetDataSelectById<MyTestData>(123);

            // assert
            Assert.AreEqual("SELECT Id,MyText FROM MyTestData WHERE Id='123'", query);
        }


        [TestMethod]
        public void CreateQuery_GetDataUpdate_Test()
        {
            // arrange
            var component = new DatabaseQueryCreator();
            var data = new MyTestData{ Id = 123, MyText = "Change text"};

            // act
            var query = component.CreateQueryUpdateData(data);

            // assert
            Assert.AreEqual("UPDATE MyTestData SET Id='123',MyText='Change text' WHERE Id='123';", query);
        }

        [TestMethod]
        public void CreateQuery_GetDataDelete_Test()
        {
            // arrange
            var component = new DatabaseQueryCreator();

            // act
            var query = component.GetDataDelete<MyTestData>(123L);

            // assert
            Assert.AreEqual("DELETE FROM MyTestData WHERE Id='123'", query);
        }

        [TestMethod]
        public void CreateQuery_GetDataDelete_ByData_Test()
        {
            // arrange
            var component = new DatabaseQueryCreator();
            var data = new MyTestData {Id = 123L, MyText = "For delete"};

            // act
            var query = component.GetDataDelete(data);

            // assert
            Assert.AreEqual("DELETE FROM MyTestData WHERE Id='123'", query);
        }
    }
}