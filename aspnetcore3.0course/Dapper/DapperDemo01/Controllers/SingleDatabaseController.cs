using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DapperExtension;

namespace DapperDemo01.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class SingleDatabaseController : ControllerBase
    {
        readonly IDapperPlusDB _sqliteDB;
      
        public SingleDatabaseController(IDapperPlusDB dapperPlusDB)
        {
            _sqliteDB = dapperPlusDB;
        }

        [HttpGet]
        public ActionResult<IEnumerable<T1>> Get()
        {
            using (_sqliteDB)
            {
                return _sqliteDB.Query<T1>("select * from t1").ToList();
            }
        }


        [HttpGet("{id}")]
        public ActionResult<T1> Get(int id)
        {
            using (_sqliteDB)
            {
                return _sqliteDB.Query<T1>("select * from t1 where id=@id", new { id }).FirstOrDefault();
            }
        }


        [HttpPost]
        public void Post([FromBody] T1 t1)
        {
            using (_sqliteDB)
            {
                _sqliteDB.Execute("insert into t1(name) values(@Name)", t1);
            }
        }


        [HttpPut("{id}")]
        public void Put(int id, [FromBody] T1 t1)
        {
            using (_sqliteDB)
            {
                t1.ID = id;
                t1.ModifyTime = DateTime.UtcNow;
                _sqliteDB.Execute("update t1 set name=@Name,modifytime=@ModifyTime where id=@ID", t1);
            }
        }


        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            using (_sqliteDB)
            {
                _sqliteDB.Execute("delete from t1 where id=@id", new { id });
            }
        }
    }
}
