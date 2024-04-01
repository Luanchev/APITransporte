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

namespace FormularioWindows
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public class Usuario
        {
            public string Correo { get; set; }
            public string Password { get; set; }
        }
        public class Respuesta
        {
            public string Token { get; set; }
        }
        private void button1_Click(object sender, EventArgs e)
        {

            var cliente1 = new HttpClient();
            //UserDTO user

        }
    }
}
