using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TED.Dashboard.Api.Controllers;
using TED.Dashboard.Api.Models;
using TED.Dashboard.Search.Models;
using TED.Dashboard.Search.Services;
using TED.Dashboard.Services.Models;

namespace TED.Dashboard.Tests.Api.Controllers
{
    [TestClass]
    public class SearchAuditControllerTest
    {
        private ISearchAuditService service;

        [TestInitialize]
        public void Setup()
        {
            var filterCounts = new List<SearchFilterCount>
            {
                new SearchFilterCount
                {
                    Count = 219,
                     Filter = "Harmony Number"
                },
                new SearchFilterCount
                {
                    Count = 90,
                     Filter = "Text"
                },
                new SearchFilterCount
                {
                    Count = 50,
                     Filter = "Batch Name"
                },
                new SearchFilterCount
                {
                    Count = 34,
                     Filter = "Project"
                }
            };
            var mock = new Mock<ISearchAuditService>();
            mock.Setup(x => x.GetFilterCounts()).Returns(filterCounts);

            var operandCounts = new List<SearchOperandCount>
            {
                new SearchOperandCount
                {
                    Count = 1000,
                     Operand = "test%"
                },
                new SearchOperandCount
                {
                    Count = 40,
                     Operand = "bomb"
                },
                new SearchOperandCount
                {
                    Count = 20,
                     Operand = "test2"
                }
            };
            mock.Setup(x => x.GetOperandCounts()).Returns(operandCounts);

            var userSearchCounts = new List<UserSearchCount>
            {
                new UserSearchCount
                {
                    Count = 123,
                    UserName = "Test User 1"
                },
                new UserSearchCount
                {
                    Count = 90,
                    UserName = "Test User 2"
                },
                new UserSearchCount
                {
                    Count = 40,
                    UserName = "Test User 3"
                },
                new UserSearchCount
                {
                    Count = 11,
                    UserName = "Test User 4"
                }
            };
            mock.Setup(x => x.GetUserSearchCounts()).Returns(userSearchCounts);

            var searchCounts = new List<SearchAuditCount>
                {
                    new SearchAuditCount
                        {
                            SearchesExecuted = 1,
                            DaySearchesExecuted = DateTime.Parse("2015-01-07 12:00:00")
                        },
                    new SearchAuditCount
                        {
                            SearchesExecuted = 9,
                            DaySearchesExecuted = DateTime.Parse("2015-01-07 11:00:00")
                        },
                    new SearchAuditCount
                        {
                            SearchesExecuted = 9,
                            DaySearchesExecuted = DateTime.Parse("2015-01-07 10:00:00")
                        },
                    new SearchAuditCount
                        {
                            SearchesExecuted = 1,
                            DaySearchesExecuted = DateTime.Parse("2015-01-07 9:00:00")
                        },
                    new SearchAuditCount
                        {
                            SearchesExecuted = 3,
                            DaySearchesExecuted = DateTime.Parse("2015-01-07 8:00:00")
                        },
                    new SearchAuditCount
                        {
                            SearchesExecuted = 2,
                            DaySearchesExecuted = DateTime.Parse("2015-01-07 7:00:00")
                        },
                    new SearchAuditCount
                        {
                            SearchesExecuted = 2,
                            DaySearchesExecuted = DateTime.Parse("2015-01-07 6:00:00")
                        }
                };

            mock.Setup(x => x.GetSearchCounts(It.IsAny<int>())).Returns(searchCounts);
            service = mock.Object;

            // build audit repository
            var searchAudits = new List<SearchAudit>
                {
                    new SearchAudit
                        {
                             UserName = "Test User 1",
                             DateExecuted = DateTime.Now.AddDays(-4),
                             Criteria = new List<SearchCriteria>
                                 {
                                     new SearchCriteria
                                         {
                                             Operator = "=",
                                             FilterName = "Harmony Number",
                                             Operands = new []{"HARM-TEST-%"}
                                         }
                                 }
                        },
                        new SearchAudit
                        {
                             UserName = "Test User 2",
                             DateExecuted = DateTime.Now.AddDays(-1),
                             Criteria = new List<SearchCriteria>
                                 {
                                     new SearchCriteria
                                         {
                                             Operator = "like",
                                             FilterName = "Cross Language Name Search",
                                             Operands = new []{"John", "Jake", "Jim"}
                                         },
                                     new SearchCriteria
                                         {
                                             Operator = "=",
                                             FilterName = "Harmony Number",
                                             Operands = new []{"HARM-TEST-%"}
                                         }
                                 }
                        }
                };

            var data = new PageableItem<SearchAudit>
            {
                ItemCount = searchAudits.Count(),
                Items = searchAudits
            };
            mock.Setup(x => x.GetPagedSearches(It.IsAny<int>(), It.IsAny<int>())).Returns(data);
        }

        [TestCleanup]
        public void Cleanup()
        {
            service = null;
        }

        [TestMethod]
        public void AuditCounts_Test()
        {
            // setup
            var controller = new SearchAuditController(service);

            // act
            var results = controller.Counts();

            // assess
            Assert.AreEqual(200, results.Status);
            Assert.AreEqual(true, results.Success);

            // test to make sure correct object type is returned
            Assert.IsInstanceOfType(results.Data, typeof(IEnumerable<SearchAuditCountDataModel>));
            var data = results.Data as IEnumerable<SearchAuditCountDataModel>;

            Assert.AreEqual(7, data.Count());
        }

        [TestMethod]
        public void FilterCounts_Test()
        {
            // setup
            var controller = new SearchAuditController(service);

            // act
            var results = controller.TopFilters();

            // assess
            Assert.AreEqual(200, results.Status);
            Assert.AreEqual(true, results.Success);

            // test to make sure correct object type is returned
            Assert.IsInstanceOfType(results.Data, typeof(IEnumerable<SearchFilterCount>));
            var data = results.Data as IEnumerable<SearchFilterCount>;

            Assert.AreEqual(4, data.Count());
        }

        [TestMethod]
        public void OperandCounts_Test()
        {
            // setup
            var controller = new SearchAuditController(service);

            // act
            var results = controller.TopSearches();

            // assess
            Assert.AreEqual(200, results.Status);
            Assert.AreEqual(true, results.Success);

            // test to make sure correct object type is returned
            Assert.IsInstanceOfType(results.Data, typeof(IEnumerable<SearchOperandCount>));
            var data = results.Data as IEnumerable<SearchOperandCount>;

            Assert.AreEqual(3, data.Count());
        }

        [TestMethod]
        public void UserSearchCounts_Test()
        {
            // setup
            var controller = new SearchAuditController(service);

            // act
            var results = controller.TopUsers();

            // assess
            Assert.AreEqual(200, results.Status);
            Assert.AreEqual(true, results.Success);

            // test to make sure correct object type is returned
            Assert.IsInstanceOfType(results.Data, typeof(IEnumerable<UserSearchCount>));
            var data = results.Data as IEnumerable<UserSearchCount>;

            Assert.AreEqual(4, data.Count());
        }

        [TestMethod]
        public void Searches_Test()
        {
            // setup
            var controller = new SearchAuditController(service);

            // act
            var results = controller.Searches(1, 20);

            // assess
            Assert.AreEqual(200, results.Status);
            Assert.AreEqual(true, results.Success);

            // test to make sure correct object type is returned
            Assert.IsInstanceOfType(results.Data, typeof(PageableItemDataModel<SearchAuditDataModel>));
            var data = results.Data as PageableItemDataModel<SearchAuditDataModel>;

            Assert.AreEqual(2, data.Items.Count());
            Assert.AreEqual(2, data.ItemCount);
        }
    }
}
