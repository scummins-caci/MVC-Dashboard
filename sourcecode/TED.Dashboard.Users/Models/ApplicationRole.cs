namespace TED.Dashboard.Users
{
    /// <summary>
    /// Class that defines all of the authorization roles the user may be a part of
    /// </summary>
    public static class ApplicationRole
    {
        /// <summary>
        /// The basic right that the user needs in order to access the TED Monitor/Dashboard application
        /// </summary>
        public const string MonitorUser = "MonitorUser";

        /// <summary>
        /// Allows the user to create and publish custom dashboards
        /// </summary>
        public const string ManageDashboards = "ManageDashboards";

        /// <summary>
        /// The role associated with viewing the state of the workflow and making changes
        /// </summary>
        public const string WorkflowAdmin = "WorkflowAdmin";

        /// <summary>
        /// The role associated with using the inbasket
        /// </summary>
        public const string WorkflowUser = "WorkflowUser";

        /// <summary>
        /// The role associated with viewing workflow items in there queues and potentially making changes
        /// </summary>
        public const string WorkflowQueueAdmin = "WorkflowQueueAdmin";

        /// <summary>
        /// The role associated with viewing dataflow info and potentially making changes
        /// </summary>
        public const string DataflowAdmin = "DataflowAdmin";

        /// <summary>
        /// The role associated with viewing search audit information
        /// </summary>
        public const string SearchAdmin = "SearchAdmin";
    }
}
