using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Dapper;

namespace DapperDemo01.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Values2Controller : ControllerBase
    {

        readonly IDapperPlusDB _dapperPlusDB;
        public Values2Controller(IDapperPlusDB  dapperPlusDB)
        {
            _dapperPlusDB = dapperPlusDB;        
        }


        [HttpGet]
        public ActionResult<IEnumerable<T1>> Get()
        {
            using (var db = _dapperPlusDB)
            {
                return db.Query<T1>("select * from t1").ToList();
            }
        }


        [HttpGet("{id}")]
        public ActionResult<T1> Get(int id)
        {
            using (var db = _dapperPlusDB)
            {
                return db.Query<T1>("select * from t1 where id=@id", new { id }).FirstOrDefault();
            }
        }


        [HttpPost]
        public void Post([FromBody] T1 t1)
        {
            using (var db = _dapperPlusDB)
            {
                db.Execute("insert into t1(name) values(@Name)", t1);
            }
        }


        [HttpPut("{id}")]
        public void Put(int id, [FromBody] T1 t1)
        {
            using (var db = _dapperPlusDB)
            {
                t1.ID = id;
                t1.ModifyTime = DateTime.UtcNow;
                db.Execute("update t1 set name=@Name,modifytime=@ModifyTime where id=@ID", t1);
            }
        }


        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            using (var db = _dapperPlusDB)
            {
                db.Execute("delete from t1 where id=@id", new { id });
            }
        }


    }

    public class T1
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime? ModifyTime { get; set; }
    }
}
