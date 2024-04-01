using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Transporte.Core.Entities;
using Npgsql;
using System.Linq.Expressions;

namespace Tranporte.Formulario
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        public class Respuesta
        {
            public string token { get; set; }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            //este ejecuta el metodo validar
            using (NpgsqlConnection conn = new NpgsqlConnection(connString))
            {
                try
                {
                    await conn.OpenAsync();
                    var cliente1 = new HttpClient();
                    //UserDTO obUsuario = null;
                    string sentence = "Select usuario, password FROM usuario " +
                                               $"where usuario = '{correo}';";
                    var cmd = new NpgsqlCommand(sentence, conn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("usuario", correo);
                    var reader = await cmd.ExecuteReaderAsync();
                    UserDTO obUsuario = null;

                    while (await reader.ReadAsync())
                    {
                        obUsuario = new UserDTO() { correo = obUsuario.correo, password = obUsuario.password };

                        var content = new StringContent(JsonConvert.SerializeObject(obUsuario), Encoding.UTF8, "application/json");

                        var response1 = await cliente1.PostAsync("https://localhost:7132/api/validar", content);

                        var jsonRespuesta1 = await response1.Content.ReadAsStringAsync();

                        var obRespuesta = JsonConvert.DeserializeObject<Respuesta>(jsonRespuesta1);

                        //este ejecuta para visualizar toda la información

                        var cliente2 = new HttpClient();

                        cliente2.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", obRespuesta.token);

                        var response = await cliente2.GetAsync("https://localhost:7132/api/camion");

                        var test = await response.Content.ReadAsStringAsync();
                    }
                    await reader.CloseAsync();

                }
                catch (Exception ex)
                {
                    
                }

            }

        }
    }
}
