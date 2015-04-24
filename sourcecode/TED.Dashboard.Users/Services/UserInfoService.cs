using System.Collections.Generic;
using System.Linq;
using TED.Dashboard.Repository;


namespace TED.Dashboard.Users.Services
{
    public class UserInfoService : IUserInfoService
    {
        private readonly IIDFilterRepository<Role> roleRepository;
        private readonly Dictionary<string, string[]> roleMappings; 


        public UserInfoService(IIDFilterRepository<Role> roleRepository)
        {
            this.roleRepository = roleRepository;

            // build mapping of hv5 to system roles
            roleMappings = new Dictionary<string, string[]>
                {
                    //{"IsSysAdmin", new []{ApplicationRole.MonitorUser}},
                    //{"IsWfQueueAdmin",new []{ ApplicationRole.WorkflowQueueAdmin, ApplicationRole.WorkflowAdmin}}
                    
                    // TODO: for now to make life simple just map everything to sysadmin;  need to map the auth roles
                    // TODO: to true hv5 permissions and create some new ones
                    {"IsSysAdmin", new []{ApplicationRole.MonitorUser, ApplicationRole.WorkflowQueueAdmin,
                                            ApplicationRole.WorkflowAdmin, ApplicationRole.DataflowAdmin,
                                            ApplicationRole.ManageDashboards, ApplicationRole.SearchAdmin, 
                                            ApplicationRole.WorkflowUser}}
                };
        }

        /// <summary>
        /// Takes the values retrieved by the repository and maps them to dashboard system roles
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IEnumerable<string> GetUserRoles(uint userId)
        {
            var returnRoles = new List<string>();
            
            // retrieve role names
            var roles = roleRepository.GetAll(userId);
            
            // map roles from hv5 to monitor/dash roles
            foreach (var role in roles.Where(x => x.Value)
                            .Where(role => roleMappings.ContainsKey(role.RoleName)))
            {
                returnRoles.AddRange(roleMappings[role.RoleName]);
            }

            return returnRoles;
        }

        public IEnumerable<string> GetUserDashboards(uint userId)
        {
            return new List<string>();
        }
    }
}
