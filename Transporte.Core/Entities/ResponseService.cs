using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transporte.Core.Entities
{
    public class ResponseService
    {
        //esta clase va a ser el modelo de respuesta en los repositorios o servicios                                                             
        public Object Data { get; set; } // va a contener todos los objetos de los consumos de base de datos
        public int Status { get; set; }
        public string Message { get; set; }
    }
}
