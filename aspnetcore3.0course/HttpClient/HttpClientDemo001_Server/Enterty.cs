using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HttpClientDemo001_Server
{
    public class Enterty
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public DateTime CreateTime { get; set; } = DateTime.Now;

        public override string ToString()
        {
            return $"ID:{ID},Name:{Name},CreateTime:{CreateTime}";
        }
    }
}
