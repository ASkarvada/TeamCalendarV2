using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamCalendar
{
    public class Relation<T> where T : Type
    {
        public Guid id;
        public static Relation<T> Create(T item)
        {
            Relation<T> rl = new Relation<T>();
            rl.id = item.id;
            return rl;
        }

        public T Get()
        {
            return StorageManager.GetStorage().findById<T>(id);
        }
    }
}
