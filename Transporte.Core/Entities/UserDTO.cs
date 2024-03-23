using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transporte.Core.Entities
{
    public class UserDTO
    {
        public int IdUsuario { get; set; }
        public string correo { get; set; }
        public string password { get; set; }
        public string rol { get; set; }

    }
}
