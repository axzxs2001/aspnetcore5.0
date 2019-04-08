using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HttpClientDemo001.Repository;
using Microsoft.AspNetCore.Mvc;

namespace HttpClientDemo001.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TypeClientsTestController : ControllerBase
    {
        readonly ITypeClientRepository _typeClientRepository;

        public TypeClientsTestController(ITypeClientRepository typeClientRepository)
        {
            _typeClientRepository = typeClientRepository;
        }

        [HttpGet("tcget")]
        public async Task<ActionResult> TestGet()
        {
            return new JsonResult(await _typeClientRepository.GetEntities());
        }
    }
}
