using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Legasy.Core.Model
{
    public class CaseClass : IEquatable<CaseClass>
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public DescriptionClass Decsription { get; set; }

        public List<FileClass> Files { get; set; }

        public CaseClass()
        {
            Name = "";
            Path = "";
            Decsription = new DescriptionClass();
            Files = new List<FileClass>();
        }

        public bool Equals(CaseClass ersr)
        {
            //Check whether the compared object is null.  
            if (Object.ReferenceEquals(ersr, null)) return false;

            //Check whether the compared object references the same data.  
            if (Object.ReferenceEquals(this, ersr)) return true;

            //Check whether the UserDetails' properties are equal.  
            return Name.Equals(ersr.Name);
        }

        // If Equals() returns true for a pair of objects   
        // then GetHashCode() must return the same value for these objects.  

        public override int GetHashCode()
        {

            //Get hash code for the UserName field if it is not null.  
            int hashN = Name == null ? 0 : Name.GetHashCode();

            //Calculate the hash code for the GPOPolicy.  
            return hashN;
        }
    }
}
