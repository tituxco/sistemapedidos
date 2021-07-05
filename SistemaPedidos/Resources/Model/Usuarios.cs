using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using SQLite;

namespace SistemaPedidos.Resources.Model
{
    public class Usuarios
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int IdClienteServer { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string usuario { get; set; }
        public string contraseña { get; set; }
        //public int activo { get; set; }
        public int vendedor { get; set; }   
        public int activo { get; set; }
    }    
    public class UsuariosServer
    {
        [JsonProperty(PropertyName = "id")]
        public int id { get; set; }
        [JsonProperty(PropertyName = "activo")]
        public int activo { get; set; }
    }
    public class RespuestaServerUsuarios
    {
        [JsonProperty(PropertyName = "estado")]
        public string estado { get; set; }

        [JsonProperty(PropertyName = "mensaje")]
        public string mensaje { get; set; }

        [JsonProperty(PropertyName = "UsuarioEstado")]
        public List<UsuariosServer> UsuariosServers { get; set; }

        public override string ToString()
        {
            return mensaje;
        }
    }
}