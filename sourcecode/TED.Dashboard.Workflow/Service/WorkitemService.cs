using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using TED.Dashboard.Repository;
using TED.Dashboard.Services.Models;
using TED.Dashboard.Workflow.Models;
using TED.Dashboard.Workflow.Repositories;

namespace TED.Dashboard.Workflow.Services
{
    public class WorkitemService : IWorkitemService
    {
        private readonly IReadOnlyRepository<WorkItemCount> workItemCountRepository;
        private readonly IReadOnlyRepository<WorkItemStatusCount> statusCountRepository;
        private readonly IProcessInfoRepository processRepository;
        private readonly IQueueRepository queueRepository;
        private readonly IWorkItemRepository workitemRepository;
        private readonly IQueueUserRepository userRepository;

        public WorkitemService(IReadOnlyRepository<WorkItemCount> workItemCountRepository,
                                IReadOnlyRepository<WorkItemStatusCount> statusCountRepository,
                                IProcessInfoRepository processRepository,
                                IQueueRepository queueRepository,
                                IWorkItemRepository workitemRepository,
                                IQueueUserRepository userRepository
            )
        {
            this.workItemCountRepository = workItemCountRepository;
            this.statusCountRepository = statusCountRepository;
            this.processRepository = processRepository;
            this.queueRepository = queueRepository;
            this.workitemRepository = workitemRepository;
            this.userRepository = userRepository;
        } 

        /// <summary>
        /// List of activites that we don't want to display
        /// service host to run
        /// </summary>
        private readonly IEnumerable<string> blacklist = new List<string> 
        {
            // none for now
        };


        public IEnumerable<WorkItemStatusCount> GetStatusCounts()
        {
            return statusCountRepository.GetAll();
        }

        public IEnumerable<WorkItemCount> GetQueueCounts()
        {
            var counts = workItemCountRepository.GetAll();
            return counts.Where(s => !blacklist.Contains(s.Activity));
        }

        public IEnumerable<WorkItemCount> GetManualQueueCounts()
        {
            var counts = workItemCountRepository.GetAll();
            
            // filter down connectors based on blacklist amd where Type is Manual
            return counts.Where(s => !blacklist.Contains(s.Activity) && s.Type == "Manual");
            // filter
        }

        public IEnumerable<WorkItemCount> GetAutomatedQueueCounts()
        {
            var counts = workItemCountRepository.GetAll();

            // filter down connectors based on blacklist amd where Type is Manual
            return counts.Where(s => !blacklist.Contains(s.Activity) && s.Type == "Automated");
            // filter
        }

        public IEnumerable<QueueUser> GetQueueUsers(ulong queueId)
        {
            return userRepository.GetQueueUsers(queueId);
        }

        public PageableItem<WorkflowProcessInfo> GetProcesses(int page = 1, int count = 25, 
                                                                Expression<Func<WorkflowProcessInfo, bool>> filter = null, 
                                                                Expression orderBy = null,
                                                                bool includeAdminInfo = false)
        {
            var processes = processRepository.GetPage(page, count, filter, orderBy).ToArray();
            return new PageableItem<WorkflowProcessInfo>
                {
                    ItemCount = processes.Any() ? processes.First().ProcessCount : 0,
                    Items = processes
                };
        }

        public PageableItem<WorkflowQueue> GetQueues(ulong processId, int page = 1, int count = 25,
                                                                Expression<Func<WorkflowQueue, bool>> filter = null, 
                                                                Expression orderBy = null)
        {
            var queues = queueRepository.GetPage(processId, page, count, filter, orderBy).ToArray();
            return new PageableItem<WorkflowQueue>
                {
                    ItemCount = queues.Any() ? queues.First().QueueCount : 0,
                    Items = queues
                };
        }

        public PageableItem<WorkItem> GetWorkItems(ulong queueId, ulong processId = 0, int page = 1, int count = 25,
                                                                Expression<Func<WorkItem, bool>> filter = null, 
                                                                Expression orderBy = null)
        {
            var workitems = workitemRepository.GetPage(queueId, processId, page, count, filter, orderBy).ToArray();
            return new PageableItem<WorkItem>
                {
                    ItemCount = workitems.Any() ? workitems.First().WorkitemCount : 0,
                    Items = workitems
                };
        }
    }
}
