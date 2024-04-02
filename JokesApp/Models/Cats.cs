using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JokesApp.Models
{
        public class Categories
        {
            public bool Error { get; set; }
            public string[] categories { get; set; }
            public Categoryalias[] CategoryAliases { get; set; }
            public long Timestamp { get; set; }
        }

        public class Categoryalias
        {
            public string Alias { get; set; }
            public string Resolved { get; set; }
        }
}
