using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamCalendar
{
    public abstract class Type
    {
        public Guid id;

        public Type()
        {
            id = Guid.NewGuid();
        }
    }
}
