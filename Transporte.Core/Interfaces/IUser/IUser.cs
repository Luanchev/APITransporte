using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transporte.Core.Entities;

namespace Transporte.Core.Interfaces.IUser
{
    public interface IUser
    {
        Task<ResponseService> ValidarUsuario(string correo, string password);
    }
}
