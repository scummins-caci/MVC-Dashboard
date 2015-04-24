using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TED.Dashboard.Repository;
using TED.Dashboard.Dataflow.Models;
using TED.Dashboard.Dataflow.Services;
using Moq;

namespace TED.Dashboard.Services.Test.Services
{
    [TestClass]
    public class DataflowServiceTest
    {
        private IPageableRepository<Receipt> repository;
        private IIDFilterRepository<MetaRouteInfo> dmrRepository;
        private IIDFilterRepository<BinaryRouteInfo> binRepository;
        private IIDFilterRepository<ScanRouteInfo> scnRepository;
        private IPageableRepository<ErrorEntry> errorRepository;

        [TestInitialize]
        public void Setup()
        {
            #region Test data

            var errors = new List<ErrorEntry>
                {
                    new ErrorEntry
                        {
                            HarmonyNumber = "TEST-HARM-0001",
                            ErrorCode = "400",
                            ErrorConstraint = "TrackingNumber",
                            ErrorMessage = "Duplicate Primary Key"
                        },
                                            new ErrorEntry
                        {
                            HarmonyNumber = "TEST-HARM-0001",
                            ErrorCode = "400",
                            ErrorConstraint = "TrackingNumber",
                            ErrorMessage = "Duplicate Primary Key"
                        },
                                            new ErrorEntry
                        {
                            HarmonyNumber = "TEST-HARM-0001",
                            ErrorCode = "400",
                            ErrorConstraint = "TrackingNumber",
                            ErrorMessage = "Duplicate Primary Key"
                        }
                };

            var receipts = new List<Receipt>
                {
                    new Receipt
                        {
                            Action = "I",
                            ChangeId = 100,
                            ExtractDate = DateTime.Now.AddDays(-4),
                            HarmonyNumber = "TEST-HARM-0001",
                            ReceiptType = "R"
                        },
                    new Receipt
                        {
                            Action = "I",
                            ChangeId = 103,
                            ExtractDate = DateTime.Now.AddDays(-4),
                            HarmonyNumber = "TEST-HARM-0002",
                            ReceiptType = "R"
                        },
                    new Receipt
                        {
                            Action = "I",
                            ChangeId = 102,
                            ExtractDate = DateTime.Now.AddDays(-4),
                            HarmonyNumber = "TEST-HARM-0003",
                            ReceiptType = "R"
                        },
                    new Receipt
                        {
                            Action = "I",
                            ChangeId = 101,
                            ExtractDate = DateTime.Now.AddDays(-4),
                            HarmonyNumber = "TEST-HARM-0004",
                            ReceiptType = "R"
                        }
                };

            var binaryRoutes = new List<BinaryRouteInfo>
                {
                    new BinaryRouteInfo
                        {
                            DestinationId = Guid.NewGuid().ToString(),
                            FileInfoSentOn = DateTime.Now.AddDays(-6),
                            Id = Guid.NewGuid().ToString(),
                            PathInfo = "c://temp//filesgohere//12321",
                            ProcessedOn = DateTime.Now.AddDays(-8),
                            ProcessedStatus = "SUCCESS",
                            XmlSentOn = DateTime.Now.AddDays(-3)
                        },
                    new BinaryRouteInfo
                        {
                            DestinationId = Guid.NewGuid().ToString(),
                            FileInfoSentOn = DateTime.Now.AddDays(-6),
                            Id = Guid.NewGuid().ToString(),
                            PathInfo = "c://temp//filesgohere//12322",
                            ProcessedOn = DateTime.Now.AddDays(-8),
                            ProcessedStatus = "FAILURE",
                            XmlSentOn = DateTime.Now.AddDays(-3)
                        },
                    new BinaryRouteInfo
                        {
                            DestinationId = Guid.NewGuid().ToString(),
                            FileInfoSentOn = DateTime.Now.AddDays(-6),
                            Id = Guid.NewGuid().ToString(),
                            PathInfo = "c://temp//filesgohere//12323",
                            ProcessedOn = DateTime.Now.AddDays(-8),
                            ProcessedStatus = "SUCCESS",
                            XmlSentOn = DateTime.Now.AddDays(-3)
                        },
                };

            var metaRoutes = new List<MetaRouteInfo>
                {
                    new MetaRouteInfo
                        {
                            DateCreated = DateTime.Now,
                            ChangeId = 100,
                            FilesSentOn = DateTime.Now.AddDays(-3),
                            HarmonyNumber = "TEST-HARM-000234",
                            Id = Guid.NewGuid().ToString(),
                            MetaId = Guid.NewGuid().ToString(),
                            Network = "EG",
                            ProcessedOn = DateTime.Now.AddDays(-1),
                            ProcessedStatus = "SUCCESS",
                            RoutedOn = DateTime.Now.AddDays(-4)
                        },
                    new MetaRouteInfo
                        {
                            DateCreated = DateTime.Now,
                            ChangeId = 100,
                            FilesSentOn = DateTime.Now.AddDays(-3),
                            HarmonyNumber = "TEST-HARM-000234",
                            Id = Guid.NewGuid().ToString(),
                            MetaId = Guid.NewGuid().ToString(),
                            Network = "EG",
                            ProcessedOn = DateTime.Now.AddDays(-1),
                            ProcessedStatus = "SUCCESS",
                            RoutedOn = DateTime.Now.AddDays(-4)
                        }
                };

            var scanRoutes = new List<ScanRouteInfo>
                {
                    new ScanRouteInfo
                        {
                            DestinationId = Guid.NewGuid().ToString(),
                            FileInfoSentOn = DateTime.Now.AddDays(-6),
                            Id = Guid.NewGuid().ToString(),
                            PathInfo = "c://temp//filesgohere//12321",
                            ProcessedOn = DateTime.Now.AddDays(-8),
                            ProcessedStatus = "SUCCESS",
                            XmlSentOn = DateTime.Now.AddDays(-3)
                        },
                    new ScanRouteInfo
                        {
                            DestinationId = Guid.NewGuid().ToString(),
                            FileInfoSentOn = DateTime.Now.AddDays(-6),
                            Id = Guid.NewGuid().ToString(),
                            PathInfo = "c://temp//filesgohere//12321",
                            ProcessedOn = DateTime.Now.AddDays(-8),
                            ProcessedStatus = "SUCCESS",
                            XmlSentOn = DateTime.Now.AddDays(-3)
                        },
                    new ScanRouteInfo
                        {
                            DestinationId = Guid.NewGuid().ToString(),
                            FileInfoSentOn = DateTime.Now.AddDays(-6),
                            Id = Guid.NewGuid().ToString(),
                            PathInfo = "c://temp//filesgohere//12321",
                            ProcessedOn = DateTime.Now.AddDays(-8),
                            ProcessedStatus = "SUCCESS",
                            XmlSentOn = DateTime.Now.AddDays(-3)
                        }
                };

            #endregion

            var receiptMock = new Mock<IPageableRepository<Receipt>>();
            receiptMock.Setup(x => x.GetAll()).Returns(receipts);
            receiptMock.Setup(x => x.GetPage(It.IsAny<int>(), It.IsAny<int>())).Returns(receipts);
            repository = receiptMock.Object;

            var dmr = new Mock<IIDFilterRepository<MetaRouteInfo>>();
            dmr.Setup(x => x.GetAll(It.IsAny<long>())).Returns(metaRoutes);
            dmrRepository = dmr.Object;

            var bin = new Mock<IIDFilterRepository<BinaryRouteInfo>>();
            bin.Setup(x => x.GetAll(It.IsAny<long>())).Returns(binaryRoutes);
            binRepository = bin.Object;

            var scn = new Mock<IIDFilterRepository<ScanRouteInfo>>();
            scn.Setup(x => x.GetAll(It.IsAny<long>())).Returns(scanRoutes);
            scnRepository = scn.Object;

            var error = new Mock<IPageableRepository<ErrorEntry>>();
            error.Setup(x => x.GetAll()).Returns(errors);
            error.Setup(x => x.GetCount()).Returns(errors.Count());
            error.Setup(x => x.GetPage(It.IsAny<int>(), It.IsAny<int>())).Returns(errors);
            errorRepository = error.Object;

        }

        [TestCleanup]
        public void Cleanup()
        {
            repository = null;
            dmrRepository = null;
            binRepository = null;
            scnRepository = null;
            errorRepository = null;
        }

        [TestMethod]
        public void GetReceipts_Test()
        {
            var service = new DataflowService(repository, errorRepository, dmrRepository, binRepository, scnRepository);

            // act
            var results = service.GetReceipts(1, 20);

            Assert.AreEqual(4, results.Items.Count());
        }

        [TestMethod]
        public void GetDataflowErrors_Test()
        {
            var service = new DataflowService(repository, errorRepository, dmrRepository, binRepository, scnRepository);

            // act
            var results = service.GetDataflowErrors(1, 20);

            Assert.AreEqual(3, results.Items.Count());
        }

        [TestMethod]
        public void GetChangeInfo_Test()
        {
            var service = new DataflowService(repository, errorRepository, dmrRepository, binRepository, scnRepository);

            // act
            var results = service.GetChangeInformation(100);

            Assert.AreEqual(3, results.BinaryRoutes.Count());
            Assert.AreEqual(2, results.DocumentMetaRoutes.Count());
            Assert.AreEqual(3, results.ScanRoutes.Count());
        }
    }
}
