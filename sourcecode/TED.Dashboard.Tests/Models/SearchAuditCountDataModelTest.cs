using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TED.Dashboard.Api.Models;

namespace TED.Dashboard.Tests.Models
{
    [TestClass]
    public class SearchAuditCountDataModelTest
    {
        [TestMethod]
        public void AuditDisplayDate_Test()
        {
            // setup
            var testModel = new SearchAuditCountDataModel
                {
                    SearchesExecuted = 5,
                    DaySearchesExecuted = DateTime.Parse("1/5/2015 6:00:00 PM")
                };

            // act
            var dayDisplay = testModel.DisplayExecuteDate;

            // assess
            Assert.AreEqual("1/5/2015", dayDisplay);
        }
    }
}
