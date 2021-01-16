using Microsoft.VisualStudio.TestTools.UnitTesting;
using OverviewRkiData.Components.Database;
using OverviewRkiData.Components.LegacyData;
using System;

namespace OverviewRkiData.Test
{
    [TestClass]
    public class InsertDataToSQLiteDatabaseTest
    {
        [TestMethod]
        public void InsertDataToDatabaseTest()
        {
            // arrange
            var testJsonFile = $"{Environment.CurrentDirectory}";
            var testDatabaseFile = $"{Environment.CurrentDirectory}\\testData.db";
            var componentLegacy = new InsertDataToSQLiteDatabase(testDatabaseFile, testJsonFile);

            // assert
            componentLegacy.Import();

            // assert
            var data = new RkiDatabaseConnector(testDatabaseFile).Select<RkiDataDb>();
            Assert.IsNotNull(data);
        }
    }
}
