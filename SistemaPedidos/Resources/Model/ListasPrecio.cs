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
    public class ListasPrecio
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string utilidad { get; set; }
        public int auxcol { get; set; }
        public override string ToString()
        {
            return nombre;
        }
    }
    public class ConfiguracionesVarias
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string valor { get; set; }
    }
    public class ConfiguracionesVariasServer
    {        
        [JsonProperty(PropertyName = "vendedor")]
        public string vendedor { get; set; }
        [JsonProperty(PropertyName = "activo")]
        public string activo { get; set; }        
        [JsonProperty(PropertyName = "version")]
        public string version { get; set; }
        public override string ToString()
        {
            return vendedor;
        }
    }
    public class RespuestaServerConfiguraciones
    {
        [JsonProperty(PropertyName = "estado")]
        public string estado { get; set; }

        [JsonProperty(PropertyName = "mensaje")]
        public string mensaje { get; set; }

        [JsonProperty(PropertyName = "configuraciones")]
        public List<ConfiguracionesVariasServer > ConfiguracionesServer { get; set; }

        public override string ToString()
        {
            return mensaje;
        }

    }

    public class ListasPrecioServer
    {
        [JsonProperty(PropertyName = "id")]
        public int id { get; set; }

        [JsonProperty(PropertyName = "nombre")]
        public string nombre { get; set; }

        [JsonProperty(PropertyName = "utilidad")]   
        public string utilidad { get; set; }

        [JsonProperty(PropertyName = "auxcol")]
        public int auxcol { get; set; }

        public override string ToString()
        {
            return nombre;
        }
    }
    public class RespuestaServerListasPrecio
    {
        [JsonProperty(PropertyName = "estado")]
        public string estado { get; set; }

        [JsonProperty(PropertyName = "mensaje")]
        public string mensaje { get; set; }

        [JsonProperty(PropertyName = "listas")]
        public List<ListasPrecioServer> ListasPrecio { get; set; }

        public override string ToString()
        {
            return mensaje;
        }

    }
    public class PromocionesDescuentos
    {
        public int id { get; set; }
        public string nombrepromo { get; set; }
        public int idproducto { get; set; }
        public int idcategoria { get; set; }
        public string compra_min { get; set; }
        public string descuento_porc { get; set; }
    }
    public class PromocionesDescuentosServer
    {
        [JsonProperty(PropertyName = "id")]
        public int id { get; set; }
        [JsonProperty(PropertyName = "nombrepromo")]
        public string nombrepromo { get; set; }
        [JsonProperty(PropertyName = "idproducto")]
        public int idproducto { get; set; }
        [JsonProperty(PropertyName = "idcategoria")]
        public int idcategoria { get; set; }
        [JsonProperty(PropertyName = "compra_min")]
        public string compra_min { get; set; }
        [JsonProperty(PropertyName = "descuento_porc")]
        public string  descuento_porc { get; set; }

    }
    public class RespuestaServerPromocionesDescuentos
    {
        [JsonProperty(PropertyName = "estado")]
        public string estado { get; set; }

        [JsonProperty(PropertyName = "mensaje")]
        public string mensaje { get; set; }

        [JsonProperty(PropertyName = "promos")]
        public List<PromocionesDescuentosServer> PromocionesDescuentos{ get; set; }

        public override string ToString()
        {
            return mensaje;
        }

    }
}