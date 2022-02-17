using System;
using System.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OverviewRkiData.Components.Database.MsSql;
using System.Diagnostics;
using System.Linq;

namespace OverviewRkiData.Test.MsSql
{
    [TestClass]
    public class DatabaseConnectorTest
    {
        private const string ConnectionString = @"Server=localhost;Database=rki-data;User Id=sa;Password=Pa55w0rd!;";

        [TestMethod]
        public void ReadDataFromMsSqlServer()
        {
            // arrange
            var connector = new DatabaseConnector(ConnectionString);
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

        [TestMethod]
        public void ReadDataFromQueryByStoreProcedure()
        {
            // arrange
            var connector = new DatabaseConnector(ConnectionString);
            string[][] sa2 = Array.Empty<string[]>();
            const string storeProcedure = "DECLARE @outputId as INT; " + 
                                          "EXECUTE CreateLandkreise @pDate = '03.09.2021', @oLandkreiseID = @outputId OUTPUT; " +
                                          "SELECT @outputId AS N'LandkreisId';";

            // act
            connector.ConnectAndDo(connect =>
            {
                sa2 = connector.ReadDataFromQuery(connect, storeProcedure);
                return 0;
            });

            // assert
            Assert.IsNotNull(sa2);
            var result = sa2.Select(w => w.Where(ww => ww != null).Select(s => s)).FirstOrDefault();
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.FirstOrDefault());
        }
        
        [TestMethod]
        public void ReadDataByStoreProcedure()
        {
            // arrange
            var connector = new DatabaseConnector(ConnectionString);
            const string storeProcedure = "DECLARE @outputId as INT; " + 
                                          "EXECUTE CreateLandkreise @pDate = '03.09.2021', @oLandkreiseID = @outputId OUTPUT; " +
                                          "SELECT @outputId AS N'LandkreisId';";
            SqlDataReader result = null;
            object resultId = null;
            
            // act
           connector.ConnectAndDo(connect =>
            {
                result = connector.ReadData(connect, storeProcedure);
                while (result.Read())
                {
                    for (int index = 0; index < result.GetColumnSchema().Count; index++)
                    {
                        resultId = result.GetValue(index);
                    }
                }
                return 0;
            });

            // assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(resultId);
            Assert.AreEqual(3, resultId);
        }

        [TestMethod]
        public void TestData()
        {
            var dt = DateTime.Now;

            var l = dt.ToFileTimeUtc();

            Debug.Print($"{l}");

            dt = DateTime.Parse("01.01.2008 00:00:00");

            l = dt.ToFileTimeUtc();

            Debug.Print($"{l}");
        }
    }
}
