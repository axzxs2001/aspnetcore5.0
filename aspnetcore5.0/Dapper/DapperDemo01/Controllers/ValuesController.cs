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
    /// <summary>
    /// 原生Dapper方式
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {

        SqliteConnection _sqliteConnection;
        public ValuesController(IConfiguration configuration)
        {
            var connection = string.Format(configuration.GetConnectionString("Sqlite"), System.IO.Directory.GetCurrentDirectory());
            _sqliteConnection = new SqliteConnection(connection);
        }


        [HttpGet]
        public ActionResult<IEnumerable<T1>> Get()
        {
            using (var con = _sqliteConnection)
            {
                return con.Query<T1>("select * from t1").ToList();
            }
        }


        [HttpGet("{id}")]
        public ActionResult<T1> Get(int id)
        {
            using (var con = _sqliteConnection)
            {
                return con.Query<T1>("select * from t1 where id=@id", new { id }).FirstOrDefault();
            }
        }


        [HttpPost]
        public void Post([FromBody] T1 t1)
        {
            using (var con = _sqliteConnection)
            {
                con.Execute("insert into t1(name) values(@Name)", t1);
            }
        }


        [HttpPut("{id}")]
        public void Put(int id, [FromBody] T1 t1)
        {
            using (var con = _sqliteConnection)
            {
                t1.ID = id;
                t1.ModifyTime = DateTime.UtcNow;
                con.Execute("update t1 set name=@Name,modifytime=@ModifyTime where id=@ID", t1);
            }
        }


        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            using (var con = _sqliteConnection)
            {
                con.Execute("delete from t1 where id=@id", new { id });
            }
        }


    }


}
