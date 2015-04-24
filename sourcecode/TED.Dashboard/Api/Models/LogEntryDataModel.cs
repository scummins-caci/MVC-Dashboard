using System;
using Omu.ValueInjecter;
using TED.Dashboard.Extensions;
using TED.Dashboard.Search.Models;

namespace TED.Dashboard.Api.Models
{
    public class LogEntryDataModel
    {
        public string LogId { get; set; }
        public string ClientMachine { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Location { get; set; }
        public string Severity { get; set; }
        public string Message { get; set; }

        /// <summary>
        /// Pretty date display
        /// </summary>
        public string DisplayTime 
        {
            get { return TimeStamp.ToPrettyDateString(); }
        }

        /// <summary>
        /// strip the namespace from the location where
        /// the log occurred
        /// </summary>
        public string DisplayLocation
        {
            get
            {
                var returnString = Location;
                if (returnString.StartsWith("HighView.Services."))
                {
                    returnString = returnString.Replace("HighView.Services.", "");
                }

                if (returnString.StartsWith("HighView.ServiceHost."))
                {
                    returnString = returnString.Replace("HighView.ServiceHost.", "");
                }

                if (returnString.StartsWith("HighView.WorkflowHost."))
                {
                    returnString = returnString.Replace("HighView.WorkflowHost.", "");
                }

                if (returnString.StartsWith("HighView.Workflow."))
                {
                    returnString = returnString.Replace("HighView.Workflow.", "");
                }

                // check to see if it ends with .Service
                returnString = returnString.Replace("..ctor()", "");
                returnString = returnString.Replace(".Service", "");
                returnString = returnString.Replace(".Run()", "");

                // remove assembly version and culture info
                if (returnString.Contains(","))
                {
                    returnString = returnString.Substring(0, returnString.IndexOf(",", StringComparison.Ordinal));
                }

                return returnString;
            }
        }

        public static LogEntryDataModel BuildFromLogEntry(LogEntry entry)
        {
            var entryVM = new LogEntryDataModel();
            entryVM.InjectFrom(entry);

            return entryVM;
        }
    }
}