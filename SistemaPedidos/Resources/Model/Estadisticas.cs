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
    public class EstadisticasVenta
    {
        [JsonProperty(PropertyName = "totVentas")]
        public string totVentas { get; set; }

        [JsonProperty(PropertyName = "comision")]
        public string comision { get; set; }
        public override string ToString()
        {
            return totVentas;
        }

    }
    public class RespuestaEstadisticasVenta
    {
        [JsonProperty(PropertyName = "estado")]
        public string estado { get; set; }

        [JsonProperty(PropertyName = "mensaje")]
        public string mensaje { get; set; }

        [JsonProperty(PropertyName = "estadisticasVenta")]
        public List<EstadisticasVenta> estadisticasVenta { get; set; }

        public override string ToString()
        {
            return mensaje;
        }

    }

    public class EstadisticasDevolucion
    {
        [JsonProperty(PropertyName = "totDevolucion")]
        public string totDevolucion { get; set; }

        public override string ToString()
        {
            return totDevolucion;
        }

    }
    public class RespuestaEstadisticasDevolucion
    {
        [JsonProperty(PropertyName = "estado")]
        public string estado { get; set; }

        [JsonProperty(PropertyName = "mensaje")]
        public string mensaje { get; set; }

        [JsonProperty(PropertyName = "estadisticasDevolucion")]
        public List<EstadisticasDevolucion> estadisticasDevolucion { get; set; }

        public override string ToString()
        {
            return mensaje;
        }

    }

    public class EstadisticasVentasComisObjetivo
    {
        [JsonProperty(PropertyName = "tipoComis")]
        public string tipoComis { get; set; }
        public string montoVenta { get; set; }
        public string porcComision { get; set; }
        public string montoComision { get; set; }

        public override string ToString()
        {
            return montoComision;
        }

    }
    public class RespuestaVentaComisObjetivo
    {
        [JsonProperty(PropertyName = "estado")]
        public string estado { get; set; }

        [JsonProperty(PropertyName = "mensaje")]
        public string mensaje { get; set; }

        [JsonProperty(PropertyName = "estadisticasVentaComisObjetivo")]
        public List<EstadisticasVentasComisObjetivo> estadisticasVentasComisObjetivo { get; set; }

        public override string ToString()
        {
            return mensaje;
        }

    }

    public class EstadisticasDevolucionComisObjetivo
    {
        [JsonProperty(PropertyName = "tipoComis")]
        public string tipoComis { get; set; }
        public string montoDevolucion { get; set; }
        public string porcComision { get; set; }
        public string montoComision { get; set; }

        public override string ToString()
        {
            return montoComision;
        }

    }
    public class RespuestaDevolucionComisObjetivo
    {
        [JsonProperty(PropertyName = "estado")]
        public string estado { get; set; }

        [JsonProperty(PropertyName = "mensaje")]
        public string mensaje { get; set; }

        [JsonProperty(PropertyName = "estadisticasDevolucionComisObjetivo")]
        public List<EstadisticasDevolucionComisObjetivo>EstadisticasDevolucionComisObjetivo { get; set; }

        public override string ToString()
        {
            return mensaje;
        }

    }

}