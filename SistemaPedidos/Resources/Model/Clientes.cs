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
    public class TablaClientes
    {
        [PrimaryKey, AutoIncrement]
        public int idclientes { get; set; }
        public int codclieMain { get; set; }
        public string nomapell_razon { get; set; }
        public string domicilio { get; set; }
        public int localidad { get; set; }
        public int iva_tipo { get; set; }
        public string cuit { get; set; }
        public string telefono { get; set; }
        public string contacto { get; set; }
        public string celular { get; set; }
        public string email { get; set; }
        public string observaciones { get; set; }
        public int lista_precios { get; set; }
        public int vendedor { get; set; }        
    }

    public class ClienteServer
    {
        [JsonProperty(PropertyName = "idclientes")]
        public int idclientes { get; set; }

        [JsonProperty(PropertyName = "nomapell_razon")]
        public string nomapell_razon { get; set; }

        [JsonProperty(PropertyName = "direccion")]
        public string direccion { get; set; }

        [JsonProperty(PropertyName = "telefono")]
        public string telefono { get; set; }

        [JsonProperty(PropertyName = "localidad")]
        public int localidad { get; set; }

        [JsonProperty(PropertyName = "iva_tipo")]
        public int iva_tipo { get; set; }

        [JsonProperty(PropertyName = "cuit")]
        public string cuit { get; set; }

        [JsonProperty(PropertyName = "contacto")]
        public string contacto { get; set; }

        [JsonProperty(PropertyName = "celular")]
        public string celular { get; set; }

        [JsonProperty(PropertyName = "email")]
        public string email { get; set; }

        [JsonProperty(PropertyName = "observaciones")]
        public string observaciones { get; set; }

        [JsonProperty(PropertyName = "lista_precios")]
        public int lista_precios { get; set; }

        [JsonProperty(PropertyName = "vendedor")]
        public int vendedor { get; set; }
        
        public override string ToString()
        {
            return nomapell_razon;
        }

    }

    public class RespuestaServerClientes
    {
        [JsonProperty(PropertyName = "estado")]
        public string estado { get; set; }

        [JsonProperty(PropertyName = "mensaje")]
        public string mensaje { get; set; }

        [JsonProperty(PropertyName = "clientes")]
        public List<ClienteServer> ClientesLista { get; set; }

        public override string ToString()
        {
            return mensaje;
        }

    }
}