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
    public class Productos
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public int codProdMain { get; set; }
        public string descripcion { get; set; }
        public string precio { get; set; }
        public string ganancia { get; set; }
        public string iva { get; set; }
        public int categoria { get; set; }
        public string cod_bar { get; set; }
        public string utilidad1 { get; set; }
        public string utilidad2 { get; set; }
        public string codigo { get; set; }
        public int calcular_precio { get; set; }
        public string presentacion { get; set; }
    }

    public class ProductosServer
    {
        [JsonProperty(PropertyName = "id")]
        public int id { get; set; }

        [JsonProperty(PropertyName = "descripcion")]
        public string descripcion { get; set; }

        [JsonProperty(PropertyName = "precio")]
        public string precio { get; set; }

        [JsonProperty(PropertyName = "ganancia")]
        public string ganancia { get; set; }

        [JsonProperty(PropertyName = "iva")]
        public string iva { get; set; }

        [JsonProperty(PropertyName = "categoria")]
        public int categoria { get; set; }

        [JsonProperty(PropertyName = "cod_bar")]
        public string cod_bar { get; set; }

        [JsonProperty(PropertyName = "utilidad1")]
        public string utilidad1 { get; set; }

        [JsonProperty(PropertyName = "utilidad2")]
        public string utilidad2 { get; set; }

        [JsonProperty(PropertyName = "codigo")]
        public string codigo { get; set; }

        [JsonProperty(PropertyName = "calcular_precio")]
        public int calcular_precio { get; set; }

        [JsonProperty(PropertyName = "presentacion")]
        public string presentacion { get; set; }
        public override string ToString()
        {
            return descripcion;
        }
    }

    public class RespuestaServerProductos
    {
        [JsonProperty(PropertyName = "estado")]
        public string estado { get; set; }

        [JsonProperty(PropertyName = "mensaje")]
        public string mensaje { get; set; }

        [JsonProperty(PropertyName = "productos")]
        public List<ProductosServer> ProductosLista { get; set; }

        public override string ToString()
        {
            return mensaje;
        }

    }
}