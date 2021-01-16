using Microsoft.VisualStudio.TestTools.UnitTesting;
using OverviewRkiData.Components.Database;

namespace OverviewRkiData.Test
{
    [TestClass]
    public class DatabaseQueryCreatorTest
    {
        [TestMethod]
        public void CreateQuery_Db_Test()
        {
            // arrange
            var component = new DatabaseQueryCreator();

            // act
            var query = component.GetCreateTableByDataObjectString<MyTestData>();

            // assert
            Assert.AreEqual("CREATE TABLE MyTestData(Id INTEGER PRIMARY KEY AUTOINCREMENT,MyText NVARCHAR NOT NULL)", query);
        }
    }
}