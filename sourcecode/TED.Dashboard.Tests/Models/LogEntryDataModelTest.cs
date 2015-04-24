using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TED.Dashboard.Api.Models;
using TED.Dashboard.Search.Models;

namespace TED.Dashboard.Tests.Models
{
    [TestClass]
    public class LogEntryDataModelTest
    {
        [TestMethod]
        public void ModelToDataModelConversion_Test()
        {
            var logEntry = new LogEntry
                {
                    ClientMachine = "TEST_CLIENT_MACHINE",
                    Location = "Test Location",
                    LogId = "a4110e3b-6cbf-44ae-ab43-130f320b3ca1",
                    Message = "Test log entry message",
                    Severity = "ERROR",
                    TimeStamp = DateTime.Now
                };

            // act
            var dataModel = LogEntryDataModel.BuildFromLogEntry(logEntry);

            // assess
            Assert.AreEqual(logEntry.ClientMachine, dataModel.ClientMachine);
            Assert.AreEqual(logEntry.Location, dataModel.Location);
            Assert.AreEqual(logEntry.LogId, dataModel.LogId);
            Assert.AreEqual(logEntry.Message, dataModel.Message);
            Assert.AreEqual(logEntry.Severity, dataModel.Severity);
            Assert.AreEqual(logEntry.TimeStamp, dataModel.TimeStamp);
        }
        
        #region display date tests

        [TestMethod]
        public void DisplayTime5MinutesAgo_Test()
        {
            var testModel = new LogEntryDataModel {TimeStamp = DateTime.Now.AddMinutes(-5)};

            // act
            var dateDisplay = testModel.DisplayTime;

            // assess
            Assert.AreEqual("5 minutes ago", dateDisplay);
        }

        [TestMethod]
        public void DisplayTime5HoursAgo_Test()
        {
            var testModel = new LogEntryDataModel { TimeStamp = DateTime.Now.AddHours(-5) };

            // act
            var dateDisplay = testModel.DisplayTime;

            // assess
            Assert.AreEqual("5 hours ago", dateDisplay);
        }

        [TestMethod]
        public void DisplayTime1DayAgo_Test()
        {
            var testModel = new LogEntryDataModel { TimeStamp = DateTime.Now.AddDays(-1) };

            // act
            var dateDisplay = testModel.DisplayTime;

            // assess
            Assert.AreEqual("1 day ago", dateDisplay);
        }

        [TestMethod]
        public void DisplayTime5DaysAgo_Test()
        {
            var testModel = new LogEntryDataModel { TimeStamp = DateTime.Now.AddDays(-5) };

            // act
            var dateDisplay = testModel.DisplayTime;

            // assess
            Assert.AreEqual("5 days ago", dateDisplay);
        }

        [TestMethod]
        public void DisplayTime1YearAgo_Test()
        {
            var testModel = new LogEntryDataModel { TimeStamp = DateTime.Now.AddYears(-1) };

            // act
            var dateDisplay = testModel.DisplayTime;

            // assess
            Assert.AreEqual("1 year ago", dateDisplay);
        }

        [TestMethod]
        public void DisplayTimeSecondsAgo_Test()
        {
            var testModel = new LogEntryDataModel { TimeStamp = DateTime.Now.AddSeconds(-10) };

            // act
            var dateDisplay = testModel.DisplayTime;

            // assess
            Assert.AreEqual("10 seconds ago", dateDisplay);
        }

        [TestMethod]
        public void DisplayTimeJustNow_Test()
        {
            var testModel = new LogEntryDataModel { TimeStamp = DateTime.Now.AddSeconds(-4) };

            // act
            var dateDisplay = testModel.DisplayTime;

            // assess
            Assert.AreEqual("just now", dateDisplay);
        }

        #endregion

        #region Display Location tests

        [TestMethod]
        public void DisplayLocationTextExtraction_Test()
        {
            var testModel = new LogEntryDataModel { Location = "HighView.Services.TextExtraction.Service" };

            // act
            var location = testModel.DisplayLocation;

            // assess
            Assert.AreEqual("TextExtraction", location);
        }

        [TestMethod]
        public void DisplayLocationOCR_Test()
        {
            var testModel = new LogEntryDataModel { Location = "HighView.Services.OCR.Service" };

            // act
            var location = testModel.DisplayLocation;

            // assess
            Assert.AreEqual("OCR", location);
        }

        [TestMethod]
        public void DisplayLocationServiceHost_Test()
        {
            var testModel = new LogEntryDataModel { Location = "HighView.ServiceHost.Runner..ctor()" };

            // act
            var location = testModel.DisplayLocation;

            // assess
            Assert.AreEqual("Runner", location);
        }

        [TestMethod]
        public void DisplayLocationWorkflowHost_Test()
        {
            var testModel = new LogEntryDataModel { Location = "HighView.WorkflowHost.ServiceRunner..ctor()" };

            // act
            var location = testModel.DisplayLocation;

            // assess
            Assert.AreEqual("ServiceRunner", location);
        }

        #endregion
    }
}
