using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transporte.Core.Entities;

namespace Transporte.Core.Interfaces.ICamion
{
    public interface ICamion
    {
        //estoy definiendo una interfaz
        Task<ResponseService> GetAllCamiones();
        Task<ResponseService> GetCamionByPlaca(string placa);
        Task<ResponseService> CreateCamion(CamionDTO camion);
        Task<ResponseService> EditCamion(CamionDTO camion);
        Task<ResponseService> DeleteCamion(int IdCamion);
    }
}
