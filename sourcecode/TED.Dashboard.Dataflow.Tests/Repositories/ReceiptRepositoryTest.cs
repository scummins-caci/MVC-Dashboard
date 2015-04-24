using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TED.Dashboard.Repository.Tests.Repositories
{
    using Dataflow.Repositories;


    [TestClass, Ignore]
    public class ReceiptRepositoryTest
    {        
        [TestMethod]
        public void GetReceipts_Test()
        {
            // setup
            var repository = new ReceiptRepository();

            // act
            var items = repository.GetPage(1, 20);

            // access
            Assert.AreEqual(20, items.Count());
        }

        [TestMethod]
        public void GetNext20Receipts_Test()
        {
            // setup
            var repository = new ReceiptRepository();

            // act
            var items = repository.GetPage(2, 20);

            // access
            Assert.AreEqual(20, items.Count());
        }

        [TestMethod]
        public void GetAllReceipts_Test()
        {
            // setup
            var repository = new ReceiptRepository();

            // act
            var items = repository.GetAll();

            // access
            Assert.AreEqual(41, items.Count());
        }

        [TestMethod]
        public void GetReceiptCount_Test()
        {
            // setup
            var repository = new ReceiptRepository();

            // act
            var count = repository.GetCount();

            // access
            Assert.IsTrue(count > 0);
        }
    }
}
