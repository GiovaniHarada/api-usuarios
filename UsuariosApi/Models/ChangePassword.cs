using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UsuariosApi.Models
{
    public class ChangePassword
    {
        public string Token { get; set; }
        public string NewPassword { get; set; }
        public Guid UserId { get; set; }
    }
}
