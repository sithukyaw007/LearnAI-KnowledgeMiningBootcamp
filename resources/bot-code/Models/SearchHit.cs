using System;
using System.Collections.Generic;

namespace Models
{
    public class SearchHit
    {
        public SearchHit()
        {
            this.PropertyBag = new Dictionary<string, object>();
        }
        public string Title { get; set; }
        public string documentUrl { get; set; }
        public string Description { get; set; }


        public IDictionary<string, object> PropertyBag { get; set; }
    }
}