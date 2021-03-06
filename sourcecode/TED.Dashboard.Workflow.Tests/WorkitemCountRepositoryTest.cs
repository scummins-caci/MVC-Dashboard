﻿using System.Configuration;
using HighView.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TED.Dashboard.Workflow.Repositories;

namespace TED.Dashboard.Repository.Tests.Repositories
{
    [TestClass, Ignore]
    public class WorkitemCountRepositoryTest
    {
        [ClassInitialize]
        public static void MyClassInitialize(TestContext testContext)
        {
            WinModule.Initialize();
            WinModule.CreateNewSession(ConfigurationManager.AppSettings["AuthenticationConnectString"],
                                       ConfigurationManager.AppSettings["UserName"],
                                       ConfigurationManager.AppSettings["Password"]);
        }

        //
        // Use ClassCleanup to run code after all tests in a class have run
        [ClassCleanup]
        public static void MyClassCleanup()
        {
            WinModule.DestroySession();
        }


        [TestMethod]
        public void GetCounts_Test()
        {
            var repository = new WorkItemCountRepository();

            // act
            var items = repository.GetAll();

            // access
            Assert.IsNotNull(items);
        }
    }
}
