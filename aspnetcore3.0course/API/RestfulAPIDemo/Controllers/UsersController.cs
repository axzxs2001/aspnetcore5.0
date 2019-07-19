using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestfulAPIDemo.Model;

namespace RestfulAPIDemo.Controllers
{

    /// <summary>
    /// 用户Controller
    /// </summary>  
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        readonly ILogger<UsersController> _logger;
        /// <summary>
        /// 用户仓储
        /// </summary>
        readonly IUserRepository _userRepository;
        /// <summary>
        /// url helper
        /// </summary>
        readonly IUrlHelper _urlHelper;

        /// <summary>
        /// 用户Controller
        /// </summary>
        /// <param name="logger">日志</param>
        /// <param name="userRepository">用户仓储</param>  
        /// <param name="urlHelper">url helper</param>
        public UsersController(ILogger<UsersController> logger, IUserRepository userRepository, IUrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
            _userRepository = userRepository;
            _logger = logger;
        }

        /// <summary>
        /// 异常发生
        /// </summary>
        /// <returns></returns>
        [HttpGet("/exception")]
        public IActionResult GetException()
        {
            throw new Exception("发生在User中的一个异常！");
        }

        /// <summary>
        /// 获取用户
        /// 资源应该使用名词, 它是个东西, 不是动作.
        /// api/getusers 就是不正确的.
        /// GET api/users 就是正确的
        /// GET api/users/{userId}
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(User), 200)]
        [HttpGet("{id}")]
        [HttpHead("{id}")]
        [HttpPost("{id}")]
        public ActionResult HandlerUser(int id)
        {
            var user = _userRepository.GetUserByID(id);
            if (user == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(user);
            }

        }
        /// <summary>
        /// 处理用户，返回可以操作的谓词
        /// </summary>
        /// <returns></returns>
        [HttpOptions("{id}")]
        [ProducesResponseType(typeof(User), 200)]
        public IActionResult HandlerUser()
        {
            Response.Headers.Add("Allow", "GET,POST,HEAD,OPTIONS");
            return Ok();
        }
        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="user">用户实体</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(User), 200)]
        [HttpPost]
        public ActionResult AddUser([FromBody]User user)
        {
            if (ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }

            var backUser = _userRepository.AddUser(user);
            if (backUser == null)
            {
                return NotFound();
            }
            else
            {
                return CreatedAtAction("GetUser", new { id = backUser.ID }, backUser);
            }
        }

        /// <summary>
        /// 如果POST到单个资源的地址 测试发生冲突
        /// </summary>
        /// <param name="userid">用户ID=1时存在</param>
        /// <returns></returns>
        [HttpPost("careateuser/{userid}")]
        public IActionResult CreateUser(int userid)
        {
            if (userid != 1)
            {
                return NotFound();
            }
            else
            {
                return StatusCode(StatusCodes.Status409Conflict);
            }
        }
        /// <summary>
        /// 局部修改用户
        /// {
        ///   "Operations":
        ///   [
        ///     {
        ///       "value": "222222",
        ///       "path": "password",
        ///       "op": "add"
        ///     },
        ///     {   
        ///       "path": "username",
        ///       "op": "remove"
        ///     }
        ///   ]
        /// }
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <param name="jsonPatchDocument">用户</param>
        /// <returns></returns>   
        [HttpPatch("{id}")]
        public IActionResult ModifyUser(int id, [FromBody]JsonPatchDocument<User> jsonPatchDocument)
        {
            var pros = new Dictionary<string, dynamic>();
            foreach (var operation in jsonPatchDocument.Operations)
            {
                switch (operation.OperationType)
                {
                    case OperationType.Add:
                    case OperationType.Replace:
                        pros.Add(operation.path, operation.value);
                        break;
                    case OperationType.Remove:
                        pros.Add(operation.path, "");
                        break;
                }
            }
            var sql = $"update table1 set {string.Join(',', pros.Select(s => s.Key + "='" + s.Value + "'"))} where id={id}";
            return Content(sql);
        }

        /// <summary>
        /// 修改帐户
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <param name="user">用户</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(int), 200)]
        [HttpPut("{id}")]
        public IActionResult UpdateAccount(int id, [FromBody]User user)
        {
            if (id == 1)
            {
                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="userPagination">分页信息</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetUsers([FromQuery]UserPagination userPagination)
        {
            var pageUsers = _userRepository.GetPagingUser(userPagination);
            if (pageUsers == null || pageUsers.Count == 0)
            {
                return NotFound();
            }
            else
            {
                var previousLink = pageUsers.HasPrevious ? CreateUserUri(userPagination, PaginationResourceUriType.PreviousPage) : null;
                var nextLink = pageUsers.HasNext ? CreateUserUri(userPagination, PaginationResourceUriType.NextPage) : null;

                var meta = new
                {
                    pageUsers.TotalItemCount,
                    pageUsers.PaginationBase.PageIndex,
                    pageUsers.PaginationBase.PageSize,
                    pageUsers.PageCount,
                    PreviousPageLink = previousLink,
                    NextPageLink = nextLink
                };
                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(meta));
                return Ok(pageUsers);
            }

        }
        /// <summary>
        /// 创建向前向后url
        /// </summary>
        /// <param name="userPagination">分页实体</param>
        /// <param name="paginationResourceUriType">分页url类型</param>
        /// <returns></returns>
        string CreateUserUri(PaginationBase userPagination, PaginationResourceUriType paginationResourceUriType)
        {

            switch (paginationResourceUriType)
            {
                case PaginationResourceUriType.PreviousPage:
                    var previousParmeters = userPagination.Clone();
                    previousParmeters.PageIndex--;
                    var res = _urlHelper.RouteUrl(previousParmeters);
                    return res;
                case PaginationResourceUriType.NextPage:
                    var nextParmeters = userPagination.Clone();
                    nextParmeters.PageIndex++;
                    return _urlHelper.RouteUrl(nextParmeters);
                default:
                    return _urlHelper.RouteUrl(userPagination);
            }
        }
    }

}
