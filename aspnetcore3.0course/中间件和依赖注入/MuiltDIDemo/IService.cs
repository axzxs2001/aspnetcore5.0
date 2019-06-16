using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MuiltDIDemo
{
    public interface IDBService
    {
        public DBType DBType { get;}

        string ConnectionString { get; }
    }
}
