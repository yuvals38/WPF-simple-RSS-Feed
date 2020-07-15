using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RssReaderAssignment.Model
{
    public class RssItem
    {
        public string UniqueId { get; set; }
        public string Title { get; set; }
        public string Link { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }

        public string Summary { get; set; }

        public string ParentUri { get; set; }
        public bool isFavorite { get; set; }
    }
}
