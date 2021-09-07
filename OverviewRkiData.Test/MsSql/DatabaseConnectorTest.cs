using Microsoft.VisualStudio.TestTools.UnitTesting;
using OverviewRkiData.Components.Database.MsSql;
using System.Diagnostics;

namespace OverviewRkiData.Test.MsSql
{
    [TestClass]
    public class DatabaseConnectorTest
    {
        [TestMethod]
        public void ReadDataFromMsSqlServer()
        {
            // arrange
            var connector = new DatabaseConnector(@"Server=localhost;Database=rki-data;User Id=sa;Password=Pa55w0rd!;");
            var count = 0;

            // act
            connector.ConnectAndDo(connect =>
            {
                var result = connector.ReadDataFromQuery(connect, "SELECT * FROM Landkreise");
                foreach (var item in result)
                {
                    foreach (var col in item)
                    {
                        Debug.Write($"{col}, ");
                    }
                    Debug.WriteLine("");
                    count++;
                }
                return 0;
            });

            // assert
            Assert.IsTrue(count > 0);
        }
    }
}
