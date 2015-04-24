using System;
using Omu.ValueInjecter;
using TED.Dashboard.Search.Models;

namespace TED.Dashboard.Api.Models
{
    public class SearchAuditCountDataModel
    {
        public int SearchesExecuted { get; set; }
        public DateTime DaySearchesExecuted { get; set; }

        public string DisplayExecuteDate 
        {
            get
            {
                return string.Format("{0}/{1}/{2}",
                                        DaySearchesExecuted.Month,
                                        DaySearchesExecuted.Day,
                                        DaySearchesExecuted.Year);
            }
        }

        public static SearchAuditCountDataModel BuildFromLogEntry(SearchAuditCount entry)
        {
            var entryVM = new SearchAuditCountDataModel();
            entryVM.InjectFrom(entry);

            return entryVM;
        }
    }
}