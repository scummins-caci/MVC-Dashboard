using System;
using System.Collections.Generic;
using Omu.ValueInjecter;
using TED.Dashboard.Extensions;
using TED.Dashboard.Search.Models;

namespace TED.Dashboard.Api.Models
{
    public class SearchAuditDataModel
    {
        public string UserName { get; set; }
        public DateTime DateExecuted { get; set; }
        public IList<SearchCriteria> Criteria { get; set; }

        /// <summary>
        /// Pretty date display
        /// </summary>
        public string DisplayTime
        {
            get { return DateExecuted.ToPrettyDateString(); }
        }

        /// <summary>
        /// 
        /// </summary>
        public string DisplayDate
        {
            get { return string.Format("{0} {1}", DateExecuted.ToShortDateString(), DateExecuted.ToShortTimeString()); }
        }

        public static SearchAuditDataModel BuildFromSearchAudit(SearchAudit entry)
        {
            var audit = new SearchAuditDataModel();
            audit.InjectFrom(entry);

            return audit;
        }
    }
}