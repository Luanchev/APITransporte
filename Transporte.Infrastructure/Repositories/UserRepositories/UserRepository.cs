using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Data;
using System.Linq;
using System.Numerics;
using System.Security.Claims;
using System.Text;
using Transporte.Core.Entities;
using Transporte.Core.Interfaces.IUser;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Transporte.Core.Interfaces.ICamion;

namespace Transporte.Infrastructure.Repositories.UserRepositories
{
    public class UserRepository : IUser
    {

        public Connection connection;
        private readonly string key;
        private readonly IConfiguration configuration; //instanciamos la configuracion predeterminada en context
        string connString = "Host=localhost;Port=2508;Username=postgres;Password=250819;Database=InfoEmpresaTransporte;"; //cadena de conexion a la base de datos(credenciales)

        ResponseService r = new ResponseService();
        private ResponseService StatusCode(int status200OK, object value)
        {
            throw new NotImplementedException();
        }

        //Constructor 
        public UserRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
            connection = new Connection();
            key = configuration.GetSection("Jwt").GetSection("Key").ToString();
        }

        #region servicio validar autenticacion
        public async Task<ResponseService> ValidarUsuario(string correo, string password) //aqui declaro el metodo que agrega un registro
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(connString))
            {
                try
                {
                    await conn.OpenAsync(); //una espera para abrir la conexion de la base de datos siempre se debe escribir

                    string sentence = "Select usuario, password FROM usuario " +
                                      $"where usuario = '{correo}';"; //lo que hacemos aqui es la sentencia que selecciona el usuario y la contraseña

                    var cmd = new NpgsqlCommand(sentence, conn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("usuario", correo);
                    var reader = await cmd.ExecuteReaderAsync();
                    UserDTO usuario = null;


                    while (await reader.ReadAsync())
                    {
                        usuario = new UserDTO();

                        if (reader["usuario"] != DBNull.Value && reader["password"] != DBNull.Value)
                        {
                            usuario.correo = Convert.ToString(reader["usuario"]);

                            usuario.password = reader["password"].ToString();

                            if (password == usuario.password)
                            {
                                var keyBytes = Encoding.ASCII.GetBytes(key);
                                var claims = new ClaimsIdentity(); // son las solicitudes

                                claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, usuario.correo));

                                var tokenDescriptor = new SecurityTokenDescriptor
                                {
                                    Subject = claims,
                                    Expires = DateTime.UtcNow.AddMinutes(5), // esto es para expirar el token despues de pasar 5 minutos
                                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature) // algoritmo criptografico
                                };

                                var tokenHandler = new JwtSecurityTokenHandler();
                                var tokenConfig = tokenHandler.CreateToken(tokenDescriptor);

                                string tokenCreado = tokenHandler.WriteToken(tokenConfig);
                                r.Status = StatusCodes.Status200OK;
                                r.Data = new { token = tokenCreado };       
                            }
                            else
                            {
                                r.Status = StatusCodes.Status401Unauthorized;
                                r.Message = "Credenciales invalidas";
                            }
                        }
                        else
                        {
                            r.Status = StatusCodes.Status404NotFound;
                            r.Message = "Usuario no encontrado";
                        }
                    }                    
                }
                catch (Exception ex)
                {
                    r.Status = StatusCodes.Status500InternalServerError;
                    r.Message = ex.Message;
                }
                return r;
            }
        }
        #endregion
    }
}
