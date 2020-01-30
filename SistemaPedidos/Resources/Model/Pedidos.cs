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
   public class PedidosMaster
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string fecha { get; set; }
        public int id_cliente { get; set; }
        public string subtotal { get; set; }
        public string iva105 { get; set; }
        public string iva21 { get; set; }
        public string total { get; set; }
        public string vendedor { get; set; }
        public string observaciones { get; set; }
        public int enviado { get; set; }
        public int finalizado { get; set; }
    }
   public  class PedidosDetalle
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public int id_master { get; set; }
        public int  codProdMain { get; set; }
        public string cod { get; set; }
        public string plu { get; set; }
        public string cantidad { get; set; }
        public string descripcion { get; set; }
        public string iva { get; set; }
        public string punit { get; set; }
        public string ptotal { get; set; }
    }
    public class PedidoMax
    {
        public int id { get; set; }
    }
    public class PedidoDetalleCant
    {
        public int cantidad { get; set; }
    }

    public class PedidosMasterServer
    {
       

        [JsonProperty(PropertyName = "fecha")]
        public string fecha { get; set; }

        [JsonProperty(PropertyName = "id_cliente")]
        public int id_cliente { get; set; }

        [JsonProperty(PropertyName = "subtotal")]
        public string subtotal { get; set; }

        [JsonProperty(PropertyName = "iva105")]
        public string iva105 { get; set; }

        [JsonProperty(PropertyName = "iva21")]
        public string iva21 { get; set; }

        [JsonProperty(PropertyName = "total")]
        public string total { get; set; }

        [JsonProperty(PropertyName = "vendedor")]
        public string vendedor { get; set; }

        [JsonProperty(PropertyName = "observaciones")]
        public string observaciones { get; set; }

        [JsonProperty(PropertyName = "observaciones2")]
        public int observaciones2 { get; set; }
        
    }
    public class PedidosDetalleServer
    {
        [JsonProperty(PropertyName = "cod")]
        public int codProdMain { get; set; }

        [JsonProperty(PropertyName = "plu")]
        public string plu { get; set; }         
        
        [JsonProperty(PropertyName = "cantidad")]
        public string cantidad { get; set; }

        [JsonProperty(PropertyName = "descripcion")]
        public string descripcion { get; set; }

        [JsonProperty(PropertyName = "iva")]
        public string iva { get; set; }

        [JsonProperty(PropertyName = "punit")]
        public string punit { get; set; }

        [JsonProperty(PropertyName = "ptotal")]
        public string ptotal { get; set; }

        [JsonProperty(PropertyName = "codint")]
        public int id_master { get; set; }
    }
    public class RespuestaServerPedidos
    {
        [JsonProperty(PropertyName = "estado")]
        public string estado { get; set; }

        [JsonProperty(PropertyName = "mensaje")]
        public string mensaje { get; set; }        
    }
}