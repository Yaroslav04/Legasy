using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Legasy.Core.Model
{
    public class CaseClass
    {
        public string Name { get; set; }
        public string Path { get; set; }

        public DescriptionClass Decsription { get; set; }

        public List<FileClass> Files { get; set; }

        public CaseClass()
        {
            Files = new List<FileClass>();
        }
    }
}
