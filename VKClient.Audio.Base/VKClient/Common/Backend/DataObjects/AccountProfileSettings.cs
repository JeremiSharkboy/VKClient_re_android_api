using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VKClient.Common.Backend.DataObjects
{
    public class AccountProfileSettings
    {
        public bool api_requests { get; set; }
        public List<DownloadPatterns> download_patterns { get; set; }
    }
    
    public class DownloadPatterns
    {
        public string type { get; set; }
        public string pattern { get; set; }
        public float probability { get; set; }
        public float error_probability { get; set; }
    }
}
