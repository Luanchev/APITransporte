using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Transporte.Core.Entities;
using Transporte.Core.Interfaces.ICamion;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Transporte.Infrastructure.Repositories.CamionRepositories
{
    //en esta clase va a venir todo el repositorio de camion, va a tener todos los servicios de administrar de la tabla de camion
    public class CamionRepository : ICamion
    {
        //instanciamos la conexion hacia la base de datos
        public Connection connection;
        private readonly IConfiguration configuration; //instanciamos la configuracion predeterminada en context
        string connString = "Host=localhost;Port=2508;Username=postgres;Password=250819;Database=InfoEmpresaTransporte;"; //cadena de conexion a la base de datos(credenciales)
        ResponseService r = new ResponseService();

        //Constructor 
        public CamionRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
            connection = new Connection();
        }


        #region Servicio Obtener todos los camiones
        public async Task<ResponseService> GetAllCamiones() //aqui declaro el metodo que trae todos los registros de la tabla camion
         {
            using (NpgsqlConnection conn = new NpgsqlConnection(connString)) //instanciar el objeto de la clase que hace la conexion con la bd
            {
                try
                {
                    await conn.OpenAsync(); //una espera para abrir la conexion de la base de datos siempre se debe escribir

                    string sentence = "SELECT * FROM public.camion " +
                                      "ORDER BY idcamion;"; // cada vez que voy a hacer una consulta a la DB debo de crear una variable que recibe comandos SQL , este SQL puede seleccionar 
                    List<CamionDTO> ListCamiones = new List<CamionDTO>(); // estamos creando la lista de camion para traer todos los registros, es decir el objeto donde guarda toda la información traida de la base de datos
                    CamionDTO Camion = null;

                    var cmd = new NpgsqlCommand(sentence, conn); // Crear objeto que ejecuta la sentencia en la base datos recibiendo como paraetro la sentencia y la conexion de la base de datos
                    cmd.CommandType = CommandType.Text;
                    var reader = await cmd.ExecuteReaderAsync();

                    while (await reader.ReadAsync()) //mientras reader tenga registos por retornar va a ejecutarse
                    {

                        Camion = new CamionDTO();

                        if (reader["idcamion"] != DBNull.Value) // si idCamion no es nulo entra en la funcion
                        {
                            Camion.IdCamion = Convert.ToInt32(reader["idcamion"]);//lo que hacemos aca es asignarle al idCamion el valor encontrado
                        }

                        if (reader["placa"] != DBNull.Value)
                        {
                            Camion.Placa = Convert.ToString(reader["placa"]);
                        }

                        if (reader["cilindraje"] != DBNull.Value)
                        {
                            Camion.Cilindraje = Convert.ToInt32(reader["cilindraje"]);
                        }

                        if (reader["capmaxcarga"] != DBNull.Value)
                        {
                            Camion.CapMaxCarga = Convert.ToInt32(reader["capmaxcarga"]);
                        }

                        if (reader["modelo"] != DBNull.Value)
                        {
                            Camion.Modelo = Convert.ToString(reader["modelo"]);
                        }


                        if (reader["marca"] != DBNull.Value)
                        {
                            Camion.Marca = Convert.ToString(reader["marca"]);
                        }

                      

                        ListCamiones.Add(Camion);
                    }

                    await reader.CloseAsync(); //cerrar la lectura de la ejecucion

                    // validaciones 

                    if(ListCamiones.Count == 0) //si no hay registros en la lista camiones
                    {
                         //se ejecuto de manera correcta, porque se realizo la consulta pero no hay datos
                        r.Message = "No hay registros";
                    }else
                    {
                        r.Data = ListCamiones;
                        r.Message = "Se ejecuto de manera correcta";
                    }

                    //se ejecuto de manera correcta
                    r.Status = 200; 
                    

                    return r;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);

                }
            }
        }
        #endregion


        #region Servicio Obtener camion por placa
        public async Task<ResponseService> GetCamionByPlaca(string placa) //aqui declaro el metodo que traer un registro por la placa
        {
            
            using (NpgsqlConnection conn = new NpgsqlConnection(connString))
            {
                try
                {
                    
                    await conn.OpenAsync(); //una espera para abrir la conexion de la base de datos siempre se debe escribir

                    string sentence = "Select * " +
                        "FROM camion " +
                        $"where placa = '{placa}';"; //lo que hacemos aqui es la sentencia que selecciona el registro de un camion por placa

                    var cmd = new NpgsqlCommand(sentence, conn);
                    cmd.CommandType = CommandType.Text;
                    var reader = await cmd.ExecuteReaderAsync();
                    CamionDTO Camion = null;

                    while (await reader.ReadAsync())
                    {

                        Camion = new CamionDTO();

                        if (reader["idcamion"] != DBNull.Value)
                        {
                            Camion.IdCamion = Convert.ToInt32(reader["idcamion"]);
                        }

                        if (reader["placa"] != DBNull.Value)
                        {
                            Camion.Placa = Convert.ToString(reader["placa"]);
                        }

                        if (reader["cilindraje"] != DBNull.Value)
                        {
                            Camion.Cilindraje = Convert.ToInt32(reader["cilindraje"]);
                        }

                        if (reader["capmaxcarga"] != DBNull.Value)
                        {
                            Camion.CapMaxCarga = Convert.ToInt32(reader["capmaxcarga"]);
                        }

                        if (reader["modelo"] != DBNull.Value)
                        {
                            Camion.Modelo = Convert.ToString(reader["modelo"]);
                        }

                        if (reader["marca"] != DBNull.Value)
                        {
                            Camion.Marca = Convert.ToString(reader["marca"]);
                        }  
                                         
                       
                    }

                    await reader.CloseAsync();

                    //Validación retorno de servicios
                    if ( Camion == null)
                    {
                        r.Status = 400;
                        r.Message = "No se encontro ningun dato con la placa seleccionada";
                    }
                    else
                    {
                        r.Data= Camion;
                        r.Status = 200;
                        r.Message = "Se ejecuto correctamente";
                    }


                    return r;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);

                }
            }
        }
        #endregion


        #region Servicio agregar nuevos registros
        public async Task<ResponseService> CreateCamion(CamionDTO camion) //aqui declaro el metodo que agrega un registro
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(connString))
            {
                try
                {
                    await conn.OpenAsync(); //una espera para abrir la conexion de la base de datos siempre se debe escribir 

                    if (await GetIdCamionByPlaca(camion.Placa, conn) > 0)
                    {
                        r.Status = 400;
                        r.Message = "No se puede adicionar un camion con una placa ya registrada";
                        return r;
                    }

                    string sentence = "INSERT INTO camion (placa, cilindraje, capMaxCarga, modelo, marca) " +                  
                        "VALUES (" +                 
                        $"'{camion.Placa}', '{camion.Cilindraje}', '{camion.CapMaxCarga}', '{camion.Modelo}', '{camion.Marca}');"; //lo que hacemos aqui es la sentencia SQL
                    
                    var cmd = new NpgsqlCommand(sentence, conn);
                    cmd.CommandType = CommandType.Text;
                    await cmd.ExecuteNonQueryAsync();

                    
                        r.Status = 200;
                        r.Message = "Se ha agregado el registro de manera exitosa";
                    
                    return r;
                }                  
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }
        #endregion


        #region Servicio para actualizar un registro existente
        public async Task<ResponseService> EditCamion(CamionDTO camion) //aqui declaro el metodo que agrega un registro
        {

            using (NpgsqlConnection conn = new NpgsqlConnection(connString))
            {
                try
                {
                    await conn.OpenAsync(); //una espera para abrir la conexion de la base de datos siempre se debe escribir 

                    //con esto validamos si idCamion existe en la tabla sino retorna 0 y arroja el error
                    if (await ValidateIdCamion(camion.IdCamion, conn) == 0)
                    {
                        r.Status = 400;
                        r.Message = "No se puede editar el camion ya que no se encuentra en la base de datos";
                        return r;
                    }
                    string sentence = "UPDATE camion " +
                              "SET placa = '" + camion.Placa + "', " +
                              "cilindraje = '" + camion.Cilindraje + "', " +
                              "capMaxCarga = '" + camion.CapMaxCarga + "', " +
                              "modelo = '" + camion.Modelo + "', " +
                              "marca = '" + camion.Marca + "' " +
                              "WHERE IdCamion = " + camion.IdCamion;                 
                    

                    var cmd = new NpgsqlCommand(sentence, conn);
                    cmd.CommandType = CommandType.Text;
                    await cmd.ExecuteNonQueryAsync();
                 
                    r.Status = 200;
                    r.Message = "Se ha editado el registro de manera exitosa";
                    return r;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }
        #endregion


        #region Servicio para borrar registros existentes
        public async Task<ResponseService> DeleteCamion(int IdCamion) //aqui declaro el metodo que agrega un registro
        {

            using (NpgsqlConnection conn = new NpgsqlConnection(connString))
            {
                try
                {
                    await conn.OpenAsync(); //una espera para abrir la conexion de la base de datos siempre se debe escribir 

                    if (await ValidateIdCamion(IdCamion, conn) == 0)
                    {
                        r.Status = 400;
                        r.Message = "No se puede eliminar el camion ya que no se encuentra el id en la base de datos";
                        return r;
                    }
                    string sentence = "DELETE FROM camion WHERE IdCamion = " + $"'{IdCamion}';";


                    var cmd = new NpgsqlCommand(sentence, conn);
                    cmd.CommandType = CommandType.Text;
                    await cmd.ExecuteNonQueryAsync();

                    r.Status = 200;
                    r.Message = "Se ha eliminado el registro de manera exitosa";

                    return r;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }
        #endregion


        #region Servicio Obtener IdCamion por placa
        public async Task<int> GetIdCamionByPlaca(string placa, NpgsqlConnection conn) //aqui declaro el metodo que traer un registro por la placa
        {
            try
            {

                string sentence = "Select IdCamion " +
                    "FROM camion " +
                    $"where placa = '{placa}';"; //lo que hacemos aqui es la sentencia que selecciona el registro de un camion por placa

                var cmd = new NpgsqlCommand(sentence, conn);
                cmd.CommandType = CommandType.Text;
                var reader = await cmd.ExecuteReaderAsync();
                int idCamionDb = 0;

                while (await reader.ReadAsync())
                {
                    if (reader["IdCamion"] != DBNull.Value)
                    {
                        idCamionDb = Convert.ToInt32(reader["IdCamion"]);
                    }

                }

                await reader.CloseAsync();
                
                return idCamionDb;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }

        }
        #endregion


        #region Servicio para validar el IdCamion
        public async Task<int> ValidateIdCamion(int IdCamion, NpgsqlConnection conn) 
        {
            try
            {
                string sentence = "Select idcamion " +
                     "FROM camion " +
                     $"where idcamion = '{IdCamion}';";

                var cmd = new NpgsqlCommand(sentence, conn);
                cmd.CommandType = CommandType.Text;
                var reader = await cmd.ExecuteReaderAsync();
                int idCamionDb = 0; //este Id es si no encuentra el valor entoncer retorna 0

                while (await reader.ReadAsync())
                {
                    if (reader["IdCamion"] != DBNull.Value)
                    {
                        idCamionDb = Convert.ToInt32(reader["IdCamion"]);
                    }
                }

                await reader.CloseAsync();

                return idCamionDb;               
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }
        }
        #endregion



    }
}
