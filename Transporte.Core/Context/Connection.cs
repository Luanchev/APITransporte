using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transporte.Core.Context
{
    public class Connection
    {
        private static IConfiguration configuration;
        private static NpgsqlConnection objConexion;
        private static string error;
        private static string connection;
        private static Queue<String> connectionName = new Queue<string>();

        public NpgsqlConnection ConnectBD(IConfiguration configuration, string name = null)
        {
            if (name != null) connection = name;
            return new NpgsqlConnection(configuration.GetConnectionString("BDConnection"));
        }
        public static void setConnection(String name)
        {
            connection = name;
        }

    }
}
