using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UsuariosApi.Business.Interfaces;
using UsuariosApi.Models;

namespace UsuariosApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserBusiness userBusiness;

        public UserController(IUserBusiness userBusiness)
        {
            this.userBusiness = userBusiness;
        }

        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] User model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var insertResult = await userBusiness.AddUser(model);

            if (insertResult.Contains("Error"))
            {
                return BadRequest(new { Error = insertResult });
            }

            return Ok(insertResult);
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers(  [FromQuery] int pageSize, 
                                                    [FromQuery] int page) 
        {

            if (pageSize == 0)
                pageSize = 10;

            var users = await userBusiness.GetUsers(pageSize, page);

            if (users != null && users.Count() > 0)
            {
                return Ok(users);
            } else if (users != null && users.Count() == 0)
            {
                return NotFound();
            }

            return BadRequest();


        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(string id)
        {
            var user = await userBusiness.GetUserByUsername(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPost("getToken")]
        public async Task<IActionResult> GetToken([FromBody] UserAuth model)
        {

            var token = await userBusiness.AuthUser(model.Username, model.Password);

            if (token.Contains("Error"))
            {
                return BadRequest(new { Error = token });
            }


            return Ok(new { Token = token });
        }
    }
}
