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
    public class CategoriaProductos
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string nombre { get; set; }
        public override string ToString()
        {
            return nombre;
        }
    }
    public class CategoriaProductosServer
    {
        [JsonProperty(PropertyName = "id")]
        public int id { get; set; }

        [JsonProperty(PropertyName = "nombre")]
        public string nombre { get; set; }

        public override string ToString()
        {
            return nombre;
        }
    }
    public class RespuestaServerCategProductos
    {
        [JsonProperty(PropertyName = "estado")]
        public string estado { get; set; }

        [JsonProperty(PropertyName = "mensaje")]
        public string mensaje { get; set; }

        [JsonProperty(PropertyName = "categorias")]
        public List<CategoriaProductosServer> ListaCategoriaProductos { get; set; }

        public override string ToString()
        {
            return mensaje;
        }

    }

}