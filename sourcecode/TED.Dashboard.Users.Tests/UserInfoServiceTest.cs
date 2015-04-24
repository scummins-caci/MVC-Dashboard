using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TED.Dashboard.Repository;
using TED.Dashboard.Users;
using TED.Dashboard.Users.Services;

namespace TED.Dashboard.Services.Tests.Services
{
    [TestClass]
    public class UserInfoServiceTest
    {
        private IIDFilterRepository<Role> repository;

        [TestInitialize]
        public void Setup()
        {
            var roles = new List<Role>
                {
                    new Role
                        {
                            RoleName = "IsSysAdmin",
                            Value = true
                        },
                    new Role
                        {
                            RoleName = "IsWfQueueAdmin",
                            Value = true
                        },
                    new Role
                        {
                            RoleName = "CanSearchExternal",
                            Value = true
                        },
                    new Role
                        {
                            RoleName = "IsQaUser",
                            Value = false
                        }
                };

            var roleRepositoryMock = new Mock<IIDFilterRepository<Role>>();
            roleRepositoryMock.Setup(x => x.GetAll(It.IsAny<long>())).Returns(roles);
            repository = roleRepositoryMock.Object;
        }

        [TestCleanup]
        public void Cleanup()
        {
            repository = null;
        }

        [TestMethod]
        public void GetUserRoles_Test()
        {
            // setup 
            var service = new UserInfoService(repository);

            // act
            var results = service.GetUserRoles(100).ToArray();

            // assert
            // only two items should be returned since two are true
            Assert.AreEqual(6, results.Count());

            // make sure that only the true values are returned

            var possibleRoles = new List<string>
                {
                    ApplicationRole.MonitorUser,
                    ApplicationRole.WorkflowQueueAdmin,
                    ApplicationRole.WorkflowAdmin,
                    ApplicationRole.DataflowAdmin,
                    ApplicationRole.ManageDashboards,
                    ApplicationRole.SearchAdmin
                };

            foreach (var result in results)
            {
                Assert.IsTrue(possibleRoles.Contains(result));
            }
        }
    }
}
