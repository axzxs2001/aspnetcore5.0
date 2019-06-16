using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MuiltDIDemo
{
    public class DBService : IDBService
    {
        readonly string _connectionstring;
        public DBType DBType { get; }

        public string ConnectionString
        {
            get
            {
                return _connectionstring;
            }
        }

        public DBService(string connectionstring, DBType dBType)
        {
            DBType = dBType;
            _connectionstring = connectionstring;
        }
    }
}
