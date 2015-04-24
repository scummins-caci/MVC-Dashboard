using System.Collections.Generic;

namespace TED.Dashboard.Users.Services
{
    public interface IUserInfoService
    {
        /// <summary>
        /// Gets a list of all of the roles a user is in
        /// </summary>
        /// <param name="userId">user to get roles for</param>
        /// <returns></returns>
        IEnumerable<string> GetUserRoles(uint userId);

        /// <summary>
        /// Gets a list of user created dashboards 
        /// </summary>
        /// <param name="userId">user to find dashboards for</param>
        /// <returns></returns>
        IEnumerable<string> GetUserDashboards(uint userId);


    }
}
