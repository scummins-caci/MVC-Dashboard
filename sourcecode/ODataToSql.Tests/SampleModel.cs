using System;
using System.Collections.Generic;

namespace ODataToSql.Tests
{
    public class SampleModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public int Count { get; set; }
        public int Items { get; set; }
        public DateTime CurrentDate { get; set; }
        public bool IsReady { get; set; }
        public ulong ID { get; set; }

        // collection type
        public IList<SampleProperty> Properties { get; set; }
    }
}
