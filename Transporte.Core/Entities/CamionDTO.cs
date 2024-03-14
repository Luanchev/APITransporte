using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transporte.Core.Entities
{
    public class CamionDTO
    {
        public int IdCamion  { get; set; }
        public string Placa { get; set; }
        public int Cilindraje { get; set; }
        public int CapMaxCarga { get; set; }
        public string Modelo { get; set; }
        public string Marca { get; set; }

    }
}
