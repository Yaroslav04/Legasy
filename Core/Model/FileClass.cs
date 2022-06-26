using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Legasy.Core.Model
{
    public class FileClass
    {
        public string Type { get; set; }
        public string Path { get; set; }
        public string Name { get; set; }
        public int Size { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
