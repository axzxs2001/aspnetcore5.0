using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIDemo02.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize("Permission")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        /// <summary>
        /// 查询产品列表
        /// </summary>
        /// <returns>产品列表</returns>
        [HttpGet("/products")]
        public ActionResult Products()
        {
            return new JsonResult(new List<ProductModel> {
                new ProductModel { ID = 1, Name = "Name1", Price = 1.45m, Quantity = 12 },
                new ProductModel { ID = 2, Name = "Name2", Price = 5.45m, Quantity = 2 }
            });
        }
        /// <summary>
        /// 按照ID查询产品
        /// </summary>
        /// <param name="id">产品ID</param>
        /// <returns></returns>
        [HttpGet("/product/{id}")]
        public ActionResult Product(int id)
        {
            return new JsonResult(
                new ProductModel { ID = id, Name = "Name" + id, Price = 1.45m, Quantity = 12 }
             );
        }
        /// <summary>
        /// 添加产品
        /// </summary>
        /// <param name="product">产品</param>
        /// <returns>新增产品</returns>
        [HttpPost("/addproduct")]
        public ActionResult AddProduct(ProductModel product)
        {
            return new JsonResult(new ProductModel { ID = 1, Name = "Name1", Price = 1.45m, Quantity = 12 });
        }
        /// <summary>
        /// 修改产品
        /// </summary>
        /// <param name="product">产品</param>
        /// <returns>结果</returns>
        [HttpPut("/modifyproduct")]
        public ActionResult ModifyProduct(ProductModel product)
        {
            return Ok();
        }
        /// <summary>
        /// 删除产品
        /// </summary>
        /// <param name="id">产品ID</param>
        /// <returns>结果</returns>
        [HttpDelete("/removeproduct/{id}")]
        public ActionResult RemoveProduct(int id)
        {
            return Ok();
        }

    }
}
