using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Key2Joy.Contracts.Mapping
{
    public abstract class AbstractMappingAspect : MarshalByRefObject, IComparable<AbstractMappingAspect>
    {
        public string Name { get; set; }

        public AbstractMappingAspect(string name)
        {
            Name = name;
        }

        public virtual int CompareTo(AbstractMappingAspect other)
        {
            return ToString().CompareTo(other.ToString());
        }

        public static bool operator ==(AbstractMappingAspect a, AbstractMappingAspect b)
        {
            if (ReferenceEquals(a, b))
                return true;

            if (ReferenceEquals(a, null)
                || ReferenceEquals(b, null))
                return false;

            return a.Equals(b);
        }
        public static bool operator !=(AbstractMappingAspect a, AbstractMappingAspect b) => !(a == b);
    }
}
