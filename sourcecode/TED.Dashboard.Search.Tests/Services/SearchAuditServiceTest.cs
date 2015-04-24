using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TED.Dashboard.Repository;
using TED.Dashboard.Search.Models;
using TED.Dashboard.Search.Services;

namespace TED.Dashboard.Services.Tests.Services
{
    [TestClass]
    public class SearchAuditServiceTest
    {
        private IReadOnlyRepository<SearchAuditCount> countRepository;
        private IReadOnlyRepository<SearchFilterCount> filterRepository;
        private IReadOnlyRepository<SearchOperandCount> operandRepository;
        private IReadOnlyRepository<UserSearchCount> userSearchRepository;
        private IPageableRepository<SearchAudit> auditRepository; 

        readonly List<SearchAuditCount> twoItemTest = new List<SearchAuditCount>
                {
                    new SearchAuditCount
                        {
                            SearchesExecuted = 2,
                            DaySearchesExecuted = DateTime.Parse("2015-01-08 12:00:00")
                        },
                    new SearchAuditCount
                        {
                            SearchesExecuted = 1,
                            DaySearchesExecuted = DateTime.Parse("2015-01-07 12:00:00")
                        },
                };

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

            var mockFilter = new Mock<IReadOnlyRepository<SearchFilterCount>>();
            mockFilter.Setup(x => x.GetAll()).Returns(filterCounts);
            filterRepository = mockFilter.Object;


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

            var mockOperand = new Mock<IReadOnlyRepository<SearchOperandCount>>();
            mockOperand.Setup(x => x.GetAll()).Returns(operandCounts);
            operandRepository = mockOperand.Object;

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

            var mockUserCount = new Mock<IReadOnlyRepository<UserSearchCount>>();
            mockUserCount.Setup(x => x.GetAll()).Returns(userSearchCounts);
            userSearchRepository = mockUserCount.Object;

            var searchCounts = new List<SearchAuditCount>
                {
                    new SearchAuditCount
                        {
                            SearchesExecuted = 29,
                            DaySearchesExecuted = DateTime.Parse("2015-01-07 12:00:00")
                        },
                    new SearchAuditCount
                        {
                            SearchesExecuted = 12,
                            DaySearchesExecuted = DateTime.Parse("2015-01-06 12:00:00")
                        },
                    new SearchAuditCount
                        {
                            SearchesExecuted = 6,
                            DaySearchesExecuted = DateTime.Parse("2015-01-05 12:00:00")
                        },
                    new SearchAuditCount
                        {
                            SearchesExecuted = 7,
                            DaySearchesExecuted = DateTime.Parse("2014-12-31 12:00:00")
                        },
                    new SearchAuditCount
                        {
                            SearchesExecuted = 18,
                            DaySearchesExecuted = DateTime.Parse("2014-12-30 12:00:00")
                        },
                    new SearchAuditCount
                        {
                            SearchesExecuted = 12,
                            DaySearchesExecuted = DateTime.Parse("2014-12-28 12:00:00")
                        },
                    new SearchAuditCount
                        {
                            SearchesExecuted = 3,
                            DaySearchesExecuted = DateTime.Parse("2014-12-24 12:00:00")
                        }
                };

            var mock = new Mock<IReadOnlyRepository<SearchAuditCount>>();
            mock.Setup(x => x.GetAll()).Returns(searchCounts);

            countRepository = mock.Object;


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

            var auditMock = new Mock<IPageableRepository<SearchAudit>>();
            auditMock.Setup(x => x.GetPage(It.IsAny<int>(), It.IsAny<int>())).Returns(searchAudits);
            auditMock.Setup(x => x.GetAll()).Returns(searchAudits);
            auditMock.Setup(x => x.GetCount()).Returns(2);

            auditRepository = auditMock.Object;
        }

        [TestCleanup]
        public void Cleanup()
        {
            countRepository = null;
            auditRepository = null;
        }

        [TestMethod]
        public void GetSearchCounts_Test()
        {
            // setup
            var service = new SearchAuditService(countRepository, 
                                                filterRepository, 
                                                operandRepository, 
                                                userSearchRepository, auditRepository);
            
            // run
            var auditItems = service.GetSearchCounts(48);

            // assess
            // all 48 hours should be represented
            Assert.AreEqual(48, auditItems.Count());
        }

        [TestMethod]
        public void GetFilterCounts_Test()
        {
            // setup
            var service = new SearchAuditService(countRepository,
                                                filterRepository,
                                                operandRepository,
                                                userSearchRepository, auditRepository);

            // run
            var filterItems = service.GetFilterCounts();

            // assess
            // all 48 hours should be represented
            Assert.AreEqual(4, filterItems.Count());
        }

        [TestMethod]
        public void GetOperandCounts_Test()
        {
            // setup
            var service = new SearchAuditService(countRepository,
                                                filterRepository,
                                                operandRepository,
                                                userSearchRepository, auditRepository);

            // run
            var operandCount = service.GetOperandCounts();

            // assess
            // all 48 hours should be represented
            Assert.AreEqual(3, operandCount.Count());
        }

        [TestMethod]
        public void GetUserSearchCounts_Test()
        {
            // setup
            var service = new SearchAuditService(countRepository,
                                                filterRepository,
                                                operandRepository,
                                                userSearchRepository, auditRepository);

            // run
            var userSearchCounts = service.GetUserSearchCounts();

            // assess
            // all 48 hours should be represented
            Assert.AreEqual(4, userSearchCounts.Count());
        }

        [TestMethod]
        public void GetSearchCountsStartingWithLessThan48ItemGap_Test()
        {
            // setup
            var mock = new Mock<IReadOnlyRepository<SearchAuditCount>>();
            mock.Setup(x => x.GetAll()).Returns(twoItemTest);
            var service = new SearchAuditService(mock.Object,
                                                    filterRepository,
                                                operandRepository,
                                                userSearchRepository, auditRepository);

            // run
            var auditItems = service.GetSearchCounts(48);

            // assess
            // all 48 hours should be represented
            Assert.AreEqual(48, auditItems.Count());
        }

        [TestMethod]
        public void GetAuditPage_Test()
        {
            // setup
            var service = new SearchAuditService(countRepository,
                                                filterRepository,
                                                operandRepository,
                                                userSearchRepository, auditRepository);

            // run
            var auditPage = service.GetPagedSearches(1, 20);

            // assess
            Assert.AreEqual(2, auditPage.ItemCount);
            Assert.AreEqual(2, auditPage.Items.Count());
            
            // TODO:  test the object?

        }

    }
}
