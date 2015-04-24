using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TED.Dashboard.Api.Controllers;
using TED.Dashboard.Api.Models;
using TED.Dashboard.Dataflow.Services;
using TED.Dashboard.Dataflow.Models;

using TED.Dashboard.Services.Models;
using Moq;

namespace TED.Dashboard.Tests.Api.Controllers
{
    [TestClass]
    public class DataflowControllerTest
    {
        private IDataflowService service;
        
        [TestInitialize]
        public void Setup()
        {

            #region test data

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

            var receiptItems = new PageableItem<Receipt>
                {
                    ItemCount = receipts.Count,
                    Items = receipts
                };


            var errorLogs = new List<ErrorEntry>
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

            var errorItems = new PageableItem<ErrorEntry>
            {
                ItemCount = errorLogs.Count,
                Items = errorLogs
            };

            var changeInfo = new DataflowChangeInfo
                {
                    BinaryRoutes = new List<BinaryRouteInfo>
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
                        },
                    DocumentMetaRoutes = new List<MetaRouteInfo>
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
                        },
                    ScanRoutes = new List<ScanRouteInfo>
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
                                    DestinationId =Guid.NewGuid().ToString(),
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
                        }
                };
#endregion

            var mock = new Mock<IDataflowService>();
            mock.Setup(x => x.GetReceipts(It.IsAny<int>(), It.IsAny<int>())).Returns(receiptItems);
            mock.Setup(x => x.GetDataflowErrors(It.IsAny<int>(), It.IsAny<int>())).Returns(errorItems);
            mock.Setup(x => x.GetChangeInformation(It.IsAny<int>())).Returns(changeInfo);

            service = mock.Object;
        }

        [TestCleanup]
        public void Cleanup()
        {
            service = null;
        }

        [TestMethod]
        public void GetReceipts_Test()
        {
            var controller = new DataflowController(service);

            // act
            var results = controller.Receipts();

            // assess
            Assert.AreEqual(200, results.Status);
            Assert.AreEqual(true, results.Success);

            // test to make sure correct object type is returned
            Assert.IsInstanceOfType(results.Data, typeof(PageableItemDataModel<ReceiptDataModel>));
            var data = results.Data as PageableItemDataModel<ReceiptDataModel>;

            Assert.AreEqual(4, data.ItemCount);

        }

        [TestMethod]
        public void GetErrors_Test()
        {
            var controller = new DataflowController(service);

            // act
            var results = controller.Errors();

            // assess
            Assert.AreEqual(200, results.Status);
            Assert.AreEqual(true, results.Success);

            // test to make sure correct object type is returned
            Assert.IsInstanceOfType(results.Data, typeof(PageableItemDataModel<ErrorEntry>));
            var data = results.Data as PageableItemDataModel<ErrorEntry>;

            Assert.AreEqual(3, data.ItemCount);

        }

        [TestMethod]
        public void GetChangeTracking_Test()
        {
            var controller = new DataflowController(service);

            // act
            var results = controller.ChangeTracking(1);

            // assess
            Assert.AreEqual(200, results.Status);
            Assert.AreEqual(true, results.Success);

            // test to make sure correct object type is returned
            Assert.IsInstanceOfType(results.Data, typeof(DataflowChangeInfo));
            var data = results.Data as DataflowChangeInfo;

            Assert.AreEqual(3, data.BinaryRoutes.Count());
            Assert.AreEqual(2, data.DocumentMetaRoutes.Count());
            Assert.AreEqual(3, data.ScanRoutes.Count());
        }
    }
}
