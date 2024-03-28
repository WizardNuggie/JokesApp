using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JokesApp.Models
{

    public class ServiceError
    {
        public bool error { get; set; }
        public bool internalError { get; set; }
        public int code { get; set; }
        public string message { get; set; }
        public string[] causedBy { get; set; }
        public string additionalInfo { get; set; }
        public long timestamp { get; set; }
    }

}
