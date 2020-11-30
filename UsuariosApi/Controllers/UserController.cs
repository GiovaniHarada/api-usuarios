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
            var guid = Guid.Parse(id);

            if (guid == null)
                return BadRequest();

            var user = await userBusiness.GetUserByGuid(guid);

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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var guid = Guid.Parse(id);

            if (guid == null)
                return BadRequest();


            var deleted = await userBusiness.DeleteUser(guid);

            if (deleted)
            {
                return Ok();
            }

            return NotFound();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] User model)
        {
            var guid = Guid.Parse(id);

            if (guid == null)
                return BadRequest();

            model.UserId = guid;


            var updated = await userBusiness.UpdateUser(model);

            if (updated)
            {
                return Ok();
            }

            return NotFound();
        }

        [HttpPost("{id}/requestPasswordToken")]
        public async Task<IActionResult> RequestPasswordToken(string id)
        {
            var guid = Guid.Parse(id);

            if (guid == null)
                return BadRequest();


            var tokenCreated = await userBusiness.ResetPasswordToken(guid);

            if (!tokenCreated)
                return BadRequest();

            return Ok();
        }

        [HttpPost("{id}/changePassword")]
        public async Task<IActionResult> ChangePassword(string id, [FromBody] ChangePassword model)
        {
            var guid = Guid.Parse(id);

            if (guid == null)
                return BadRequest();

            model.UserId = guid;


            var passwordChanged = await userBusiness.ChangePassword(model);

            if (!passwordChanged)
                return BadRequest();

            return Ok();
        }

    }
}
